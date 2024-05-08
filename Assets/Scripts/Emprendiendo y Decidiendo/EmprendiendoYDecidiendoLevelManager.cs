using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmprendiendoYDecidiendoLevelManager : MonoBehaviour
{
    private int currentLevel = 1;
    private int maxLevel = 10;
    private int puntaje = 0;

    [SerializeField] private EmprendiendoYDecidiendoInformacion emprendiendoYDecidiendoInformacion;
    [SerializeField] private EmprendiendoYDecidiendoPresupuesto emprendiendoYDecidiendoPresupuesto;
    [SerializeField] private GameObject panelFinDeJuego;
    [SerializeField] private GameObject panelFinDeNivel;

    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public int MaxLevel { get => maxLevel; set => maxLevel = value; }
    public int Puntaje { get => puntaje; set => puntaje = value; }

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

}
