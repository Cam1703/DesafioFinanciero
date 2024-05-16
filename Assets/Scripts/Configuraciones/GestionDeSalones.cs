using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

[System.Serializable]
public class Salon
{
    public string nombreSalon;
    public string codigoSalon;
    public string profesorId;
    public Juego1Configuraciones juego1Configuraciones;
    public Juego2Configuraciones juego2Configuraciones;
    public Juego3Configuraciones juego3Configuraciones;

    public Salon(string nombreSalon, string codigoSalon, string profesorId)
    {
        this.profesorId = profesorId;
        this.nombreSalon = nombreSalon;
        this.codigoSalon = codigoSalon;
        this.juego1Configuraciones = new Juego1Configuraciones(true, true, 100, 50, 400, true, 6);
        this.juego2Configuraciones = new Juego2Configuraciones(true, 100, 50, 300, true, 4);
        this.juego3Configuraciones = new Juego3Configuraciones(true, true, true, true, 5, 400);
    }

    public Salon (Salon salon)
    {
        this.profesorId = salon.profesorId;
        this.nombreSalon = salon.nombreSalon;
        this.codigoSalon = salon.codigoSalon;
        this.juego1Configuraciones = new Juego1Configuraciones(salon.juego1Configuraciones);
        this.juego2Configuraciones = new Juego2Configuraciones(salon.juego2Configuraciones);
        this.juego3Configuraciones = new Juego3Configuraciones(salon.juego3Configuraciones);
    }
}


public class GestionDeSalones : MonoBehaviour
{
    [SerializeField] private GameObject tablaDeSalones;
    [SerializeField] private GameObject filaTablaSalonesPrefab;
    [SerializeField] private GameObject botonAgregarSalon;

    [SerializeField] private GameObject panelAgregarSalon;
    [SerializeField] private TMP_InputField nombreSalonInput;
    [SerializeField] private TMP_InputField codigoSalonInput;
    [SerializeField] private Button botonGuardarSalon;

    [SerializeField] private GameManager gameManager;
    private string codigoSalon;
    private string profesorId;

    // Start is called before the first frame update
    void Start()
    {
        var usuarioActual = gameManager.GetUsuarioActual();
        Debug.Log(usuarioActual.id);
        profesorId = usuarioActual.id;
        MostrarSalonesEnTabla();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AgregarSalon()
    {
        codigoSalon = RandomString();
        codigoSalonInput.text = codigoSalon;
        panelAgregarSalon.SetActive(true);
    }

    public void GuardarSalon()
    {
        Salon salon = new Salon(nombreSalonInput.text, codigoSalon, profesorId);
        SaveSystem.SaveSalon(salon);
        panelAgregarSalon.SetActive(false);
        MostrarSalonesEnTabla();
    }

    public static string RandomString() // Generar un código aleatorio para el salón
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[4];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        string finalString = new String(stringChars);
        return finalString;
    }

    private void CrearFilaTablaSalones(Salon salon, int id)
    {
        GameObject fila = Instantiate(filaTablaSalonesPrefab, tablaDeSalones.transform);
        fila.transform.GetChild(0).GetComponent<TMP_Text>().text = id.ToString();
        fila.transform.GetChild(1).GetComponent<TMP_Text>().text = salon.nombreSalon;
        fila.transform.GetChild(2).GetComponent<TMP_Text>().text = salon.codigoSalon;
    }

    public void MostrarSalonesEnTabla()
    {
        List<Salon> salones = SaveSystem.LoadSalones(profesorId);

        // Limpiar la tabla antes de agregar nuevas filas
        foreach (Transform child in tablaDeSalones.transform)
        {
            Destroy(child.gameObject);
        }

        int id= 1;
        // Iterar sobre la lista de salones y crear una fila para cada uno
        foreach (Salon salon in salones)
        {
            CrearFilaTablaSalones(salon, id);
            id++;
        }
    }

    public Salon GetSalonByCodigo(string codigoSalon)
    {
        List<Salon> salones = SaveSystem.LoadSalones(profesorId);
        Salon salon = salones.FirstOrDefault(salon => salon.codigoSalon == codigoSalon);
        return salon;
    }
}
