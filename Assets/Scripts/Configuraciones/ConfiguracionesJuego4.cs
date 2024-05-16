using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Juego4Configuraciones
{
    public bool habilitarSeguros;
    public bool habilitarPensiones;
    public int puntosRespuestaCorrecta;
    public int puntosRespuestaIncorrecta;
    public int puntajeAprobatorio;
    public bool habilitarPuntosEnContra;
    public int cantidadDePreguntas;

    public Juego4Configuraciones(bool habilitarSeguros, bool habilitarPensiones, int puntosRespuestaCorrecta, int puntosRespuestaIncorrecta, int puntajeAprobatorio, bool habilitarPuntosEnContra, int cantidadDePreguntas)
    {
        this.habilitarSeguros = habilitarSeguros;
        this.habilitarPensiones = habilitarPensiones;
        this.puntosRespuestaCorrecta = puntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = puntosRespuestaIncorrecta;
        this.puntajeAprobatorio = puntajeAprobatorio;
        this.habilitarPuntosEnContra = habilitarPuntosEnContra;
        this.cantidadDePreguntas = cantidadDePreguntas;
    }

    public Juego4Configuraciones(Juego4Configuraciones configuraciones)
    {
        this.habilitarSeguros = configuraciones.habilitarSeguros;
        this.habilitarPensiones = configuraciones.habilitarPensiones;
        this.puntosRespuestaCorrecta = configuraciones.puntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = configuraciones.puntosRespuestaIncorrecta;
        this.puntajeAprobatorio = configuraciones.puntajeAprobatorio;
        this.habilitarPuntosEnContra = configuraciones.habilitarPuntosEnContra;
        this.cantidadDePreguntas = configuraciones.cantidadDePreguntas;
    }
}
public class ConfiguracionesJuego4 : MonoBehaviour
{
    //Tematicas de juego
    private bool habilitarSeguros = true;
    private bool habilitarPensiones = true;

    //Caracteristicas de juego
    private int puntosRespuestaCorrecta = 100;
    private int puntosRespuestaIncorrecta = 50;
    private int puntajeAprobatorio = 400;
    private bool habilitarPuntosEnContra = true;
    private int cantidadDePreguntas = 6;

    //Tematicas de juego UI
    [SerializeField] private Toggle togglePensiones;
    [SerializeField] private Toggle toggleSeguros;

    //Caracteristicas de juego UI
    [SerializeField] private TMP_InputField inputFieldPuntosRespuestaCorrecta;
    [SerializeField] private TMP_InputField inputFieldPuntosRespuestaIncorrecta;
    [SerializeField] private TMP_InputField inputFieldPuntajeAprobatorio;
    [SerializeField] private Toggle toggleHabilitarPuntosEnContra;
    [SerializeField] private TMP_InputField inputFieldCantidadDePreguntas;

    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Juego4Configuraciones configuraciones = gameManager.GetSalonActual().juego4Configuraciones;
        togglePensiones.isOn = configuraciones.habilitarPensiones;
        toggleSeguros.isOn = configuraciones.habilitarSeguros;
        inputFieldPuntosRespuestaCorrecta.text = configuraciones.puntosRespuestaCorrecta.ToString();
        inputFieldCantidadDePreguntas.text = configuraciones.cantidadDePreguntas.ToString();
        inputFieldPuntosRespuestaIncorrecta.text = configuraciones.puntosRespuestaIncorrecta.ToString();
        inputFieldPuntajeAprobatorio.text = configuraciones.puntajeAprobatorio.ToString();
        toggleHabilitarPuntosEnContra.isOn = configuraciones.habilitarPuntosEnContra;


    }

    public void GuardarConfiguracion()
    {
        habilitarPensiones = togglePensiones.isOn;
        habilitarSeguros = toggleSeguros.isOn;

        puntosRespuestaCorrecta = int.Parse(inputFieldPuntosRespuestaCorrecta.text);
        puntosRespuestaIncorrecta = int.Parse(inputFieldPuntosRespuestaIncorrecta.text);
        puntajeAprobatorio = int.Parse(inputFieldPuntajeAprobatorio.text);
        habilitarPuntosEnContra = toggleHabilitarPuntosEnContra.isOn;
        cantidadDePreguntas = int.Parse(inputFieldCantidadDePreguntas.text);

        Juego4Configuraciones configuraciones = new Juego4Configuraciones(habilitarPensiones, habilitarSeguros, puntosRespuestaCorrecta, puntosRespuestaIncorrecta, puntajeAprobatorio, habilitarPuntosEnContra, cantidadDePreguntas);
        Salon salon = gameManager.GetSalonActual();
        salon.juego4Configuraciones = new Juego4Configuraciones(configuraciones);
        gameManager.SetSalonActual(salon);
        SaveSystem.UpdateSalon(salon);
    }
}
