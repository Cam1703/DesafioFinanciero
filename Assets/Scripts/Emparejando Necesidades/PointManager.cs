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

    // Start is called before the first frame update
    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        Juego2Configuraciones configuraciones = SaveSystem.GetConfiguracionesJuego2PorSalon(codSalon);
        puntosParejaCorrecta = configuraciones.puntosRespuestaCorrecta;
        puntosParejaIncorrecta = configuraciones.puntosRespuestaIncorrecta;
        nroDeParejas = configuraciones.cantidadDePreguntas;

        demandantes = GameObject.FindGameObjectsWithTag("Demandante");
        //eliminar demandantes segun configuracion
    
        for (int i = 0; i < demandantes.Length - nroDeParejas; i++)
        {
            Destroy(demandantes[i]);
        }

        text.text = "Puntos: " + points.ToString();
        puntajeFinal.text = "Puntaje final: " + points.ToString();
        pairCounterMax = GameObject.FindGameObjectsWithTag(tag: "Demandante").Length;
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
            winPanel.SetActive(true);
        }
    }
}
