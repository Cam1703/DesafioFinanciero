using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuInicio : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_InputField usuarioInput;
    [SerializeField] private TMP_InputField contrasenaInput;
    [SerializeField] private TMP_Text mensajeError;
    private List<Usuario> usuarios = new List<Usuario>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarSesion()
    {

        if(UsuarioRegistrado())
        {
            gameManager.CambiarEscena("MenuPrincipal");
        }
        else
        {
            mensajeError.text = "Usuario o contraseña incorrectos";
            mensajeError.gameObject.SetActive(true);
        }
    }

    private bool UsuarioRegistrado()
    {
        usuarios = SaveSystem.LoadUsers();

        foreach (Usuario usuario in usuarios)
        {
            if(usuario.usuario == usuarioInput.text && usuario.contrasena == contrasenaInput.text)
            {
                gameManager.SetUsuarioActual(usuario);
                return true;
            }
        }
        return false;
    }
}
