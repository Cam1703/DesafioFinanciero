using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EmprendiendoYDecidiendoPanelMesCompletado : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text tituloText;
    [SerializeField] TMP_Text puntajeText;
    [SerializeField] TMP_Text mesActual;

    private int puntaje;  //viene de EmprendiendoYDecidiendoLevelManager
    private int mes; //viene de EmprendiendoYDecidiendoLevelManager
    private int maxMes;  //viene de EmprendiendoYDecidiendoLevelManager

    [SerializeField] private EmprendiendoYDecidiendoLevelManager emprendiendoYDecidiendoLevelManager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCerrarPanel()
    {
        gameObject.SetActive(false); 
    }

    public void ActualizarInfoPanel()
    {
        puntaje = emprendiendoYDecidiendoLevelManager.Puntaje;
        mes = emprendiendoYDecidiendoLevelManager.CurrentLevel;
        maxMes = emprendiendoYDecidiendoLevelManager.MaxLevel;

        mesActual.text = "Mes " + mes + " de " + maxMes;
        puntajeText.text = "Puntaje: " + puntaje;
        tituloText.text = "¡Mes " + mes + " completado!";
    }
}
