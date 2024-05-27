using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmprendiendoYDecidiendoBanco : MonoBehaviour
{
    private float montoPrestamo;
    private float tasaInteres = 0.05f;
    private int nroCuotas;
    private float montoCuotas;
    private float montoTotalPrestamo;

    [SerializeField] private TMP_InputField montoPrestamoInputField;
    [SerializeField] private TMP_InputField tasaInteresInputField;
    [SerializeField] private TMP_InputField nroCuotasInputField;
    [SerializeField] private TMP_InputField montoCuotaText;
    [SerializeField] private TMP_InputField pagoTotalDelPrestamo;

    [SerializeField] private EmprendiendoYDecidiendoPresupuesto presupuesto;
    [SerializeField] private Button buttonSolicitar;

    // Start is called before the first frame update
    void Start()
    {
        tasaInteresInputField.GetComponent<TMP_InputField>().text = tasaInteres.ToString();
        montoCuotaText.GetComponent<TMP_InputField>().text = "0.00";
        pagoTotalDelPrestamo.GetComponent<TMP_InputField>().text = "0.00";
        VerificarCamposVacios();
    }

    public void CalcularPagoTotalDelPrestamo()
    {
        if (VerificarCamposVacios())
        {
            montoPrestamo = float.Parse(montoPrestamoInputField.text);
            nroCuotas = int.Parse(nroCuotasInputField.text);

            montoCuotas = (montoPrestamo * tasaInteres) / (1 - Mathf.Pow(1 + tasaInteres, -nroCuotas));
            montoTotalPrestamo = montoCuotas * nroCuotas; 
            montoCuotaText.text = montoCuotas.ToString("0.00");
            pagoTotalDelPrestamo.text = montoTotalPrestamo.ToString("0.00");
        }
    }


    public virtual void AgregarPrestamoAlPresupuesto()
    {
        presupuesto.ActualizarCantidadDeCuotasPorMejora(montoCuotas, "Prestamo", nroCuotas);
        presupuesto.ActualizarAhorros(montoPrestamo);
        buttonSolicitar.interactable = false;
        buttonSolicitar.GetComponentInChildren<TMP_Text>().text = "Prestamo solicitado";
    }

    private bool VerificarCamposVacios()
    {
        if (string.IsNullOrEmpty(montoPrestamoInputField.text) || string.IsNullOrEmpty(nroCuotasInputField.text))
        {
            buttonSolicitar.interactable = false;
            buttonSolicitar.GetComponentInChildren<TMP_Text>().text = "Complete los campos";
            buttonSolicitar.GetComponentInChildren<TMP_Text>().color = Color.gray;
            return false;
        }
        else
        {
            buttonSolicitar.interactable = true;
            buttonSolicitar.GetComponentInChildren<TMP_Text>().text = "Solicitar préstamo";
            buttonSolicitar.GetComponentInChildren<TMP_Text>().color = Color.white;
            return true;
        }
    }

}
