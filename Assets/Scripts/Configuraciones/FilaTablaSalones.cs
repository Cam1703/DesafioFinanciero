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


    public async void GestionarSalon()
    {
        Debug.Log("Gestionando salon: " + codigoSalonText.text);
        Salon salon = await gestionDeSalones.GetSalonByCodigoAsync(codigoSalonText.text);
        gameManager.SetSalonActual(salon);
        gameManager.CambiarEscena("GestionarSalon");
    }

    public async void EliminarSalon()
    {
        Debug.Log("Eliminando salon: " + codigoSalonText.text);
        await SaveSystem.DeleteSalonAsync(codigoSalonText.text);
        await gestionDeSalones.MostrarSalonesEnTabla();
    }
}
