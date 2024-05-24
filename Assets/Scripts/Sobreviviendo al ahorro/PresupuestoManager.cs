using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PresupuestoManager : MonoBehaviour
{
    private float ahorros;
    private float sueldo = 1500;
    private float inversiones;
    private float servicios;
    private float comida;
    private float alquiler;
    private float transporte;
    private float seguro;
    private float total;

    private List<GameObject> enemigos;

    [SerializeField] private TMP_Text ahorrosTexto;
    [SerializeField] private TMP_Text sueldoTexto;
    [SerializeField] private TMP_Text inversionesTexto;
    [SerializeField] private TMP_Text serviciosTexto;
    [SerializeField] private TMP_Text comidaTexto;
    [SerializeField] private TMP_Text alquilerTexto;
    [SerializeField] private TMP_Text transporteTexto;
    [SerializeField] private TMP_Text seguroTexto;
    [SerializeField] private TMP_Text totalTexto;

    [SerializeField] SobreviviendoAlAhorroLevelManager levelManager;
    [SerializeField] SobreviviendoAlAhorroGameManager parte1_gameManager;

    public float Ahorros { get => ahorros; set => ahorros = value; }
    public float Servicios { get => servicios; set => servicios = value; }
    public float Inversiones { get => inversiones; set => inversiones = value; }
    public float Comida { get => comida; set => comida = value; }
    public float Alquiler { get => alquiler; set => alquiler = value; }
    public float Transporte { get => transporte; set => transporte = value; }
    public float Seguro { get => seguro; set => seguro = value; }
    public float Total { get => total; set => total = value; }

    // Start is called before the first frame update
    void Start()
    {
        enemigos = levelManager.enemigos;
        ahorros = parte1_gameManager.monedas;
        AsignarPrecios(enemigos);
        total = CalcularTotal();

        EscribirPresupuesto();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AsignarPrecios(List<GameObject> enemigos)
    {
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Luz") || enemigo.name.Contains("Agua"))
            {

                servicios += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
            }
            else if (enemigo.name.Contains("Comida"))
            {
                comida += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda ? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
            }
            else if (enemigo.name.Contains("Alquiler"))
            {
                alquiler += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda ? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
            }
            else if (enemigo.name.Contains("Transporte"))
            {
                transporte += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda ? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
            }
            else if (enemigo.name.Contains("Seguro"))
            {
                seguro += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda ? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
            }

        }
    }


    public void EscribirPresupuesto()
    {
        ahorrosTexto.text = "Ahorros: " + ahorros.ToString();
        sueldoTexto.text = "Sueldo: +" + sueldo.ToString();
        if(inversiones > 0)
        {
            inversionesTexto.text = "Inversiones: +" + inversiones.ToString();
        }
        else
        {
            inversionesTexto.text = "Inversiones: " + inversiones.ToString();
        }
        serviciosTexto.text = "Servicios: -" + servicios.ToString();
        comidaTexto.text = "Comida: -" + comida.ToString();
        alquilerTexto.text = "Alquiler: -" + alquiler.ToString();
        transporteTexto.text = "Transporte: -" + transporte.ToString();
        seguroTexto.text = "Seguro: -" + seguro.ToString();
        totalTexto.text = "Total: " + total.ToString();
    }

    private float CalcularTotal()
    {
        return ahorros + sueldo + inversiones - servicios - comida - alquiler - transporte - seguro;
    }

    public void ActualizarPrecioComida(float precio)
    {
        comida = precio;
        total = CalcularTotal();
        EscribirPresupuesto();
    }

    public void ActualizarPrecioTransporte(float precio)
    {
        transporte = precio;
        total = CalcularTotal();
        EscribirPresupuesto();
    }

    public void ActualizarPrecioSeguro(float precio)
    {
        seguro = precio;
        total = CalcularTotal();
        EscribirPresupuesto();
    }

    public void ActualizarGananciaInversiones(float precio)
    {
        inversiones += precio;
        total = CalcularTotal();
        EscribirPresupuesto();
    }

    public void ActualizarValoresPresupuesto(SobreviviendoAlAhorroLevelManager.Presupuesto presupuesto)
    {

        ahorros = parte1_gameManager.monedas;
        sueldo = presupuesto.sueldo;
        inversiones = presupuesto.inversiones;
        servicios = presupuesto.servicios;
        comida = presupuesto.comida;
        alquiler = presupuesto.alquiler;
        transporte = presupuesto.transporte;
        seguro = presupuesto.seguro;
        total = CalcularTotal();
        EscribirPresupuesto();

    }
}
