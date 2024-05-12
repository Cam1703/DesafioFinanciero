using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    private static List<Usuario> usuarios = new List<Usuario>();
    public static Usuario usuarioActual;
    public void CambiarEscena(string nombreEscena)
    {
        Debug.Log("Cambiando a escena: " + nombreEscena);
        SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }

    public void Awake()
    {
        SaveSystem.Init();
        usuarios = SaveSystem.LoadUsers();
        Debug.Log("Usuarios cargados: " + usuarios.Count);
    }
    
    public void GuardarUsuario(Usuario usuario)
    {
        SaveSystem.SaveUser(usuario);
        usuarios = SaveSystem.LoadUsers();
        Debug.Log("Usuarios cargados: " + usuarios.Count);
    }

    public void SetUsuarioActual(Usuario usuario)
    {
        usuarioActual = new Usuario(usuario);
        Debug.Log("Usuario actual: " + usuarioActual);
    }

    public Usuario GetUsuarioActual()
    {
        return usuarioActual;
    }
}
