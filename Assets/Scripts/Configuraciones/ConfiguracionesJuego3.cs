using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Juego3Configuraciones
{
    public bool habilitarCompraDeBonificaciones;
    public bool habilitarInversiones;
    public bool habilitarEventosAleatorios;
    public bool habilitarBanco;

    public int cantidadDeNiveles;
    public int puntajeAprobatorio;

    public Juego3Configuraciones(bool habilitarCompraDeBonificaciones, bool habilitarInversiones, bool habilitarEventosAleatorios, bool habilitarBanco, int cantidadDeNiveles, int puntajeAprobatorio)
    {
        this.habilitarCompraDeBonificaciones = habilitarCompraDeBonificaciones;
        this.habilitarInversiones = habilitarInversiones;
        this.habilitarEventosAleatorios = habilitarEventosAleatorios;
        this.habilitarBanco = habilitarBanco;
        this.cantidadDeNiveles = cantidadDeNiveles;
        this.puntajeAprobatorio = puntajeAprobatorio;
    }

    public Juego3Configuraciones(Juego3Configuraciones configuraciones)
    {
        this.habilitarCompraDeBonificaciones = configuraciones.habilitarCompraDeBonificaciones;
        this.habilitarInversiones = configuraciones.habilitarInversiones;
        this.habilitarEventosAleatorios = configuraciones.habilitarEventosAleatorios;
        this.habilitarBanco = configuraciones.habilitarBanco;
        this.cantidadDeNiveles = configuraciones.cantidadDeNiveles;
        this.puntajeAprobatorio = configuraciones.puntajeAprobatorio;
    }

}

public class ConfiguracionesJuego3 : MonoBehaviour
{

    //Tematicas de juego
    private bool habilitarCompraDeBonificaciones = true;
    private bool habilitarInversiones = true;
    private bool habilitarEventosAleatorios = true;
    private bool habilitarBanco = true;
    
    //Caracteristicas de juego
    private int cantidadDeNiveles = 5;
    private int puntajeAprobatorio = 400;


    //Tematicas de juego UI
    [SerializeField] private Toggle toggleCompraDeBonificaciones;
    [SerializeField] private Toggle toggleInversiones;
    [SerializeField] private Toggle toggleEventosAleatorios;
    [SerializeField] private Toggle toggleBanco;

    //Caracteristicas de juego UI
    [SerializeField] private TMP_InputField inputFieldCantidadDeNiveles;
    [SerializeField] private TMP_InputField inputFieldPuntajeAprobatorio;

    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Juego3Configuraciones configuraciones = gameManager.GetSalonActual().juego3Configuraciones;
        toggleBanco.isOn = configuraciones.habilitarBanco;
        toggleCompraDeBonificaciones.isOn = configuraciones.habilitarCompraDeBonificaciones;
        toggleEventosAleatorios.isOn = configuraciones.habilitarEventosAleatorios;
        toggleInversiones.isOn = configuraciones.habilitarInversiones;
        inputFieldCantidadDeNiveles.text = configuraciones.cantidadDeNiveles.ToString();
        inputFieldPuntajeAprobatorio.text = configuraciones.puntajeAprobatorio.ToString();

    }

    public void GuardarConfiguracion()
    {
        habilitarBanco = toggleBanco.isOn;
        habilitarCompraDeBonificaciones = toggleCompraDeBonificaciones.isOn;
        habilitarEventosAleatorios = toggleEventosAleatorios.isOn;
        habilitarInversiones = toggleInversiones.isOn;
        cantidadDeNiveles = int.Parse(inputFieldCantidadDeNiveles.text);
        puntajeAprobatorio = int.Parse(inputFieldPuntajeAprobatorio.text);

        Juego3Configuraciones configuraciones = new Juego3Configuraciones(habilitarCompraDeBonificaciones, habilitarInversiones, habilitarEventosAleatorios, habilitarBanco, cantidadDeNiveles, puntajeAprobatorio);
        Salon salon = gameManager.GetSalonActual();
        salon.juego3Configuraciones = configuraciones;
        SaveSystem.UpdateSalon(salon);
    }
}
