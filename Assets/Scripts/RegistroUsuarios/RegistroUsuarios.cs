using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class Usuario
{
    private static int proximoId = 1;
    public int id;
    public string usuario;
    public string contrasena;
    public string nombres;
    public string apellidos;
    public string codigoDeClase;
    public bool isProfesor;

    public Usuario(string usuario, string contrasena, string nombres, string apellidos, string codigoDeClase, bool isProfesor)
    {
        this.id = proximoId++;
        this.usuario = usuario;
        this.contrasena = contrasena;
        this.nombres = nombres;
        this.apellidos = apellidos;
        this.codigoDeClase = codigoDeClase;
        this.isProfesor = isProfesor;
    }

    public Usuario(Usuario usuario)
    {
        this.id = proximoId++;
        this.usuario = usuario.usuario;
        contrasena = usuario.contrasena;
        nombres = usuario.nombres;
        apellidos = usuario.apellidos;
        codigoDeClase = usuario.codigoDeClase;
        isProfesor = usuario.isProfesor;
    }
}

public class RegistroUsuarios : MonoBehaviour
{
    private bool isProfesor;

    [SerializeField] private GameObject panelRegistro;
    [SerializeField] private GameObject panelSeleccionarTipoRegistro;

    [SerializeField] private TMP_InputField usuarioInput;
    [SerializeField] private TMP_InputField contrasenaInput;
    [SerializeField] private TMP_InputField nombresInput;
    [SerializeField] private TMP_InputField apellidsoInput;
    [SerializeField] private TMP_InputField codigoDeClaseInput;
    [SerializeField] private TMP_Text codigoDeClaseText;

    [SerializeField] private Button botonRegistrar;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        SaveSystem.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        botonRegistrar.enabled = false;
        panelSeleccionarTipoRegistro.SetActive(true);
        panelRegistro.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ValidarCampos();
    }

    public void SetIsProfesor(bool tipo)
    {
        isProfesor = tipo;
        panelRegistro.SetActive(true);
        panelSeleccionarTipoRegistro.SetActive(false);
        if (isProfesor)
        {
            codigoDeClaseInput.gameObject.SetActive(false);
            codigoDeClaseText.gameObject.SetActive(false);
        }
    }

    public void RegistrarUsuario()
    {
        Usuario usuario = new Usuario(usuarioInput.text, contrasenaInput.text, nombresInput.text, apellidsoInput.text, codigoDeClaseInput.text, isProfesor);

        Debug.Log("Usuario registrado: " + usuario.usuario);
        gameManager.GuardarUsuario(usuario);
    }

    public void ValidarCampos()
    {
        if (usuarioInput.text != "" && contrasenaInput.text != "" && nombresInput.text != "" && apellidsoInput.text != "" && (isProfesor ? true : codigoDeClaseInput.text != ""))
        {
            botonRegistrar.enabled = true;
            botonRegistrar.GetComponent<Image>().color = Color.white;
            botonRegistrar.GetComponentInChildren<TMP_Text>().text = "Registrarme";
        }
        else
        {
            botonRegistrar.enabled = false;
            botonRegistrar.GetComponent<Image>().color = Color.gray;
            botonRegistrar.GetComponentInChildren<TMP_Text>().text = "Llene todos los campos";
        }
    }

    public void Guardar()
    {

    }
}
