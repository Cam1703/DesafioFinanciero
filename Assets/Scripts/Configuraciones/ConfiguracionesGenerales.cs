using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguracionesGenerales : MonoBehaviour
{

    [SerializeField] private TMP_InputField usuarioInput;
    [SerializeField] private TMP_InputField contrasenaInput;
    [SerializeField] private TMP_InputField nombresInput;
    [SerializeField] private TMP_InputField apellidsoInput;
    [SerializeField] private TMP_InputField codigoDeClaseInput;
    [SerializeField] private TMP_Text codigoDeClaseText;

    [SerializeField] private Button buttonGuardar;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject buttonTabsPanel;
    private Usuario usuarioActual;
    
    // Start is called before the first frame update
    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        Debug.Log("Usuario actual: " + usuarioActual);
        if (usuarioActual != null)
        {
            usuarioInput.text = usuarioActual.usuario;
            contrasenaInput.text = usuarioActual.contrasena;
            nombresInput.text = usuarioActual.nombres;
            apellidsoInput.text = usuarioActual.apellidos;
            if(!usuarioActual.isProfesor) // Si no es profesor
            {
                codigoDeClaseInput.gameObject.SetActive(true);
                codigoDeClaseText.gameObject.SetActive(true);
                codigoDeClaseInput.text = usuarioActual.codigoDeClase;
                buttonTabsPanel.SetActive(false);
            }
            else
            {
                codigoDeClaseInput.gameObject.SetActive(false);
                codigoDeClaseText.gameObject.SetActive(false);
            }
        }
    }

    public void GuardarCambios()
    {
        Usuario usuario = gameManager.GetUsuarioActual();
        usuario.usuario = usuarioInput.text;
        usuario.contrasena = contrasenaInput.text;
        usuario.nombres = nombresInput.text;
        usuario.apellidos = apellidsoInput.text;
        usuario.codigoDeClase = codigoDeClaseInput.text;
        Console.WriteLine("Usuario modificado: " + usuario);
        SaveSystem.ModifyUser(usuario);
        gameManager.SetUsuarioActual(usuario);
    }
}
