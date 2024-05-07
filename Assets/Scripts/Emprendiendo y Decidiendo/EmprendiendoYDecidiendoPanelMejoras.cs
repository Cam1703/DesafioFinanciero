using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EmprendiendoYDecidiendoPanelMejoras : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text nombreDeMejora;
    [SerializeField] private TMP_Text precioDeMejora;
    [SerializeField] private TMP_InputField nroCuotasInputField;
    [SerializeField] private EmprendiendoYDecidiendoMejoras emprendiendoYDecidiendoMejoras;
    [SerializeField] private TMP_InputField totalInputField;
    [SerializeField] private EmprendiendoYDecidiendoPresupuesto presupuesto;

    private float precio;
    private bool isDirecto;
    private int nroCuotas;
    private float interes = 0.05f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNombreYPrecioDeMejora(string nombre, float precio)
    {
        nombreDeMejora.text = nombre;
        precioDeMejora.text = precio.ToString();
        this.precio = precio;
    }

    public void ChangeIsDirecto(bool isDirecto)
    {
        this.isDirecto = isDirecto;
    }

    public void ChangeNroCuotas()
    {
        int.TryParse(nroCuotasInputField.text, out  nroCuotas);
        MostrarTotal();
    }

    private void MostrarTotal()
    {
        float total;

        total = precio + (precio * interes * nroCuotas);

        totalInputField.text = total.ToString();
    }

    public void ComprarMejora()
    {
        if (isDirecto)
        {
            presupuesto.ActualizarAhorros(-precio);
        }
        else
        {
            // manejar el caso de que la compra sea en cuotas

        }
    }
}
