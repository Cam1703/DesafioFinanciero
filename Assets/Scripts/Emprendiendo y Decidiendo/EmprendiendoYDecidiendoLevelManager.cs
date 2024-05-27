using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmprendiendoYDecidiendoLevelManager : MonoBehaviour
{
    private int currentLevel = 1;
    private int maxLevel = 3;
    private int puntaje = 0;
    private int puntajeAprobatorio;

    [SerializeField] private EmprendiendoYDecidiendoInformacion emprendiendoYDecidiendoInformacion;
    [SerializeField] private EmprendiendoYDecidiendoPresupuesto emprendiendoYDecidiendoPresupuesto;
    [SerializeField] private GameObject panelFinDeJuego;
    [SerializeField] private GameObject panelFinDeNivel;
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public int MaxLevel { get => maxLevel; set => maxLevel = value; }
    public int Puntaje { get => puntaje; set => puntaje = value; }

    private Usuario usuarioActual;
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ////Inicializar configuraciones
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        Juego5Configuraciones juego5Configuraciones = SaveSystem.GetConfiguracionesJuego5PorSalon(codSalon);
        maxLevel = juego5Configuraciones.cantidadDeNiveles;
        puntajeAprobatorio = juego5Configuraciones.puntajeAprobatorio;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CambiarNivel()
    {
        if (currentLevel > 0 && currentLevel < maxLevel)
        {
            puntaje = emprendiendoYDecidiendoInformacion.CalcularPuntaje();
            emprendiendoYDecidiendoInformacion.CalcularCantidadDeClientes();
            emprendiendoYDecidiendoInformacion.CalcularGananciasMensuales();
            emprendiendoYDecidiendoInformacion.ActualizarInformacion();
            emprendiendoYDecidiendoPresupuesto.RealizarPagos();
            emprendiendoYDecidiendoPresupuesto.ActualizarPresupuesto();
            panelFinDeNivel.GetComponent<EmprendiendoYDecidiendoPanelMesCompletado>().ActualizarInfoPanel();
            panelFinDeNivel.SetActive(true);
            currentLevel++;
            Debug.Log("Cambiando a nivel: " + currentLevel);
        }
        else
        {
            panelFinDeJuego.SetActive(true);
            Debug.LogError("Fin de juego");
            // Guarda el puntaje en el usuario actual
            if (usuarioActual.puntajesMaximos.puntajeMaximoJuego5 < puntaje)
            {
                usuarioActual.puntajesMaximos.puntajeMaximoJuego5 = puntaje;
            }
            if (puntajeAprobatorio <= puntaje)
            {
                usuarioActual.puntajesMaximos.juego5Aprobado = true;
            }

            SaveSystem.ModifyUser(usuarioActual);
        }
    }

}
