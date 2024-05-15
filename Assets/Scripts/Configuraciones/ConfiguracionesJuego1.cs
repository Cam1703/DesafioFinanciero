using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Juego1Configuraciones
{
    public bool habilitarSeccionOfertantesYDemandantes;
    public bool habilitarSeccionRecomendarPlanesFinancieros;
    public int puntosRespuestaCorrecta;
    public int puntosRespuestaIncorrecta;
    public int puntajeAprobatorio;
    public bool habilitarPuntosEnContra;
    public int cantidadDePreguntas;

    public Juego1Configuraciones(bool habilitarSeccionOfertantesYDemandantes, bool habilitarSeccionRecomendarPlanesFinancieros, int puntosRespuestaCorrecta, int puntosRespuestaIncorrecta, int puntajeAprobatorio, bool habilitarPuntosEnContra, int cantidadDePreguntas)
    {
        this.habilitarSeccionOfertantesYDemandantes = habilitarSeccionOfertantesYDemandantes;
        this.habilitarSeccionRecomendarPlanesFinancieros = habilitarSeccionRecomendarPlanesFinancieros;
        this.puntosRespuestaCorrecta = puntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = puntosRespuestaIncorrecta;
        this.puntajeAprobatorio = puntajeAprobatorio;
        this.habilitarPuntosEnContra = habilitarPuntosEnContra;
        this.cantidadDePreguntas = cantidadDePreguntas;
    }

    public Juego1Configuraciones(Juego1Configuraciones configuraciones)
    {
        this.habilitarSeccionOfertantesYDemandantes = configuraciones.habilitarSeccionOfertantesYDemandantes;
        this.habilitarSeccionRecomendarPlanesFinancieros = configuraciones.habilitarSeccionRecomendarPlanesFinancieros;
        this.puntosRespuestaCorrecta = configuraciones.puntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = configuraciones.puntosRespuestaIncorrecta;
        this.puntajeAprobatorio = configuraciones.puntajeAprobatorio;
        this.habilitarPuntosEnContra = configuraciones.habilitarPuntosEnContra;
        this.cantidadDePreguntas = configuraciones.cantidadDePreguntas;
    }
}

public class ConfiguracionesJuego1 : MonoBehaviour
{

    //Tematicas de juego
    private bool habilitarSeccionOfertantesYDemandantes = true;
    private bool habilitarSeccionRecomendarPlanesFinancieros = true;

    //Caracteristicas de juego
    private int puntosRespuestaCorrecta = 100;
    private int puntosRespuestaIncorrecta = 50;
    private int puntajeAprobatorio= 400;
    private bool habilitarPuntosEnContra = true;
    private int cantidadDePreguntas = 6;

    //Tematicas de juego UI
    [SerializeField] private Toggle toggleSeccionOfertantesYDemandantes;
    [SerializeField] private Toggle toggleSeccionRecomendarPlanesFinancieros;

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
        Juego1Configuraciones configuraciones = gameManager.GetSalonActual().juego1Configuraciones;
        toggleSeccionOfertantesYDemandantes.isOn = configuraciones.habilitarSeccionOfertantesYDemandantes;
        toggleSeccionRecomendarPlanesFinancieros.isOn = configuraciones.habilitarSeccionRecomendarPlanesFinancieros;
        inputFieldPuntosRespuestaCorrecta.text = configuraciones.puntosRespuestaCorrecta.ToString();
        inputFieldCantidadDePreguntas.text = configuraciones.cantidadDePreguntas.ToString();
        inputFieldPuntosRespuestaIncorrecta.text = configuraciones.puntosRespuestaIncorrecta.ToString();
        inputFieldPuntajeAprobatorio.text = configuraciones.puntajeAprobatorio.ToString();
        toggleHabilitarPuntosEnContra.isOn = configuraciones.habilitarPuntosEnContra;


    }

    public void GuardarConfiguracion()
    {
        habilitarSeccionOfertantesYDemandantes = toggleSeccionOfertantesYDemandantes.isOn;
        habilitarSeccionRecomendarPlanesFinancieros = toggleSeccionRecomendarPlanesFinancieros.isOn;
        
        puntosRespuestaCorrecta = int.Parse(inputFieldPuntosRespuestaCorrecta.text);
        puntosRespuestaIncorrecta = int.Parse(inputFieldPuntosRespuestaIncorrecta.text);
        puntajeAprobatorio = int.Parse(inputFieldPuntajeAprobatorio.text);
        habilitarPuntosEnContra = toggleHabilitarPuntosEnContra.isOn;
        cantidadDePreguntas = int.Parse(inputFieldCantidadDePreguntas.text);

        Juego1Configuraciones configuraciones = new Juego1Configuraciones(habilitarSeccionOfertantesYDemandantes, habilitarSeccionRecomendarPlanesFinancieros, puntosRespuestaCorrecta, puntosRespuestaIncorrecta, puntajeAprobatorio ,habilitarPuntosEnContra, cantidadDePreguntas);
        Salon salon = gameManager.GetSalonActual();
        salon.juego1Configuraciones = new Juego1Configuraciones(configuraciones);
        gameManager.SetSalonActual(salon);
        SaveSystem.UpdateSalon(salon);
    }
}
