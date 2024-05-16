using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointManager : MonoBehaviour
{
    private int points = 0;

    [SerializeField] private TMP_Text text;
    public int pairCounter = 0;
    private int pairCounterMax;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text puntajeFinal;
    private GameObject[] demandantes;

    //Configuraciones de juego
    private Usuario usuarioActual;
    [SerializeField] GameManager gameManager;
    private int puntosParejaCorrecta = 10;
    private int puntosParejaIncorrecta = 5;
    private int nroDeParejas = 4;
    private int puntajeAprobatorio = 100;

    // Start is called before the first frame update
    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        Juego2Configuraciones configuraciones = SaveSystem.GetConfiguracionesJuego2PorSalon(codSalon);
        puntosParejaCorrecta = configuraciones.puntosRespuestaCorrecta;
        puntosParejaIncorrecta = configuraciones.puntosRespuestaIncorrecta;
        nroDeParejas = configuraciones.cantidadDePreguntas;
        puntajeAprobatorio = configuraciones.puntajeAprobatorio;

        demandantes = GameObject.FindGameObjectsWithTag("Demandante");
        //eliminar demandantes segun configuracion
    
        for (int i = 0; i < demandantes.Length - nroDeParejas; i++)
        {
            Destroy(demandantes[i]);
            Debug.Log("Eliminando demandante" + demandantes[i].name);
        }

        text.text = "Puntos: " + points.ToString();
        puntajeFinal.text = "Puntaje final: " + points.ToString();
        pairCounterMax = nroDeParejas;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPoint()
    {
        points += puntosParejaCorrecta;
        text.text = "Puntos: " + points.ToString();
        puntajeFinal.text = "Puntaje final: " + points.ToString();
    }

    public void subtractPoint()
    {
        points -= puntosParejaIncorrecta;
        text.text = "Puntos: " + points.ToString();
        puntajeFinal.text = "Puntaje final: " + points.ToString();
    }

    public void addPairCounter()
    {
        pairCounter++;
        if (pairCounter == pairCounterMax)
        {
            // Guarda el puntaje en el usuario actual
            if (usuarioActual.puntajesMaximos.puntajeMaximoJuego2 < points)
            {
                usuarioActual.puntajesMaximos.puntajeMaximoJuego2 = points;
            }
            if (puntajeAprobatorio <= points)
            {
                usuarioActual.puntajesMaximos.juego2Aprobado = true;
            }

            SaveSystem.ModifyUser(usuarioActual);
            winPanel.SetActive(true);
        }
    }
}
