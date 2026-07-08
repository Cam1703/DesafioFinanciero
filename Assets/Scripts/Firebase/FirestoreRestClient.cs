using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class FirestoreException : Exception
{
    public long StatusCode { get; }

    public FirestoreException(string message, long statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

/// <summary>
/// Cliente mínimo para la API REST de Cloud Firestore (https://firestore.googleapis.com/v1/...).
/// Se usa en vez del SDK nativo de Firebase porque ese SDK solo soporta Desktop
/// (Windows/Mac/Linux) dentro del Editor de Unity para flujos de desarrollo, no en
/// builds exportados — ver la nota en la carpeta "02 Sistemas/Persistencia en Firebase"
/// del vault de Obsidian. La REST API funciona igual en cualquier plataforma porque es HTTPS puro.
/// </summary>
public class FirestoreRestClient
{
    private readonly string projectId;
    private readonly string apiKey;
    private readonly FirebaseAuthRestClient auth;

    private string DocumentsUrl => $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents";

    public FirestoreRestClient(string projectId, string apiKey, FirebaseAuthRestClient auth)
    {
        this.projectId = projectId;
        this.apiKey = apiKey;
        this.auth = auth;
    }

    /// <summary>Devuelve null si el documento no existe.</summary>
    public async Task<JObject> GetDocumentAsync(string relativePath)
    {
        string url = $"{DocumentsUrl}/{relativePath}?key={apiKey}";
        using var req = UnityWebRequest.Get(url);
        await AttachAuthAndSendAsync(req);

        if (req.responseCode == 404) return null;
        EnsureSuccess(req);
        return JObject.Parse(req.downloadHandler.text);
    }

    /// <summary>Crea o sobrescribe por completo un documento (equivalente a set(), no a un merge parcial).</summary>
    public async Task<JObject> UpsertDocumentAsync<T>(string relativePath, T data)
    {
        JObject fields = FirestoreValue.ToDocumentFields(data);
        var body = new JObject { ["fields"] = fields };

        string url = $"{DocumentsUrl}/{relativePath}?key={apiKey}";
        using var req = new UnityWebRequest(url, "PATCH");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body.ToString()));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        await AttachAuthAndSendAsync(req);
        EnsureSuccess(req);
        return JObject.Parse(req.downloadHandler.text);
    }

    public async Task DeleteDocumentAsync(string relativePath)
    {
        string url = $"{DocumentsUrl}/{relativePath}?key={apiKey}";
        using var req = UnityWebRequest.Delete(url);
        req.downloadHandler = new DownloadHandlerBuffer();

        await AttachAuthAndSendAsync(req);
        if (req.responseCode != 404) EnsureSuccess(req);
    }

    /// <summary>
    /// Consulta por igualdad (AND de todos los filtros). Alcanza para los casos de este
    /// proyecto (usuario==X, codigoDeClase==X, profesorId==X, isProfesor==false, etc.).
    /// </summary>
    public async Task<List<JObject>> QueryAsync(string collectionId, Dictionary<string, object> equalityFilters)
    {
        var filters = new JArray();
        foreach (KeyValuePair<string, object> kv in equalityFilters)
        {
            filters.Add(new JObject
            {
                ["fieldFilter"] = new JObject
                {
                    ["field"] = new JObject { ["fieldPath"] = kv.Key },
                    ["op"] = "EQUAL",
                    ["value"] = FirestoreValue.Encode(JToken.FromObject(kv.Value))
                }
            });
        }

        JToken where = filters.Count == 1
            ? filters[0]
            : new JObject { ["compositeFilter"] = new JObject { ["op"] = "AND", ["filters"] = filters } };

        var structuredQuery = new JObject
        {
            ["structuredQuery"] = new JObject
            {
                ["from"] = new JArray { new JObject { ["collectionId"] = collectionId } },
                ["where"] = where
            }
        };

        string url = $"{DocumentsUrl}:runQuery?key={apiKey}";
        using var req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(structuredQuery.ToString()));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        await AttachAuthAndSendAsync(req);
        EnsureSuccess(req);

        var results = new List<JObject>();
        var arr = JArray.Parse(req.downloadHandler.text);
        foreach (JToken item in arr)
        {
            if (item["document"] is JObject doc)
            {
                results.Add(doc);
            }
        }
        return results;
    }

    private async Task AttachAuthAndSendAsync(UnityWebRequest req)
    {
        if (auth != null)
        {
            string token = await auth.GetIdTokenAsync();
            req.SetRequestHeader("Authorization", "Bearer " + token);
        }
        await SendAsync(req);
    }

    private static Task SendAsync(UnityWebRequest req)
    {
        var tcs = new TaskCompletionSource<bool>();
        UnityWebRequestAsyncOperation op = req.SendWebRequest();
        op.completed += _ => tcs.SetResult(true);
        return tcs.Task;
    }

    private static void EnsureSuccess(UnityWebRequest req)
    {
        if (req.result != UnityWebRequest.Result.Success)
        {
            throw new FirestoreException($"Firestore error {req.responseCode}: {req.downloadHandler?.text}", req.responseCode);
        }
    }
}
