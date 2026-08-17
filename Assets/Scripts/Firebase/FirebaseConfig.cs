using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Lee Assets/StreamingAssets/firebase-config.json (projectId + webApiKey).
/// El Web API Key de Firebase no es secreto: la protección real la dan las
/// Reglas de Seguridad de Firestore, no ocultar esta clave.
/// </summary>
public static class FirebaseConfig
{
    private const string FileName = "firebase-config.json";
    private const string ResourceName = "firebase-config";

    private static string projectId;
    private static string webApiKey;
    private static bool loaded;

    public static string ProjectId
    {
        get { EnsureLoaded(); return projectId; }
    }

    public static string WebApiKey
    {
        get { EnsureLoaded(); return webApiKey; }
    }

    private static void EnsureLoaded()
    {
        if (loaded) return;

        TextAsset resourceConfig = Resources.Load<TextAsset>(ResourceName);
        if (resourceConfig != null)
        {
            ApplyJson(resourceConfig.text, "Resources/" + FileName);
            loaded = true;
            return;
        }

        string path = Path.Combine(Application.streamingAssetsPath, FileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"No se encontró {FileName} ni en Resources ni en StreamingAssets. Copia el archivo a Assets/Resources/{FileName} " +
                "o Assets/StreamingAssets/firebase-config.json y completa projectId/webApiKey con los datos de tu proyecto de Firebase.", path);
        }

        ApplyJson(File.ReadAllText(path), path);
        loaded = true;
    }

    private static void ApplyJson(string jsonText, string source)
    {
        JObject json = JObject.Parse(jsonText);
        projectId = json.Value<string>("projectId");
        webApiKey = json.Value<string>("webApiKey");

        if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(webApiKey))
        {
            throw new InvalidDataException(
                $"La configuración de Firebase en {source} existe pero projectId/webApiKey están vacíos. Complétalos con los datos de " +
                "Configuración del proyecto > General en la consola de Firebase.");
        }
    }
}
