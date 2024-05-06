using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointManager : MonoBehaviour
{
    private int points = 0;
    private int puntosParejaCorrecta = 10;
    [SerializeField] private TMP_Text text;
    public int pairCounter = 0;
    private int pairCounterMax;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text puntajeFinal;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Puntos: " + points.ToString();
        puntajeFinal.text = "Puntaje final: " + points.ToString();
        pairCounterMax = GameObject.FindGameObjectsWithTag(tag: "Demandante").Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPoint()
    {
        points += puntosParejaCorrecta;
        text.text = "Puntos: " + points.ToString();
        puntajeFinal.text = "Puntaje final: " + points.ToString();
    }

    public void addPairCounter()
    {
        pairCounter++;
        if (pairCounter == pairCounterMax)
        {
            winPanel.SetActive(true);
        }
    }
}
