using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        Debug.Log("Cambiando a escena: " + nombreEscena);
        SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }

}
