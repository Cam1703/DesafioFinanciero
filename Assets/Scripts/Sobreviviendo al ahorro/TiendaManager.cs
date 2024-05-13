 using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TiendaManager : MonoBehaviour
{
    // Start is called before the first frame update

    class Bonificacion
    {
        public string nombre;
        public float precio;
        public Bonificacion(string nombre, float precio)
        {
            this.nombre = nombre;
            this.precio = precio;
        }
    }

    private List<Bonificacion> bonificaciones = new List<Bonificacion>();
    [SerializeField] private PresupuestoManager presupuestoManager;
    [SerializeField] private SobreviviendoAlAhorroLevelManager levelManager;
    [SerializeField] private Sprite agregarAPresupuestoBG;
    [SerializeField] private Sprite removerAPresupuestoBG;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SobreviviendoAlAhorroGameManager gameManager;
    void Start()
    {
        crearListaBonificaciones();
        levelManager = FindObjectOfType<SobreviviendoAlAhorroLevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void crearListaBonificaciones()
    {
        if (bonificaciones.Count == 0)
        {
            bonificaciones.Add(new Bonificacion("Taxi", 100));
            bonificaciones.Add(new Bonificacion("Comida Saludable", 100));
            bonificaciones.Add(new Bonificacion("Seguro de vida", 100));
        }
    }

    public void agregarTaxiAPresupuesto(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText.text == "Agregar a Presupuesto")
        {
            presupuestoManager.ActualizarPrecioTransporte(bonificaciones[0].precio);
            levelManager.ActivarDesactivarEnemigoTransporte(true);
            UpdateButtonAppearance(button);
            playerController.speed = 7;
        }
        else
        {
            playerController.speed = 5;
            presupuestoManager.ActualizarPrecioTransporte(0);
            levelManager.ActivarDesactivarEnemigoTransporte(false);
            UpdateButtonAppearance(button);
        }

    }

    public void agregarComidaSaludableAPresupuesto(Button button)
    {

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText.text == "Agregar a Presupuesto")
        {
            gameManager.totalVidas = 5;
            presupuestoManager.ActualizarPrecioComida(bonificaciones[1].precio + presupuestoManager.Comida);
            levelManager.ActualizarPrecioComida((int)bonificaciones[1].precio + (int)presupuestoManager.Comida);
            UpdateButtonAppearance(button);
        }
        else
        {
            gameManager.totalVidas = 3;
            presupuestoManager.ActualizarPrecioComida(presupuestoManager.Comida - bonificaciones[1].precio);
            levelManager.ActualizarPrecioComida((int)bonificaciones[1].precio + (int)presupuestoManager.Comida);
            UpdateButtonAppearance(button);
        }

    }

    public void agregarSeguroDeVidaAPresupuesto(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

        if (buttonText.text == "Agregar a Presupuesto")
        {
            gameManager.hasSeguro = true;
            presupuestoManager.ActualizarPrecioSeguro(bonificaciones[2].precio);
            levelManager.ActivarDesactivarEnemigoSeguro(true);
            UpdateButtonAppearance(button);
        }
        else
        {
            gameManager.hasSeguro = false;
            presupuestoManager.ActualizarPrecioSeguro(0);
            levelManager.ActivarDesactivarEnemigoSeguro(false);
            UpdateButtonAppearance(button);
        }

    }



    private void UpdateButtonAppearance(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        // Cambia la imagen de fondo del botón y el texto según el estado actual
        if (buttonText.text != "Agregar a Presupuesto")
        {
            buttonText.text = "Agregar a Presupuesto"; // Cambia el texto del botón a "Agregar Presupuesto"

            button.GetComponent<Image>().sprite = agregarAPresupuestoBG; // Cambia la imagen de fondo del botón a la imagen de agregar presupuesto

        }
        else
        {
            buttonText.text = "Quitar de Presupuesto"; // Cambia el texto del botón a "Quitar de Presupuesto"

            button.GetComponent<Image>().sprite = removerAPresupuestoBG; // Cambia la imagen de fondo del botón a la imagen de quitar de presupuesto

        }

    }
}
