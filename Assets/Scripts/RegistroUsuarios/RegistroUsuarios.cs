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
    public string id;
    public string usuario;
    public string contrasena;
    public string nombres;
    public string apellidos;
    public string codigoDeClase;
    public bool isProfesor;
    public PuntajeMaximoActualEnJuegos puntajesMaximos;
    public Usuario(string usuario, string contrasena, string nombres, string apellidos, string codigoDeClase, bool isProfesor, PuntajeMaximoActualEnJuegos puntajesMaximos)
    {
        id = Guid.NewGuid().ToString();
        this.usuario = usuario;
        this.contrasena = contrasena;
        this.nombres = nombres;
        this.apellidos = apellidos;
        this.codigoDeClase = codigoDeClase;
        this.isProfesor = isProfesor;
        this.puntajesMaximos = puntajesMaximos;
    }

    public Usuario(Usuario usuario)
    {
        id = usuario.id;
        this.usuario = usuario.usuario;
        contrasena = usuario.contrasena;
        nombres = usuario.nombres;
        apellidos = usuario.apellidos;
        codigoDeClase = usuario.codigoDeClase;
        isProfesor = usuario.isProfesor;
        puntajesMaximos = usuario.puntajesMaximos;
    }
}

[System.Serializable]
public class PuntajeMaximoActualEnJuegos
{
    public int puntajeMaximoJuego1;
    public int puntajeMaximoJuego2;
    public int puntajeMaximoJuego3;
    public int puntajeMaximoJuego4;
    public int puntajeMaximoJuego5;

    public bool juego1Aprobado;
    public bool juego2Aprobado;
    public bool juego3Aprobado;
    public bool juego4Aprobado;
    public bool juego5Aprobado;

    public PuntajeMaximoActualEnJuegos()
    {
        puntajeMaximoJuego1 = 0;
        puntajeMaximoJuego2 = 0;
        puntajeMaximoJuego3 = 0;
        puntajeMaximoJuego4 = 0;
        puntajeMaximoJuego5 = 0;

        juego1Aprobado = false;
        juego2Aprobado = false;
        juego3Aprobado = false;
        juego4Aprobado = false;
        juego5Aprobado = false;
    }

    public PuntajeMaximoActualEnJuegos(int puntajeMaximoJuego1, int puntajeMaximoJuego2, int puntajeMaximoJuego3, int puntajeMaximoJuego4, int puntajeMaximoJuego5, bool juego1Aprobado, bool juego2Aprobado, bool juego3Aprobado, bool juego4Aprobado, bool juego5Aprobado)
    {
        this.puntajeMaximoJuego1 = puntajeMaximoJuego1;
        this.puntajeMaximoJuego2 = puntajeMaximoJuego2;
        this.puntajeMaximoJuego3 = puntajeMaximoJuego3;
        this.puntajeMaximoJuego4 = puntajeMaximoJuego4;
        this.puntajeMaximoJuego5 = puntajeMaximoJuego5;

        this.juego1Aprobado = juego1Aprobado;
        this.juego2Aprobado = juego2Aprobado;
        this.juego3Aprobado = juego3Aprobado;
        this.juego4Aprobado = juego4Aprobado;
        this.juego5Aprobado = juego5Aprobado;
    }

    public PuntajeMaximoActualEnJuegos(PuntajeMaximoActualEnJuegos puntajesMaximos)
    {
        puntajeMaximoJuego1 = puntajesMaximos.puntajeMaximoJuego1;
        puntajeMaximoJuego2 = puntajesMaximos.puntajeMaximoJuego2;
        puntajeMaximoJuego3 = puntajesMaximos.puntajeMaximoJuego3;
        puntajeMaximoJuego4 = puntajesMaximos.puntajeMaximoJuego4;
        puntajeMaximoJuego5 = puntajesMaximos.puntajeMaximoJuego5;

        juego1Aprobado = puntajesMaximos.juego1Aprobado;
        juego2Aprobado = puntajesMaximos.juego2Aprobado;
        juego3Aprobado = puntajesMaximos.juego3Aprobado;
        juego4Aprobado = puntajesMaximos.juego4Aprobado;
        juego5Aprobado = puntajesMaximos.juego5Aprobado;
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
    [SerializeField] private TMP_Text mensajeErrorCodigoNoExiste;

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
        if (!isProfesor)
        {
            if (SaveSystem.GetSalonByCodigo(codigoDeClaseInput.text) == null)
            {
                mensajeErrorCodigoNoExiste.gameObject.SetActive(true);
                return;
            }
        }
        PuntajeMaximoActualEnJuegos puntajesMaximos = new PuntajeMaximoActualEnJuegos();
        Usuario usuario = new Usuario(usuarioInput.text, contrasenaInput.text, nombresInput.text, apellidsoInput.text, codigoDeClaseInput.text, isProfesor, puntajesMaximos);
        
        Debug.Log("Usuario registrado: " + usuario.usuario);
        gameManager.GuardarUsuario(usuario);
        gameManager.CambiarEscena("Inicio");
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

}
