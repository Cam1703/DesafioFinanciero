using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabConfigurarion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Button> tabs;
    [SerializeField] private GameManager gameManager;
    private bool habilitarCompraDeBonificaciones;
    private bool habilitarInversiones;
    private bool habilitarBanco;
    private bool habilitarEventosAleatorios;
    private Usuario usuarioActual;


    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        Juego3Configuraciones juego3Configuraciones = SaveSystem.GetConfiguracionesJuego3PorSalon(codSalon);
        Debug.Log("habilitarCompraDeBonificaciones:" + juego3Configuraciones.habilitarCompraDeBonificaciones);
        habilitarBanco = juego3Configuraciones.habilitarBanco;
        habilitarCompraDeBonificaciones = juego3Configuraciones.habilitarCompraDeBonificaciones;
        habilitarInversiones = juego3Configuraciones.habilitarInversiones;
        habilitarEventosAleatorios = juego3Configuraciones.habilitarEventosAleatorios;

        foreach (Button tab in tabs)
        {
            if (tab.name == "Tienda" && !habilitarCompraDeBonificaciones)
            {
                tab.gameObject.SetActive(false);
            }
            else if (tab.name == "Inversiones" && !habilitarInversiones)
            {
                tab.gameObject.SetActive(false);
            }
            else if (tab.name == "Banco" && !habilitarBanco)
            {
                tab.gameObject.SetActive(false);
            }
        }
    }

    
}
