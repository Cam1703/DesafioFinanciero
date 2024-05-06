using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BancoManager : MonoBehaviour
{
    [SerializeField] private SobreviviendoAlAhorroGameManager gameManager;
    [SerializeField] private GameObject panelCuentaAhorros;
    [SerializeField] private GameObject panelPlazoFijo;
    [SerializeField] private Button buttonCuentaAhorros;
    [SerializeField] private Button buttonPlazoFijo;
    [SerializeField] private Sprite depositarBG;
    [SerializeField] private Sprite removerBG;
    [SerializeField] private TMP_InputField inputFieldDepositoInicialCuentaAhorros;
    [SerializeField] private TMP_InputField inputFieldDepositoInicialPlazoFijo;
    [SerializeField] private TMP_InputField inputFieldMeses;
    [SerializeField] private TMP_InputField inputFieldGanancia;

    private float depositoInicialCuentaAhorros;
    private float depositoInicialPlazoFijo;
    private float meses;
    private float ganancia;
    private float interesPlazoFijo;
    private float interesCuentaAhorros;
    private float dineroEnCuentaAhorros;
    private float dineroEnPlazoFijo;

    // Start is called before the first frame update
    void Start()
    {
        inputFieldGanancia.interactable = false;
        inputFieldGanancia.text = "";
        interesPlazoFijo = 0.1f;
        interesCuentaAhorros = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateButtonAppearance(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        Image buttonImage = button.GetComponent<Image>();
        if (buttonText.text == "Adquirir")
        {
            buttonImage.sprite = removerBG;
            buttonText.text = "Retirar";
            //panelCuentaAhorros.SetActive(true);
        }
        else
        {
            buttonText.text = "Adquirir";
            buttonImage.sprite = depositarBG;
            //panelCuentaAhorros.SetActive(false);

        }
    }


    public void GetInputCuentaAhorrosOnClick(Button button)
    {
        if (float.TryParse(inputFieldDepositoInicialCuentaAhorros.text, out depositoInicialCuentaAhorros))
        {
            Debug.Log("Deposito inicial cuenta ahorros: " + depositoInicialCuentaAhorros);
        }
        else
        {
            Debug.Log("Error al obtener el deposito inicial de la cuenta de ahorros");
        }
        UpdateButtonAppearance(button);
        
    }

    public void GetInputPlazoFijoOnClick(Button button)
    {
        if (float.TryParse(inputFieldDepositoInicialPlazoFijo.text, out depositoInicialPlazoFijo))
        {
            Debug.Log("Deposito inicial plazo fijo: " + depositoInicialPlazoFijo);
        }
        else
        {
            Debug.Log("Error al obtener el deposito inicial del plazo fijo");
        }

        if (float.TryParse(inputFieldMeses.text, out meses))
        {
            Debug.Log("Meses: " + meses);
        }
        else
        {
            Debug.Log("Error al obtener los meses");
        }



        UpdateButtonAppearance(button);

    }

    private float GetGananciaTotalPlazoFijo()
    {
        dineroEnPlazoFijo = depositoInicialPlazoFijo * (1 + interesPlazoFijo * meses);
        Debug.Log("Dinero en plazo fijo: " + dineroEnPlazoFijo);
        Debug.Log("Interes plazo fijo: " + interesPlazoFijo);
        Debug.Log("Meses: " + meses);
        Debug.Log("Ganancia: " + dineroEnPlazoFijo);

        return dineroEnPlazoFijo;
    }

    private float GetGananciaActualCuentaAhorros(int mesActual)
    {
       
        return dineroEnCuentaAhorros * (1 + interesCuentaAhorros * mesActual);
    }

    private float GetGananciaActualPlazoFijo(int mesActual)
    {
        return depositoInicialPlazoFijo * (1 + interesPlazoFijo * mesActual);
    }

    public void CalcularGananciaConInput()
    {
        if (float.TryParse(inputFieldDepositoInicialPlazoFijo.text, out depositoInicialPlazoFijo))
        {
            Debug.Log("Deposito inicial plazo fijo: " + depositoInicialPlazoFijo);
        }
        else
        {
            Debug.Log("Error al obtener el deposito inicial del plazo fijo");
        }

        if (float.TryParse(inputFieldMeses.text, out meses))
        {
            Debug.Log("Meses: " + meses);
        }
        else
        {
            Debug.Log("Error al obtener los meses");
        }

        inputFieldGanancia.text = GetGananciaTotalPlazoFijo().ToString();
        Debug.Log("Ganancia total plazo fijo: " + GetGananciaTotalPlazoFijo());
    }
}
