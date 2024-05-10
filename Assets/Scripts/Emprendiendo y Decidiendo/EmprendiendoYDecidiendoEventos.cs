using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Evento
{
    public string nombre;
    public string descripcion;
    public float precio;

    public Evento(string nombre, string descripcion, float precio)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precio = precio;
    }
}

public class EmprendiendoYDecidiendoEventos : MonoBehaviour
{

    [SerializeField] TMP_Text textoEvento;
    private List<Evento> eventoList = new List<Evento>
        {
            new Evento("Feria de emprendedores", "Participa en la feria de emprendedores. Aumenta tu popularidad en un 10%, cuesta 1000 soles", 1000),
            new Evento("Incendio", "Ocurrió un incendio en tu local, la perdida de materiales tendrá un costo de 500 soles. ", 500),
            new Evento("Robo", "Robaron en tu local. la pérdida en materiales tendrá un costo de 500 soles en total", 500)
        };
    private Evento eventoActual;
    [SerializeField] Button buttonDeclinar;
    [SerializeField] Button buttonPagar;
    [SerializeField] Button buttonUsarSeguro;
    [SerializeField] GameObject panelPagar;
    [SerializeField] EmprendiendoYDecidiendoPanelMejoras emprendiendoYDecidiendoPanelMejoras;
    [SerializeField] EmprendiendoYDecidiendoSeguros emprendiendoYDecidiendoSeguros;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerarEvento()
    {
        int random = Random.Range(0, eventoList.Count);
        textoEvento.text = eventoList[random].descripcion;
        eventoActual = eventoList[random];
        if (eventoActual.nombre == "Feria de emprendedores")
        {
            buttonDeclinar.gameObject.SetActive(true);
            buttonPagar.gameObject.SetActive(true);
            buttonUsarSeguro.gameObject.SetActive(false);
        }
        else
        {
            buttonDeclinar.gameObject.SetActive(false);
            buttonPagar.gameObject.SetActive(true);
            if (emprendiendoYDecidiendoSeguros.HasSeguroContraIncendios && eventoActual.nombre == "Incendio")
            {
                buttonUsarSeguro.gameObject.SetActive(true);
            }
            else if (emprendiendoYDecidiendoSeguros.HasSeguroContraRobo && eventoActual.nombre == "Robo")
            {
                buttonUsarSeguro.gameObject.SetActive(true);
            }
            else
            {
                buttonUsarSeguro.gameObject.SetActive(false);
            }
        }
    }

    public void AbrirPanelEventos()
    {
        gameObject.SetActive(true);
    }

    public void CerrarPanelEventos()
    {
        gameObject.SetActive(false);
    }

    public void PagarEvento()
    {
        gameObject.SetActive(false);

        emprendiendoYDecidiendoPanelMejoras.SetNombreYPrecioDeMejora("Pago de Eventos", eventoActual.precio);
        panelPagar.SetActive(true);

    }

}
