using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Juego5Configuraciones
{
    public bool habilitarCompraDeMejoras;
    public bool habilitarSeguros;
    public bool habilitarBanco;

    public int cantidadDeNiveles;
    public int puntajeAprobatorio;

    public Juego5Configuraciones(bool habilitarCompraDeMejoras, bool habilitarSeguros, bool habilitarBanco, int cantidadDeNiveles, int puntajeAprobatorio)
    {
        this.habilitarCompraDeMejoras = habilitarCompraDeMejoras;
        this.habilitarSeguros = habilitarSeguros;
        this.habilitarBanco = habilitarBanco;
        this.cantidadDeNiveles = cantidadDeNiveles;
        this.puntajeAprobatorio = puntajeAprobatorio;
    }

    public Juego5Configuraciones(Juego5Configuraciones configuraciones)
    {
        this.habilitarCompraDeMejoras = configuraciones.habilitarCompraDeMejoras;
        this.habilitarSeguros = configuraciones.habilitarSeguros;
        this.habilitarBanco = configuraciones.habilitarBanco;
        this.cantidadDeNiveles = configuraciones.cantidadDeNiveles;
        this.puntajeAprobatorio = configuraciones.puntajeAprobatorio;
    }

}
public class ConfiguracionesJuego5 : MonoBehaviour
{
    // Temáticas de juego
    private bool habilitarCompraDeMejoras = true;
    private bool habilitarSeguros = true;
    private bool habilitarBanco = true;

    // Características de juego
    private int cantidadDeNiveles = 5;
    private int puntajeAprobatorio = 400;

    // Temáticas de juego UI
    [SerializeField] private Toggle toggleCompraDeMejoras;
    [SerializeField] private Toggle toggleSeguros;
    [SerializeField] private Toggle toggleBanco;

    // Características de juego UI
    [SerializeField] private TMP_InputField inputFieldCantidadDeNiveles;
    [SerializeField] private TMP_InputField inputFieldPuntajeAprobatorio;

    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Juego5Configuraciones configuraciones = gameManager.GetSalonActual().juego5Configuraciones;
        toggleBanco.isOn = configuraciones.habilitarBanco;
        toggleCompraDeMejoras.isOn = configuraciones.habilitarCompraDeMejoras;
        toggleSeguros.isOn = configuraciones.habilitarSeguros;
        inputFieldCantidadDeNiveles.text = configuraciones.cantidadDeNiveles.ToString();
        inputFieldPuntajeAprobatorio.text = configuraciones.puntajeAprobatorio.ToString();
    }

    public void GuardarConfiguracion()
    {
        habilitarBanco = toggleBanco.isOn;
        habilitarCompraDeMejoras = toggleCompraDeMejoras.isOn;
        habilitarSeguros = toggleSeguros.isOn;
        cantidadDeNiveles = int.Parse(inputFieldCantidadDeNiveles.text);
        puntajeAprobatorio = int.Parse(inputFieldPuntajeAprobatorio.text);

        Juego5Configuraciones configuraciones = new Juego5Configuraciones(habilitarCompraDeMejoras, habilitarSeguros, habilitarBanco, cantidadDeNiveles, puntajeAprobatorio);
        Salon salon = gameManager.GetSalonActual();
        salon.juego5Configuraciones = configuraciones;
        SaveSystem.UpdateSalon(salon);
    }
}
