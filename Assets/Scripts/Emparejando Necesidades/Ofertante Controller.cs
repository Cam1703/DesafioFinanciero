using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfertanteController : MonoBehaviour
{
    public bool isSelected = false;
    public bool isPaired = false;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarSeleccionado()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        isSelected = true;
    }

    public void MostrarNoSeleccionado()
    {
        GetComponent<Renderer>().material.color = Color.white;
        isSelected = false;
    }


}
