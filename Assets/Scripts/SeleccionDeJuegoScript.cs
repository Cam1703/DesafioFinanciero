using System.Collections;
using UnityEngine;

public class SeleccionDeJuegoScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject panelDeSecciones;
    [SerializeField] private GameObject seccion;
    private Vector3 posicionInicialPanelDeSecciones;

    public void Start()
    {
        posicionInicialPanelDeSecciones = panelDeSecciones.transform.localPosition;
    }

    // Mover la vista de la sección hacia la derecha de forma suave
    public void MoverSectionViewDerecha(int indiceNuevaSeccion)
    {
        float seccionWidth = seccion.GetComponent<RectTransform>().rect.width;
        Vector3 nuevaPosicion = panelDeSecciones.transform.localPosition + new Vector3(-seccionWidth, 0, 0);
        StartCoroutine(MoverDeFormaSuave(nuevaPosicion));

        if (indiceNuevaSeccion == 1)
        {
            // Volver a la posición inicial
            StartCoroutine(MoverDeFormaSuave(posicionInicialPanelDeSecciones));
        }
    }

    // Mover la vista de la sección hacia la izquierda de forma suave
    public void MoverSectionViewIzquierda(int indiceNuevaSeccion)
    {
        float seccionWidth = seccion.GetComponent<RectTransform>().rect.width;
        Vector3 nuevaPosicion = panelDeSecciones.transform.localPosition + new Vector3(seccionWidth, 0, 0);
        StartCoroutine(MoverDeFormaSuave(nuevaPosicion));

        if (indiceNuevaSeccion == 4)
        {
            // Volver a la posición final
            Vector3 posicionFinal = -posicionInicialPanelDeSecciones;
            Debug.Log("Posicion final: " + posicionFinal);
            StartCoroutine(MoverDeFormaSuave(posicionFinal));
        }
    }

    // Coroutine para mover el panel de forma suave
    private IEnumerator MoverDeFormaSuave(Vector3 destino)
    {
        float duracion = 0.5f; // Duración de la animación en segundos
        Vector3 posicionInicial = panelDeSecciones.transform.localPosition;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            panelDeSecciones.transform.localPosition = Vector3.Lerp(posicionInicial, destino, tiempoTranscurrido / duracion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        panelDeSecciones.transform.localPosition = destino;
    }
}
