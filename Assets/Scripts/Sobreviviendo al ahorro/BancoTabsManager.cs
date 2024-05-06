using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BancoTabsManager : MonoBehaviour
{
    public GameObject[] tabs; // Array que contiene los paneles de las pesta�as
    public Sprite[] selectedButtonSprite; // Imagen de fondo para el bot�n seleccionado
    public Sprite[] unselectedButtonSprite; // Imagen de fondo para los botones no seleccionados

    public Button[] tabButtons; // Array de botones asociados a cada pesta�a

    private int currentTabIndex = 0; // �ndice de la pesta�a actualmente seleccionada

    private void Start()
    {
        // Al iniciar, muestra solo la primera pesta�a y establece su bot�n como seleccionado
        ShowTab(0);
    }

    // M�todo para mostrar una pesta�a espec�fica y ocultar las dem�s
    public void ShowTab(int tabIndex)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == tabIndex)
            {
                tabs[i].SetActive(true); // Muestra la pesta�a seleccionada
                tabButtons[i].GetComponent<Image>().sprite = selectedButtonSprite[i]; // Cambia la imagen de fondo del bot�n a seleccionado
            }
            else
            {
                tabs[i].SetActive(false); // Oculta las dem�s pesta�as
                tabButtons[i].GetComponent<Image>().sprite = unselectedButtonSprite[i]; // Cambia la imagen de fondo del bot�n a no seleccionado
            }
        }
        currentTabIndex = tabIndex; // Actualiza el �ndice de la pesta�a actual
    }

    // M�todo para manejar el evento de cambio de pesta�a al hacer clic en un bot�n
    public void ChangeTab(int newTabIndex)
    {
        // Oculta la pesta�a actual y muestra la nueva pesta�a
        ShowTab(newTabIndex);
    }
}
