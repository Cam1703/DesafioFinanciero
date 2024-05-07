using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmprendiendoYDecidiendoPresupuesto : MonoBehaviour
{
    private float ahorros= 2000f;
    private float gananciasMensuales; //viene de informacion
    private float alquilerLocal = 500f;
    public float sueldoEmpleados = 1025f;
    public int cantidadDeEmpleados = 1;
    private float insumos = 500f;
    private float servicios = 500f;
    private float pagoDeCuotas = 0;
    private float total=0;


    [SerializeField] private TMP_Text ahorrosText;
    [SerializeField] private TMP_Text gananciasMensualesText;
    [SerializeField] private TMP_Text alquilerLocalText;
    [SerializeField] private TMP_Text sueldoEmpleadosText;
    [SerializeField] private TMP_Text cantidadDeEmpleadosText;
    [SerializeField] private TMP_Text insumosText;
    [SerializeField] private TMP_Text serviciosText;
    [SerializeField] private TMP_Text pagoDeCuotasText;
    [SerializeField] private TMP_Text totalText;

    [SerializeField] private EmprendiendoYDecidiendoInformacion informacion;

    private int mesesRestantesCuotaExoabdirLocal = 0;
    private int mesesRestantesCuotaMejoresInsumos = 0;
    private int mesesRestantesCuotaCampañaPublicitaria = 0;
    private int mesesRestantesCuotaMejoresSillas = 0;

    // Start is called before the first frame update
    void Start()
    {
        gananciasMensuales = informacion.gananciasMensuales;
        ActualizarPresupuesto();
    }

   
    public void ActualizarPresupuesto()
    {
        ahorrosText.text = "Ahorros: " + ahorros.ToString();
        gananciasMensualesText.text = "Ganancias: " + gananciasMensuales.ToString();
        alquilerLocalText.text = "Alquiler: -" + alquilerLocal.ToString();
        sueldoEmpleadosText.text = "Sueldo: -" + sueldoEmpleados.ToString();
        cantidadDeEmpleadosText.text = "Empleados: x" + cantidadDeEmpleados.ToString();
        insumosText.text = "Insumos: -" + insumos.ToString();
        serviciosText.text = "Servicios: -" + servicios.ToString();
        pagoDeCuotasText.text = "Pago_de_Cuotas: -" + pagoDeCuotas.ToString();
        CalcularTotal();

    }


    public void ActualizarAhorros(float cantidad)
    {
        ahorros += cantidad;
        ActualizarPresupuesto();
    }

    public void ActualizarAlquilerLocal(float cantidad)
    {
        alquilerLocal += cantidad;
        ActualizarPresupuesto();
    }

    public void ActualizarSueldoEmpleados(float cantidad)
    {
        sueldoEmpleados += cantidad;
        ActualizarPresupuesto();
    }

    public void ActualizarCantidadDeEmpleados(int cantidad)
    {
        cantidadDeEmpleados += cantidad;
        ActualizarPresupuesto();
    }

    public void ActualizarInsumos(float cantidad)
    {
        insumos += cantidad;
        ActualizarPresupuesto();
    }

    public void ActualizarServicios(float cantidad)
    {
        servicios += cantidad;
        ActualizarPresupuesto();
    }

    public void ActualizarPagoDeCuotas(float cantidad)
    {
        pagoDeCuotas += cantidad;
        ActualizarPresupuesto();
    }

    private void CalcularTotal()
    {
        total = ahorros + gananciasMensuales - alquilerLocal - sueldoEmpleados * cantidadDeEmpleados - insumos - servicios - pagoDeCuotas;
        totalText.text = "Total: " + total.ToString();
    }

    public void OnClickAumentarSueldo()
    {
        ActualizarSueldoEmpleados(100f);
    }

    public void OnClickDisminuirSueldo()
    {
        ActualizarSueldoEmpleados(-100f);
    }

    public void OnClickAumentarCantidadDeEmpleados()
    {
        ActualizarCantidadDeEmpleados(1);
    }

    public void OnClickDisminuirCantidadDeEmpleados()
    {
        ActualizarCantidadDeEmpleados(-1);
    }



}
