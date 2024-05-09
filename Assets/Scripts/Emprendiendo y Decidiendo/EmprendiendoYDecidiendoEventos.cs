using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private List<Evento> eventoList;
    private Evento eventoActual;

    void Start()
    {
        eventoList = new List<Evento>();
        eventoList.Add(new Evento("Feria de emprendedores", "Participa en la feria de emprendedores. Aumenta tu popularidad en un 10%", 1000));
        eventoList.Add(new Evento("Incendio", "Ocurrió un incendio en tu local", 500));
        eventoList.Add(new Evento("Robo", "Robaron en tu local", 500));
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
    }

    public void AbrirPanelEventos()
    {
        gameObject.SetActive(true);
    }

    public void CerrarPanelEventos()
    {
        gameObject.SetActive(false);
    }


}
