using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmprendiendoYDecidiendoPresupuesto : MonoBehaviour
{
    private float ahorros = 2000f;
    private float gananciasMensuales; //viene de informacion
    private float alquilerLocal = 500f;
    public float sueldoEmpleados = 1025f;
    public int cantidadDeEmpleados = 1;
    private float insumos = 500f;
    private float servicios = 500f;
    private float pagoDeCuotas = 0;
    private float seguros = 0f;
    private float total = 0;
    

    [SerializeField] private TMP_Text ahorrosText;
    [SerializeField] private TMP_Text gananciasMensualesText;
    [SerializeField] private TMP_Text alquilerLocalText;
    [SerializeField] private TMP_Text sueldoEmpleadosText;
    [SerializeField] private TMP_Text cantidadDeEmpleadosText;
    [SerializeField] private TMP_Text insumosText;
    [SerializeField] private TMP_Text serviciosText;
    [SerializeField] private TMP_Text pagoDeCuotasText;
    [SerializeField] private TMP_Text totalText;
    [SerializeField] private TMP_Text segurosText;

    [SerializeField] private EmprendiendoYDecidiendoInformacion informacion;

    private int mesesRestantesCuotaExpandirLocal = 0;
    private int mesesRestantesCuotaMejoresInsumos = 0;
    private int mesesRestantesCuotaCampañaPublicitaria = 0;
    private int mesesRestantesCuotaMejoresSillas = 0;
    private int mesesRestantesCuotaPrestamo = 0;
    private int mesesRestantesPagoEventos = 0;

    private float montoCuotaExpandirLocal = 0f;
    private float montoCuotaMejoresInsumos = 0f;
    private float montocuotaCampañaPublicitaria = 0f;
    private float montoCuotaMejoresSillas = 0f;
    private float montoCuotaPrestamo = 0f;
    private float montoCuotaPagoEventos = 0f;

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
        segurosText.text = "Seguros: -" + seguros.ToString();
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

    public void ActualizarPagoDeCuotas()
    {
        pagoDeCuotas = montocuotaCampañaPublicitaria + montoCuotaExpandirLocal + montoCuotaMejoresInsumos + montoCuotaMejoresSillas + montoCuotaPrestamo + montoCuotaPagoEventos;
        ActualizarPresupuesto();
    }

    public void ActualizarPagoSeguros(float cantidad)
    {
        seguros += cantidad;
        ActualizarPresupuesto();
    }

    private void CalcularTotal()
    {
        total = ahorros + gananciasMensuales - alquilerLocal - sueldoEmpleados * cantidadDeEmpleados - insumos - servicios - pagoDeCuotas - seguros;
        totalText.text = "Total: " + total.ToString();
    }

    public void OnClickAumentarSueldo()
    {
        ActualizarSueldoEmpleados(100f);
        informacion.SetSueldoEmpleados(sueldoEmpleados);
    }

    public void OnClickDisminuirSueldo()
    {
        ActualizarSueldoEmpleados(-100f);
        informacion.SetSueldoEmpleados(sueldoEmpleados);
    }

    public void OnClickAumentarCantidadDeEmpleados()
    {
        ActualizarCantidadDeEmpleados(1);
        informacion.SetCantidadEmpleados(cantidadDeEmpleados);
    }

    public void OnClickDisminuirCantidadDeEmpleados()
    {
        ActualizarCantidadDeEmpleados(-1);
        informacion.SetCantidadEmpleados(cantidadDeEmpleados);
    }

    public void ActualizarCantidadDeCuotasPorMejora(float cuota, string mejora, int nroCuotas)
    {
        if (mejora == "Expandir Local")
        {
            montoCuotaExpandirLocal = cuota;
            mesesRestantesCuotaExpandirLocal = nroCuotas;
            informacion.SetHasLocalAlquiladoExpandido(true);
        }
        else if (mejora == "Mejores Insumos")
        {
            montoCuotaMejoresInsumos = cuota;
            mesesRestantesCuotaMejoresInsumos = nroCuotas;
            informacion.SetHasMejoresInsumos(true);
        }
        else if (mejora == "Campaña Publicitaria")
        {
            montocuotaCampañaPublicitaria = cuota;
            mesesRestantesCuotaCampañaPublicitaria = nroCuotas;
            informacion.SetHasPublicidad(true);
        }
        else if (mejora == "Mejores Sillas")
        {
            montoCuotaMejoresSillas = cuota;
            mesesRestantesCuotaMejoresSillas = nroCuotas;
            informacion.SetHasMejoresSillas(true);
        }
        else if (mejora == "Prestamo")
        {
            montoCuotaPrestamo = cuota;
            mesesRestantesCuotaPrestamo = nroCuotas;
        }
        else if (mejora == "Pago de Eventos")
        {
            montoCuotaPagoEventos = cuota;
            mesesRestantesPagoEventos = nroCuotas;
        }

        ActualizarPagoDeCuotas();
        ActualizarPresupuesto();
    }

    public void PagarCuotas()
    {
        if (mesesRestantesCuotaExpandirLocal > 0)
        {
            mesesRestantesCuotaExpandirLocal--;
            ActualizarAhorros(-montoCuotaExpandirLocal);
        }
        if (mesesRestantesCuotaMejoresInsumos > 0)
        {
            mesesRestantesCuotaMejoresInsumos--;
            ActualizarAhorros(-montoCuotaMejoresInsumos);
        }
        if (mesesRestantesCuotaCampañaPublicitaria > 0)
        {
            mesesRestantesCuotaCampañaPublicitaria--;
            ActualizarAhorros(-montocuotaCampañaPublicitaria);
        }
        if (mesesRestantesCuotaMejoresSillas > 0)
        {
            mesesRestantesCuotaMejoresSillas--;
            ActualizarAhorros(-montoCuotaMejoresSillas);
        }
        if (mesesRestantesCuotaPrestamo > 0)
        {
            mesesRestantesCuotaPrestamo--;
            ActualizarAhorros(-montoCuotaPrestamo);
        }
        if (mesesRestantesPagoEventos > 0)
        {
            mesesRestantesPagoEventos--;
            ActualizarAhorros(-montoCuotaPagoEventos);
        }
        ActualizarPagoDeCuotas();
    }

    public void RealizarPagos()
    {
        ActualizarAhorros(-alquilerLocal);
        ActualizarAhorros(-sueldoEmpleados * cantidadDeEmpleados);
        ActualizarAhorros(-insumos);
        ActualizarAhorros(-servicios);
        ActualizarAhorros(-seguros);
        ActualizarAhorros(gananciasMensuales);
        PagarCuotas();
        gananciasMensuales = informacion.gananciasMensuales;
        ActualizarPresupuesto();
    }
}
