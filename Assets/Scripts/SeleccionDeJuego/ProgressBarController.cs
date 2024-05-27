using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] GameObject fillProgress;
    [SerializeField] GameManager gameManager;
    [SerializeField] int juego;
    [SerializeField] TMP_Text puntaje;
    private Usuario usuarioActual;
    private Salon salonActual;
    private float maxWidth = 318.63f;

    void Start()
    {
        usuarioActual = gameManager.GetUsuarioActual();
        salonActual = SaveSystem.GetSalonByCodigo(usuarioActual.codigoDeClase);

        Debug.Log(salonActual.codigoSalon);
        PuntajeMaximoActualEnJuegos dataPuntajeAlumno = usuarioActual.puntajesMaximos;

        float puntajeMaximo = 0;
        float puntajeActual = 0;

        switch (juego)
        {
            case 1:
                puntajeMaximo = salonActual.juego1Configuraciones.puntajeAprobatorio;
                puntajeActual = dataPuntajeAlumno.puntajeMaximoJuego1;
                break;
            case 2:
                puntajeMaximo = salonActual.juego2Configuraciones.puntajeAprobatorio;
                puntajeActual = dataPuntajeAlumno.puntajeMaximoJuego2;
                break;
            case 3:
                puntajeMaximo = salonActual.juego3Configuraciones.puntajeAprobatorio;
                puntajeActual = dataPuntajeAlumno.puntajeMaximoJuego3;
                break;
            case 4:
                puntajeMaximo = salonActual.juego4Configuraciones.puntajeAprobatorio;
                puntajeActual = dataPuntajeAlumno.puntajeMaximoJuego4;
                break;
            case 5:
                puntajeMaximo = salonActual.juego5Configuraciones.puntajeAprobatorio;
                puntajeActual = dataPuntajeAlumno.puntajeMaximoJuego5;
                break;
            default:
                Debug.LogWarning("Invalid game number");
                return;
        }

        UpdateProgressBar(puntajeActual, puntajeMaximo);
    }

    private void UpdateProgressBar(float puntajeActual, float puntajeMaximo)
    {
        float progress = CalculateProgress(puntajeActual, puntajeMaximo);
        fillProgress.GetComponent<RectTransform>().sizeDelta = new Vector2(progress, fillProgress.GetComponent<RectTransform>().sizeDelta.y);
        puntaje.text = $"{puntajeActual}/{puntajeMaximo} puntos";
        puntaje.fontStyle = FontStyles.Bold;
    }

    private float CalculateProgress(float puntajeActual, float puntajeMaximo)
    {
        if (puntajeMaximo == 0)
        {
            Debug.LogWarning("Puntaje máximo es 0, no se puede calcular el progreso.");
            return 0;
        }

        float progressRatio = puntajeActual / puntajeMaximo;
        float progressWidth = progressRatio * maxWidth;
        return Mathf.Clamp(progressWidth, 0, maxWidth);
    }
}
