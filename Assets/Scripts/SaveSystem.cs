using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Persistencia sobre Cloud Firestore vía su API REST (ver Assets/Scripts/Firebase/).
/// Antes esto leía/escribía Assets/Saves/usuarios.json y salones.json enteros en
/// cada llamada; ahora cada método es una operación de red puntual contra Firestore,
/// por eso todo es async. Los nombres de los métodos se mantuvieron lo más parecido
/// posible a los originales para minimizar el cambio en quien los llama.
/// </summary>
public static class SaveSystem
{
    private const string UsuariosCollection = "usuarios";
    private const string SalonesCollection = "salones";

    private static FirestoreRestClient firestore;
    private static bool initialized;

    public static void Init()
    {
        if (initialized) return;

        var auth = new FirebaseAuthRestClient(FirebaseConfig.WebApiKey);
        firestore = new FirestoreRestClient(FirebaseConfig.ProjectId, FirebaseConfig.WebApiKey, auth);
        initialized = true;
    }

    // ---------- Usuarios ----------

    public static async Task SaveUserAsync(Usuario usuario)
    {
        await firestore.UpsertDocumentAsync($"{UsuariosCollection}/{usuario.id}", usuario);
    }

    /// <summary>Alias de SaveUserAsync: en Firestore "guardar" y "modificar" son la misma operación (upsert).</summary>
    public static async Task ModifyUserAsync(Usuario usuario)
    {
        await SaveUserAsync(usuario);
    }

    /// <summary>Busca por nombre de usuario (username), no por id. Devuelve null si no existe.</summary>
    public static async Task<Usuario> BuscarUsuarioAsync(string usuario)
    {
        var filters = new Dictionary<string, object> { ["usuario"] = usuario };
        List<JObject> docs = await firestore.QueryAsync(UsuariosCollection, filters);
        return docs.Count > 0 ? FirestoreValue.FromDocument<Usuario>(docs[0]) : null;
    }

    /// <summary>Alumnos (no profesores) inscritos en un salón, para la tabla de GestionAlumnos.</summary>
    public static async Task<List<Usuario>> LoadAlumnosDeSalonAsync(string codigoSalon)
    {
        var filters = new Dictionary<string, object>
        {
            ["codigoDeClase"] = codigoSalon,
            ["isProfesor"] = false
        };
        List<JObject> docs = await firestore.QueryAsync(UsuariosCollection, filters);
        return docs.Select(FirestoreValue.FromDocument<Usuario>).ToList();
    }

    // ---------- Salones ----------

    public static async Task SaveSalonAsync(Salon salon)
    {
        await firestore.UpsertDocumentAsync($"{SalonesCollection}/{salon.codigoSalon}", salon);
    }

    /// <summary>Alias de SaveSalonAsync (mismo motivo que ModifyUserAsync).</summary>
    public static async Task UpdateSalonAsync(Salon salon)
    {
        await SaveSalonAsync(salon);
    }

    public static async Task<Salon> GetSalonByCodigoAsync(string codigoSalon)
    {
        JObject doc = await firestore.GetDocumentAsync($"{SalonesCollection}/{codigoSalon}");
        return doc != null ? FirestoreValue.FromDocument<Salon>(doc) : null;
    }

    public static async Task<bool> ExisteSalonAsync(string codigoSalon)
    {
        return await GetSalonByCodigoAsync(codigoSalon) != null;
    }

    public static async Task DeleteSalonAsync(string codigoSalon)
    {
        await firestore.DeleteDocumentAsync($"{SalonesCollection}/{codigoSalon}");
    }

    public static async Task<List<Salon>> LoadSalonesAsync(string profesorId)
    {
        var filters = new Dictionary<string, object> { ["profesorId"] = profesorId };
        List<JObject> docs = await firestore.QueryAsync(SalonesCollection, filters);
        return docs.Select(FirestoreValue.FromDocument<Salon>).ToList();
    }
}
