using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UsuarioData
{
    public List<Usuario> usuarios = new List<Usuario>();
}

[System.Serializable]
public class SalonData
{
    public List<Salon> salones = new List<Salon>();
}

public static class SaveSystem
{

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string USERS_FILE = "usuarios.json";
    private static readonly string usersFilePath = Path.Combine(SAVE_FOLDER, USERS_FILE);
    public static readonly string SALONES_FILE = "salones.json";
    private static readonly string salonesFilePath = Path.Combine(SAVE_FOLDER, SALONES_FILE);
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


    public static void SaveSalon(Salon salon)
    {
        SalonData salonData = new SalonData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(salonesFilePath))
        {
            string json = File.ReadAllText(salonesFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                salonData = JsonUtility.FromJson<SalonData>(json);
            }
        }

        // Agregar salon y guardar
        salonData.salones.Add(new Salon(salon));
        string newDataJson = JsonUtility.ToJson(salonData);
        Debug.Log(newDataJson);
        File.WriteAllText(salonesFilePath, newDataJson);
    }

    public static List<Salon> LoadSalones(string profesorId)
    {
        SalonData salonData = new SalonData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(salonesFilePath))
        {
            string json = File.ReadAllText(salonesFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                salonData = JsonUtility.FromJson<SalonData>(json);
            }
        }

        //filtrar por id del profesor

        List<Salon> salones = new List<Salon>();
        foreach (Salon salon in salonData.salones)
        {
            if (salon.profesorId == profesorId)
            {
                salones.Add(salon);
            }
        }

        return salones;
    }

    public static void DeleteSalon(string codigoSalon)
    {
        SalonData salonData = new SalonData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(salonesFilePath))
        {
            string json = File.ReadAllText(salonesFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                salonData = JsonUtility.FromJson<SalonData>(json);
            }
        }

        // Buscar salon y eliminarlo
        for (int i = 0; i < salonData.salones.Count; i++)
        {
            if (salonData.salones[i].codigoSalon == codigoSalon)
            {
                salonData.salones.RemoveAt(i);
                break;
            }
        }

        string newDataJson = JsonUtility.ToJson(salonData);
        File.WriteAllText(salonesFilePath, newDataJson);

        Debug.Log("Salon eliminado: " + newDataJson);
    }

    public static Salon GetSalonByCodigo (string codigo)
    {
        SalonData salonData = new SalonData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(salonesFilePath))
        {
            string json = File.ReadAllText(salonesFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                salonData = JsonUtility.FromJson<SalonData>(json);
            }
        }

        // Buscar salon y retornarlo
        foreach (Salon salon in salonData.salones)
        {
            if (salon.codigoSalon == codigo)
            {
                return salon;
            }
        }

        return null;
    }

    public static void UpdateSalon(Salon salon)
    {
        SalonData salonData = new SalonData();

        // Cargar datos existentes o inicializar nuevos
        if (File.Exists(salonesFilePath))
        {
            string json = File.ReadAllText(salonesFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                salonData = JsonUtility.FromJson<SalonData>(json);
            }
        }

        // Buscar salon y modificarlo
        for (int i = 0; i < salonData.salones.Count; i++)
        {
            if (salonData.salones[i].codigoSalon == salon.codigoSalon)
            {
                salonData.salones[i] = salon;
                break;
            }
        }

        string newDataJson = JsonUtility.ToJson(salonData);
        File.WriteAllText(salonesFilePath, newDataJson);

    }

    public static Juego1Configuraciones GetConfiguracionesJuego1PorSalon(string codigoSalon)
    {
        Salon salon = GetSalonByCodigo(codigoSalon);
        return salon.juego1Configuraciones;
    }

    public static Juego2Configuraciones GetConfiguracionesJuego2PorSalon(string codigoSalon)
    {
        Salon salon = GetSalonByCodigo(codigoSalon);
        return salon.juego2Configuraciones;
    }

    public static Juego3Configuraciones GetConfiguracionesJuego3PorSalon(string codigoSalon)
    {
        Salon salon = GetSalonByCodigo(codigoSalon);
        return salon.juego3Configuraciones;
    }
}
