using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmprendiendoYDecidiendoTabConfiguration : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Button> tabs;
    [SerializeField] private GameManager gameManager;
    private bool habilitarCompraDeMejoras;
    private bool habilitarSeguros;
    private bool habilitarBanco;
    private Usuario usuarioActual;


    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        Juego5Configuraciones juego5Configuraciones = SaveSystem.GetConfiguracionesJuego5PorSalon(codSalon);
        habilitarBanco = juego5Configuraciones.habilitarBanco;
        habilitarCompraDeMejoras = juego5Configuraciones.habilitarCompraDeMejoras;
        habilitarSeguros = juego5Configuraciones.habilitarSeguros;

        foreach (Button tab in tabs)
        {
            if (tab.name == "Mejoras" && !habilitarCompraDeMejoras)
            {
                tab.gameObject.SetActive(false);
            }
            else if (tab.name == "Seguros" && !habilitarSeguros)
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
