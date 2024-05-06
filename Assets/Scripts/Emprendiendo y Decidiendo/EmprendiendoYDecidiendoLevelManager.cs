using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmprendiendoYDecidiendoLevelManager : MonoBehaviour
{
    private int currentLevel = 1;
    private int maxLevel = 3;
    private int puntaje = 0;

    [SerializeField] private EmprendiendoYDecidiendoInformacion emprendiendoYDecidiendoInformacion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CambiarNivel(int nivel)
    {
        if (nivel > 0 && nivel <= maxLevel)
        {
            currentLevel = nivel;
            puntaje = emprendiendoYDecidiendoInformacion.CalcularPuntaje();
            emprendiendoYDecidiendoInformacion.ActualizarInformacion();
            Debug.Log("Cambiando a nivel: " + nivel);
        }
        else
        {
            Debug.LogError("Fin de juego");
        }
    }

}
