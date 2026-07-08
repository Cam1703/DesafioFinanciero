using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public Juego4Configuraciones juego4Configuraciones;
    public Juego5Configuraciones juego5Configuraciones;

    public Salon(string nombreSalon, string codigoSalon, string profesorId)
    {
        this.profesorId = profesorId;
        this.nombreSalon = nombreSalon;
        this.codigoSalon = codigoSalon;
        this.juego1Configuraciones = new Juego1Configuraciones(true, true, 100, 50, 400, true, 6);
        this.juego2Configuraciones = new Juego2Configuraciones(true, 100, 50, 300, true, 4);
        this.juego3Configuraciones = new Juego3Configuraciones(true, true, true, true, 5, 400);
        this.juego4Configuraciones = new Juego4Configuraciones(true, true, 100, 50, 400, true, 6);
        this.juego5Configuraciones = new Juego5Configuraciones(true, true, true, 5, 400);
    }

    public Salon (Salon salon)
    {
        this.profesorId = salon.profesorId;
        this.nombreSalon = salon.nombreSalon;
        this.codigoSalon = salon.codigoSalon;
        this.juego1Configuraciones = new Juego1Configuraciones(salon.juego1Configuraciones);
        this.juego2Configuraciones = new Juego2Configuraciones(salon.juego2Configuraciones);
        this.juego3Configuraciones = new Juego3Configuraciones(salon.juego3Configuraciones);
        this.juego4Configuraciones = new Juego4Configuraciones(salon.juego4Configuraciones);
        this.juego5Configuraciones = new Juego5Configuraciones(salon.juego5Configuraciones);
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
    async void Start()
    {
        var usuarioActual = gameManager.GetUsuarioActual();
        Debug.Log(usuarioActual.id);
        profesorId = usuarioActual.id;
        await MostrarSalonesEnTabla();
    }

    public async void AgregarSalon()
    {
        if (botonAgregarSalon != null) botonAgregarSalon.GetComponent<Button>().enabled = false;
        try
        {
            codigoSalon = await GenerarCodigoUnicoAsync();
            codigoSalonInput.text = codigoSalon;
            panelAgregarSalon.SetActive(true);
        }
        finally
        {
            if (botonAgregarSalon != null) botonAgregarSalon.GetComponent<Button>().enabled = true;
        }
    }

    public async void GuardarSalon()
    {
        if (botonGuardarSalon != null) botonGuardarSalon.enabled = false;
        try
        {
            Salon salon = new Salon(nombreSalonInput.text, codigoSalon, profesorId);
            await SaveSystem.SaveSalonAsync(salon);
            panelAgregarSalon.SetActive(false);
            await MostrarSalonesEnTabla();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar salón: " + e);
        }
        finally
        {
            if (botonGuardarSalon != null) botonGuardarSalon.enabled = true;
        }
    }

    /// <summary>
    /// Antes el código se generaba sin verificar colisión contra salones ya
    /// existentes; ahora que salones.json era local por instalación el riesgo era
    /// bajo, pero al pasar a una sola base de datos compartida en Firebase sí
    /// importa evitar que dos salones terminen con el mismo código.
    /// </summary>
    private static async Task<string> GenerarCodigoUnicoAsync()
    {
        const int maxIntentos = 10;
        for (int i = 0; i < maxIntentos; i++)
        {
            string candidato = RandomString();
            if (!await SaveSystem.ExisteSalonAsync(candidato))
            {
                return candidato;
            }
        }
        // Extremadamente improbable con 62^4 combinaciones, pero se evita un candidato duplicado silencioso.
        Debug.LogWarning("No se encontró un código de salón único tras varios intentos; se usará el último generado.");
        return RandomString();
    }

    public static string RandomString() // Generar un c�digo aleatorio para el sal�n
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

    public async Task MostrarSalonesEnTabla()
    {
        List<Salon> salones = await SaveSystem.LoadSalonesAsync(profesorId);

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

    public async Task<Salon> GetSalonByCodigoAsync(string codigoSalon)
    {
        return await SaveSystem.GetSalonByCodigoAsync(codigoSalon);
    }
}
