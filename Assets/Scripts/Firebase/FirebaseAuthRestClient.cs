using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

/// <summary>
/// Autenticación anónima de Firebase vía la API REST de Identity Toolkit.
/// No reemplaza el login propio del juego (usuario/contraseña siguen viviendo en
/// Firestore) — esto solo obtiene un idToken para que las Reglas de Seguridad de
/// Firestore puedan exigir "request.auth != null" sin tener que usar Firebase
/// Authentication para las cuentas reales de alumnos/profesores.
/// </summary>
public class FirebaseAuthRestClient
{
    private readonly string apiKey;
    private string idToken;
    private string refreshToken;
    private DateTime expiresAtUtc;

    public FirebaseAuthRestClient(string apiKey)
    {
        this.apiKey = apiKey;
    }

    public async Task<string> GetIdTokenAsync()
    {
        if (idToken != null && DateTime.UtcNow < expiresAtUtc)
        {
            return idToken;
        }

        if (refreshToken != null)
        {
            await RefreshAsync();
        }
        else
        {
            await SignInAnonymouslyAsync();
        }

        return idToken;
    }

    private async Task SignInAnonymouslyAsync()
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}";
        var body = new JObject { ["returnSecureToken"] = true };

        using var req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body.ToString()));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        await SendAsync(req);

        if (req.result != UnityWebRequest.Result.Success)
        {
            throw new Exception($"No se pudo autenticar anónimamente con Firebase: {req.downloadHandler.text}");
        }

        JObject json = JObject.Parse(req.downloadHandler.text);
        ApplyTokenResponse(json.Value<string>("idToken"), json.Value<string>("refreshToken"), json.Value<string>("expiresIn"));
    }

    private async Task RefreshAsync()
    {
        string url = $"https://securetoken.googleapis.com/v1/token?key={apiKey}";
        string form = $"grant_type=refresh_token&refresh_token={refreshToken}";

        using var req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(form));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        await SendAsync(req);

        if (req.result != UnityWebRequest.Result.Success)
        {
            // El refresh token pudo expirar o ser revocado: reintenta con una sesión anónima nueva.
            refreshToken = null;
            await SignInAnonymouslyAsync();
            return;
        }

        JObject json = JObject.Parse(req.downloadHandler.text);
        ApplyTokenResponse(json.Value<string>("id_token"), json.Value<string>("refresh_token"), json.Value<string>("expires_in"));
    }

    private void ApplyTokenResponse(string newIdToken, string newRefreshToken, string expiresInSeconds)
    {
        idToken = newIdToken;
        refreshToken = newRefreshToken;
        double seconds = double.Parse(expiresInSeconds);
        // Se resta un margen de 60s para refrescar antes de que el token expire de verdad.
        expiresAtUtc = DateTime.UtcNow.AddSeconds(seconds - 60);
    }

    private static Task SendAsync(UnityWebRequest req)
    {
        var tcs = new TaskCompletionSource<bool>();
        UnityWebRequestAsyncOperation op = req.SendWebRequest();
        op.completed += _ => tcs.SetResult(true);
        return tcs.Task;
    }
}
