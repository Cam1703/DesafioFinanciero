using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Juego2Configuraciones
{
    public bool habilitarSeccionOfertantesYDemandantes;
    public int puntosRespuestaCorrecta;
    public int puntosRespuestaIncorrecta;
    public int puntajeAprobatorio;
    public bool habilitarPuntosEnContra;
    public int cantidadDePreguntas;

    public Juego2Configuraciones(bool habilitarSeccionOfertantesYDemandantes, int puntosRespuestaCorrecta, int puntosRespuestaIncorrecta, int puntajeAprobatorio, bool habilitarPuntosEnContra, int cantidadDePreguntas)
    {
        this.habilitarSeccionOfertantesYDemandantes = habilitarSeccionOfertantesYDemandantes;
        this.puntosRespuestaCorrecta = puntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = puntosRespuestaIncorrecta;
        this.puntajeAprobatorio = puntajeAprobatorio;
        this.habilitarPuntosEnContra = habilitarPuntosEnContra;
        this.cantidadDePreguntas = cantidadDePreguntas;
    }

    public Juego2Configuraciones(Juego2Configuraciones configuraciones)
    {
        this.habilitarSeccionOfertantesYDemandantes = configuraciones.habilitarSeccionOfertantesYDemandantes;
        this.puntosRespuestaCorrecta = configuraciones.puntosRespuestaCorrecta;
        this.puntosRespuestaIncorrecta = configuraciones.puntosRespuestaIncorrecta;
        this.puntajeAprobatorio = configuraciones.puntajeAprobatorio;
        this.habilitarPuntosEnContra = configuraciones.habilitarPuntosEnContra;
        this.cantidadDePreguntas = configuraciones.cantidadDePreguntas;
    }
}

public class ConfiguracionesJuego2 : MonoBehaviour
{
    //Tematicas de juego
    private bool habilitarSeccionOfertantesYDemandantes = true;

    //Caracteristicas de juego
    private int puntosRespuestaCorrecta = 100;
    private int puntosRespuestaIncorrecta = 50;
    private int puntajeAprobatorio = 400;
    private bool habilitarPuntosEnContra = true;
    private int cantidadDePreguntas = 6;

    //Tematicas de juego UI
    [SerializeField] private Toggle toggleSeccionOfertantesYDemandantes;

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
        Juego2Configuraciones configuraciones = gameManager.GetSalonActual().juego2Configuraciones;
        toggleSeccionOfertantesYDemandantes.isOn = configuraciones.habilitarSeccionOfertantesYDemandantes;
        inputFieldPuntosRespuestaCorrecta.text = configuraciones.puntosRespuestaCorrecta.ToString();
        inputFieldCantidadDePreguntas.text = configuraciones.cantidadDePreguntas.ToString();
        inputFieldPuntosRespuestaIncorrecta.text = configuraciones.puntosRespuestaIncorrecta.ToString();
        inputFieldPuntajeAprobatorio.text = configuraciones.puntajeAprobatorio.ToString();
        toggleHabilitarPuntosEnContra.isOn = configuraciones.habilitarPuntosEnContra;


    }

    public void GuardarConfiguracion()
    {
        habilitarSeccionOfertantesYDemandantes = toggleSeccionOfertantesYDemandantes.isOn;

        puntosRespuestaCorrecta = int.Parse(inputFieldPuntosRespuestaCorrecta.text);
        puntosRespuestaIncorrecta = int.Parse(inputFieldPuntosRespuestaIncorrecta.text);
        puntajeAprobatorio = int.Parse(inputFieldPuntajeAprobatorio.text);
        habilitarPuntosEnContra = toggleHabilitarPuntosEnContra.isOn;
        cantidadDePreguntas = int.Parse(inputFieldCantidadDePreguntas.text);

        Juego2Configuraciones configuraciones = new Juego2Configuraciones(habilitarSeccionOfertantesYDemandantes , puntosRespuestaCorrecta, puntosRespuestaIncorrecta, puntajeAprobatorio, habilitarPuntosEnContra, cantidadDePreguntas);
        Salon salon = gameManager.GetSalonActual();
        salon.juego2Configuraciones = new Juego2Configuraciones(configuraciones);
        gameManager.SetSalonActual(salon);
        SaveSystem.UpdateSalon(salon);
    }
}
