using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BancoTabsManager : MonoBehaviour
{
    public GameObject[] tabs; // Array que contiene los paneles de las pestañas
    public Sprite[] selectedButtonSprite; // Imagen de fondo para el botón seleccionado
    public Sprite[] unselectedButtonSprite; // Imagen de fondo para los botones no seleccionados

    public Button[] tabButtons; // Array de botones asociados a cada pestaña

    private int currentTabIndex = 0; // Índice de la pestaña actualmente seleccionada

    private void Start()
    {
        // Al iniciar, muestra solo la primera pestaña y establece su botón como seleccionado
        ShowTab(0);
    }

    // Método para mostrar una pestaña específica y ocultar las demás
    public void ShowTab(int tabIndex)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == tabIndex)
            {
                tabs[i].SetActive(true); // Muestra la pestaña seleccionada
                tabButtons[i].GetComponent<Image>().sprite = selectedButtonSprite[i]; // Cambia la imagen de fondo del botón a seleccionado
            }
            else
            {
                tabs[i].SetActive(false); // Oculta las demás pestañas
                tabButtons[i].GetComponent<Image>().sprite = unselectedButtonSprite[i]; // Cambia la imagen de fondo del botón a no seleccionado
            }
        }
        currentTabIndex = tabIndex; // Actualiza el índice de la pestaña actual
    }

    // Método para manejar el evento de cambio de pestaña al hacer clic en un botón
    public void ChangeTab(int newTabIndex)
    {
        // Oculta la pestaña actual y muestra la nueva pestaña
        ShowTab(newTabIndex);
    }
}
