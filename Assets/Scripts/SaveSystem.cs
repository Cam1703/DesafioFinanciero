using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UsuarioData
{
    public List<Usuario> usuarios = new List<Usuario>();
}

public static class SaveSystem
{

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string USERS_FILE = "usuarios.json";
    private static readonly string usersFilePath = Path.Combine(SAVE_FOLDER, USERS_FILE);
    public static void Init()
    {
        // Crear la carpeta de guardado si no existe
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        // Verificar si el archivo de usuarios existe, y crearlo si no
        if (!File.Exists(usersFilePath))
        {
            File.Create(usersFilePath).Close(); // Create() devuelve un FileStream, así que cerramos el archivo después de crearlo
        }
    }

    public static void SaveUser(Usuario usuario)
    {
        UsuarioData usuarioData = new UsuarioData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(usersFilePath))
        {
            string json = File.ReadAllText(usersFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                usuarioData = JsonUtility.FromJson<UsuarioData>(json);
            }
        }

        // Agregar usuario y guardar
        usuarioData.usuarios.Add(new Usuario(usuario));
        string newDataJson = JsonUtility.ToJson(usuarioData);
        Debug.Log(newDataJson);
        File.WriteAllText(usersFilePath, newDataJson);
    }

    public static List<Usuario> LoadUsers()
    {
        UsuarioData usuarioData = new UsuarioData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(usersFilePath))
        {
            string json = File.ReadAllText(usersFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                usuarioData = JsonUtility.FromJson<UsuarioData>(json);
            }
        }

        return usuarioData.usuarios;
    }

    public static void DeleteUsers()
    {
        if (File.Exists(usersFilePath))
        {
            File.Delete(usersFilePath);
        }
    }

    public static void ModifyUser(Usuario usuario)
    {
        UsuarioData usuarioData = new UsuarioData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(usersFilePath))
        {
            string json = File.ReadAllText(usersFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                usuarioData = JsonUtility.FromJson<UsuarioData>(json);
            }
        }

        // Buscar usuario y modificarlo
        for (int i = 0; i < usuarioData.usuarios.Count; i++)
        {
            if (usuarioData.usuarios[i].id == usuario.id)
            {
                usuarioData.usuarios[i] = usuario;
                break;
            }
        }


        string newDataJson = JsonUtility.ToJson(usuarioData);
        File.WriteAllText(usersFilePath, newDataJson);

        Debug.Log("Usuario modificado: " + newDataJson);
    }
}
