using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemandanteController : MonoBehaviour
{
    public bool isSelected = false;
    public int id;
    public bool isPaired = false;
    private OfertanteController ofertanteController;
    [SerializeField] private LineController lineController;
    [SerializeField] private PointManager pointManager;

    [SerializeField] private AudioSource soundEffectSource; // Asigna un AudioSource en el inspector
    [SerializeField] private AudioClip correctAnswerSound;
    [SerializeField] private AudioClip wrongAnswerSound;
    [SerializeField] private AudioClip selected;
    [SerializeField] private AudioClip unselected;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!isSelected)
                    {
                        // Si el objeto no ha sido seleccionado, lo seleccionamos
                        soundEffectSource.clip = selected;
                        soundEffectSource.Play();
                        MostrarSeleccionado();
                    }
                    else
                    {
                        // Si el objeto ya ha sido seleccionado, lo deseleccionamos
                        soundEffectSource.clip = unselected;
                        soundEffectSource.Play();
                        MostrarNoSeleccionado();
                    }
                }
                else
                {
                    // Si se ha clickeado otro objeto
                    ofertanteController = hit.collider.gameObject.GetComponent<OfertanteController>();
                    if (ofertanteController != null && isSelected && !isPaired)
                    {
                        // Si el demandante está seleccionado y no está emparejado y el objeto clickeado es un ofertante
                        GameObject ofertante = hit.collider.gameObject;
                        Debug.Log("Se ha emparejado el demandante " + id + " con el ofertante " + ofertanteController.id);
                        ofertanteController.MostrarSeleccionado();
                        isPaired = true;
                        ofertanteController.isPaired = true;
                        lineController.GetPuntoInicio();
                        lineController.GetPuntoFin(ofertante);
                        MostrarNoSeleccionado();
                        ofertanteController.MostrarNoSeleccionado();
                        if (ofertanteController.id == id)
                        {
                            soundEffectSource.clip = correctAnswerSound;
                            soundEffectSource.Play();
                            pointManager.addPoint();
                        }
                        else
                        {
                            soundEffectSource.clip = wrongAnswerSound;
                            soundEffectSource.Play();
                            pointManager.subtractPoint();
                        }
                        pointManager.addPairCounter();
                    }
                    else
                    {
                        // Si el demandante ya está emparejado, simplemente lo deseleccionamos
                        MostrarNoSeleccionado();
                    }
                }
            }
            else
            {
                // Si no se ha clickeado ningún objeto, simplemente deseleccionamos al demandante
                MostrarNoSeleccionado();
            }
        }
    }

    private void MostrarSeleccionado()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        isSelected = true;
    }

    private void MostrarNoSeleccionado()
    {
        GetComponent<Renderer>().material.color = Color.white;
        isSelected = false;
    }


}
