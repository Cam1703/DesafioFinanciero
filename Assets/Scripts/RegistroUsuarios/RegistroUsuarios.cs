using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroUsuarios : MonoBehaviour
{
    private bool isProfesor;

    [SerializeField] private GameObject panelRegistro;
    [SerializeField] private GameObject panelSeleccionarTipoRegistro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsProfesor(bool tipo)
    {
        isProfesor = tipo;
        panelRegistro.SetActive(true);
        panelSeleccionarTipoRegistro.SetActive(false);
    }
}
