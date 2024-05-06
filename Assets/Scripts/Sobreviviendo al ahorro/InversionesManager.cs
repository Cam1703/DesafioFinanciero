using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InversionesManager : MonoBehaviour
{
    class Inversion
    {
        public string nombre;
        public float probabilidadGanancia;
        public float multiplicacionPorGanancia;
        public bool activa; // Nuevo atributo para indicar si la inversión está activa
        public Inversion(string nombre, float probabilidadGanancia, float multiplicacionPorGanancia)
        {
            this.nombre = nombre;
            this.probabilidadGanancia = probabilidadGanancia;
            this.multiplicacionPorGanancia = multiplicacionPorGanancia;
            this.activa = false; // Al iniciar, la inversión está desactivada

        }
    }

    private List<Inversion> inversiones = new List<Inversion>();
    [SerializeField] private PresupuestoManager presupuestoManager;
    [SerializeField] private SobreviviendoAlAhorroGameManager gameManager;
    [SerializeField] private Sprite depositarBG;
    [SerializeField] private Sprite removerBG;
    [SerializeField] private GameObject panelDepositoInicial;
    [SerializeField] private TMP_InputField inputFieldDepositoInicial;
    [SerializeField] private Button cerrarPanelDeposito;
    private Button botonActivo;
    private Inversion inversionActiva;

    // Start is called before the first frame update
    void Start()
    {
        crearListaInversiones();
        gameManager = FindObjectOfType<SobreviviendoAlAhorroGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void crearListaInversiones()
    {
        if (inversiones.Count == 0)
        {
            inversiones.Add(new Inversion("Bolsa de Valores", 0.5f, 2f));
            inversiones.Add(new Inversion("Emprendimiento", 0.7f, 1.5f));
            inversiones.Add(new Inversion("Fondo Mutuo", 1f, 1.2f));
        }
    }


    private void UpdateButtonAppearance(Button button)
    {
        botonActivo = button;
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        Image buttonImage = button.GetComponent<Image>();
        if (buttonText.text == "Invertir")
        {
            buttonImage.sprite = removerBG;
            buttonText.text = "Retirar";
        }
        else
        {
            buttonText.text = "Invertir";
            buttonImage.sprite = depositarBG;
        }
    }

    public void UpdatePanelDepositoInicial(bool isActive)
    {
        panelDepositoInicial.SetActive(isActive);
    }

    public void GetInputTextOnClick()
    {
        // Intenta convertir el texto del campo de entrada a un número flotante
        if (float.TryParse(inputFieldDepositoInicial.text, out float depositoInicial))
        {
            // La conversión fue exitosa, ahora puedes usar el valor de depositoInicial
            Debug.Log(depositoInicial);
            depositoInicial = -depositoInicial;
            presupuestoManager.ActualizarGananciaInversiones(depositoInicial);
            UpdatePanelDepositoInicial(false);
            UpdateButtonAppearance(botonActivo);
            ActivarDesactivarInversion();
        }
        else
        {
            // Manejar el caso en que la conversión no fue exitosa
            Debug.LogWarning("Error: No se pudo convertir el valor ingresado a un número flotante.");
        }
    }

    public void setButtonActivo(Button button)
    {
        botonActivo = button;
        if(botonActivo.GetComponentInChildren<TMP_Text>().text == "Invertir")
        {
            UpdatePanelDepositoInicial(true);
            cerrarPanelDeposito.onClick.AddListener(() => UpdatePanelDepositoInicial(false));

        }
        else
        {
            presupuestoManager.ActualizarGananciaInversiones(presupuestoManager.Inversiones - presupuestoManager.Inversiones);
            UpdateButtonAppearance(botonActivo);
        }

    }

    public float CalcularGananciaInversiones()
    {
        float gananciaTotal = 0;
        foreach (Inversion inversion in GetInversionesActivas())
        {
            gananciaTotal += CalcularGananciaOPerdidaEnInversion(inversion.nombre);
        }
        Debug.Log("Ganancia total: " + gananciaTotal);
        return gananciaTotal;
    }

    private List<Inversion> GetInversionesActivas()
    {
        List<Inversion> inversionesActivas = new List<Inversion>();
        foreach (Inversion inversion in inversiones)
        {
            if (inversion.activa)
            {
                inversionesActivas.Add(inversion);
            }
        }
        Debug.Log("Inversiones activas: " + inversionesActivas.Count);
        return inversionesActivas;
    }


    private float CalcularGananciaOPerdidaEnInversion(string nombre)
    {
        float ganancia = 0;
        foreach (Inversion inversion in inversiones)
        {
            if (inversion.nombre == nombre)
            {
                if (inversion.activa)
                {
                    if (Random.value <= inversion.probabilidadGanancia)
                    {
                        ganancia = presupuestoManager.Inversiones * inversion.multiplicacionPorGanancia;
                        Debug.Log("Ganaste en la inversión de " + inversion.nombre);
                    }
                    else
                    {
                        Debug.Log("Perdiste en la inversión de " + inversion.nombre);
                        ganancia = -presupuestoManager.Inversiones;
                    }
                }
            }
        }
        return ganancia;
    }

    public void ActivarDesactivarInversion()
    {
        string nombre = inversionActiva.nombre;
        Debug.Log("Inversion activa: " + inversionActiva.nombre);
        foreach (Inversion inversion in inversiones)
        {
            if (inversion.nombre == nombre)
            {
                inversion.activa = !inversion.activa;
            }
        }

        inputFieldDepositoInicial.text = "";
    }

    public void ObtenerInversionClickeada(string nombre)
    {
        inversionActiva = inversiones.Find(inversion => inversion.nombre == nombre);
    }

}
