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
    [SerializeField] private TMP_InputField inputFieldDineroEnCuentaAhorros;
    [SerializeField] private TMP_InputField inputFieldDineroEnPlazoFijo;
    [SerializeField] private GameObject montoGanadoPlazoFijoPanel;
    [SerializeField] private GameObject depositoInicialPlazoFijoPanel;
    [SerializeField] private GameObject mesesPlazoFijoPanel;
    [SerializeField] private GameObject gananciaPlazoFijoPanel;

    [SerializeField] private PresupuestoManager presupuestoManager;

    private float depositoInicialCuentaAhorros;
    private float depositoInicialPlazoFijo;
    private float meses;
    private float mesesPasados;
    private float ganancia;
    private float interesPlazoFijo;
    private float dineroEnCuentaAhorros;
    private float dineroEnPlazoFijo;



    // Start is called before the first frame update
    void Start()
    {
        inputFieldGanancia.interactable = false;
        inputFieldGanancia.text = "";
        interesPlazoFijo = 0.05f;
        montoGanadoPlazoFijoPanel.SetActive(false);
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
            dineroEnCuentaAhorros += depositoInicialCuentaAhorros;
            inputFieldDepositoInicialCuentaAhorros.text = "";
            inputFieldDineroEnCuentaAhorros.text = depositoInicialCuentaAhorros.ToString();
        }
        else
        {
            Debug.Log("Error al obtener el deposito inicial de la cuenta de ahorros");
        }
        button.GetComponentInChildren<TMP_Text>().text = "Depositar";

    }



    public void GetInputPlazoFijoOnClick(Button button)
    {
        if (button.GetComponentInChildren<TMP_Text>().text == "Adquirir")
        {
            if (float.TryParse(inputFieldDepositoInicialPlazoFijo.text, out depositoInicialPlazoFijo))
            {
                Debug.Log("Deposito inicial plazo fijo: " + depositoInicialPlazoFijo);
                dineroEnPlazoFijo += depositoInicialPlazoFijo;
                inputFieldDepositoInicialPlazoFijo.text = "";
                inputFieldMeses.text = "";
                inputFieldGanancia.text = "";
                inputFieldDineroEnPlazoFijo.text = depositoInicialPlazoFijo.ToString();
            }
            else
            {
                Debug.Log("Error al obtener el deposito inicial del plazo fijo");
            }
            presupuestoManager.Ahorros -= depositoInicialPlazoFijo;
            presupuestoManager.EscribirPresupuesto();
            montoGanadoPlazoFijoPanel.SetActive(true);
            depositoInicialPlazoFijoPanel.SetActive(false);
            mesesPlazoFijoPanel.SetActive(false);
            gananciaPlazoFijoPanel.SetActive(false);
        }
        else
        {
            RetirarDineroPlazoFijo();
            mesesPasados = 0;
            inputFieldDepositoInicialPlazoFijo.text = "";
            montoGanadoPlazoFijoPanel.SetActive(false);
            depositoInicialPlazoFijoPanel.SetActive(true);
            mesesPlazoFijoPanel.SetActive(true);
            gananciaPlazoFijoPanel.SetActive(true);
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

    public void UpdateMontoGanadoHastaLaFecha()
    {
        mesesPasados++;
        inputFieldDineroEnPlazoFijo.text = GetGananciaActualPlazoFijo().ToString();
        dineroEnPlazoFijo = GetGananciaActualPlazoFijo();
    }

    private float GetGananciaActualPlazoFijo()
    {
        return depositoInicialPlazoFijo + depositoInicialPlazoFijo * interesPlazoFijo * mesesPasados;
    }

    private void RetirarDineroPlazoFijo()
    {
        dineroEnPlazoFijo = 0;
        inputFieldDineroEnPlazoFijo.text = dineroEnPlazoFijo.ToString();
        presupuestoManager.Ahorros += depositoInicialPlazoFijo;
        if(mesesPasados< meses)
        {
            //descontar penalización
            presupuestoManager.Ahorros -= depositoInicialPlazoFijo * 0.1f;
        }
        presupuestoManager.EscribirPresupuesto();
    }
}
