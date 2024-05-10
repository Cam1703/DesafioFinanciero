using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EmprendiendoYDecidiendoPanelMejoras : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text nombreDeMejora;
    [SerializeField] private TMP_Text precioDeMejora;
    [SerializeField] private TMP_InputField nroCuotasInputField;
    [SerializeField] private EmprendiendoYDecidiendoMejoras emprendiendoYDecidiendoMejoras;
    [SerializeField] private TMP_InputField totalInputField;
    [SerializeField] private EmprendiendoYDecidiendoPresupuesto presupuesto;
    [SerializeField] private EmprendiendoYDecidiendoInformacion informacion;
    [SerializeField] Button expandirLocalButton;
    [SerializeField] Button mejoresInsumosButton;
    [SerializeField] Button campañaPublicitariaButton;
    [SerializeField] Button mejoresSillasButton;

    private string nombre;
    private float precio;
    private bool isDirecto = true;
    private int nroCuotas;
    private float interes = 0.05f;
    private float total;
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
        this.nombre = nombre;
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

        total = precio + (precio * interes * nroCuotas);

        totalInputField.text = total.ToString();
    }

    public void ComprarMejora()
    {
        if (isDirecto)
        {
            presupuesto.ActualizarAhorros(-precio);
            presupuesto.ActualizarCantidadDeCuotasPorMejora(0f,nombre, 0);
            if (nombre == "Expandir Local")
            {
                informacion.SetHasLocalAlquiladoExpandido(true);
            }
            else if (nombre == "Mejores Insumos")
            {
                informacion.SetHasMejoresInsumos(true);
            }
            else if (nombre == "Campaña Publicitaria")
            {
                informacion.SetHasPublicidad(true);
            }
            else if (nombre == "Mejores Sillas")
            {
                informacion.SetHasMejoresSillas(true);
            }
        }
        else
        {
            presupuesto.ActualizarCantidadDeCuotasPorMejora(total/nroCuotas, nombre, nroCuotas );

        }

        DeshabilitarBoton();
    }

    private void DeshabilitarBoton()
    {
        if (nombre == "Expandir Local")
        {
            expandirLocalButton.interactable = false;
        }
        else if (nombre == "Mejores Insumos")
        {
            mejoresInsumosButton.interactable = false;
        }
        else if (nombre == "Campaña Publicitaria")
        {
            campañaPublicitariaButton.interactable = false;
        }
        else if (nombre == "Mejores Sillas")
        {
            mejoresSillasButton.interactable = false;
        }
    }
}
