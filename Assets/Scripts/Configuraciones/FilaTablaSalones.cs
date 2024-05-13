using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilaTablaSalones : MonoBehaviour
{
    [SerializeField] private Button botonGestionar;
    [SerializeField] private Button botonEliminar;
    [SerializeField] private TMP_Text codigoSalonText;

    private GameManager gameManager;
    private GestionDeSalones gestionDeSalones;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gestionDeSalones = FindObjectOfType<GestionDeSalones>();
    }


    public void GestionarSalon()
    {
        Debug.Log("Gestionando salon: " + codigoSalonText.text);
        Salon salon = gestionDeSalones.GetSalonByCodigo(codigoSalonText.text);
        gameManager.SetSalonActual(salon);
        gameManager.CambiarEscena("GestionarSalon");
    }

    public void EliminarSalon()
    {
        Debug.Log("Eliminando salon: " + codigoSalonText.text);
        SaveSystem.DeleteSalon(codigoSalonText.text);
        gestionDeSalones.MostrarSalonesEnTabla();
    }
}
