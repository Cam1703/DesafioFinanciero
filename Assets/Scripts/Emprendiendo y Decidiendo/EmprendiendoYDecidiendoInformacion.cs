using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmprendiendoYDecidiendoInformacion : MonoBehaviour
{
    private int cantidadDeClientes = 50;
    public float gananciasMensuales = 3000f;
    private float porcentajeDePopularidad = 50f;
    private float porcentajeDeSatisfaccionClientes = 75f;
    private float porcentajeDeSatisfaccionEmpleados = 50f;
    private int cantidadDeEmpleados; //viene de presupuesto
    private float sueldoDeEmpleados; //viene de presupuesto

    private bool hasLocalPropio = false;
    private bool hasLocalAlquiladoExpandido = false;

    private bool hasPublicidad = false;
    private bool hasMejoresInsumos = false;
    private bool hasMejoresSillas = false;

    private List<string> riesgos = new List<string>();


    [SerializeField] private TMP_Text cantidadDeClientesText;
    [SerializeField] private TMP_Text gananciasMensualesText;
    [SerializeField] private TMP_Text porcentajeDePopularidadText;
    [SerializeField] private TMP_Text porcentajeDeSatisfaccionClientesText;
    [SerializeField] private TMP_Text porcentajeDeSatisfaccionEmpleadosText;

    [SerializeField] private TMP_Text riesgo1;
    [SerializeField] private TMP_Text riesgo2;

    [SerializeField] private EmprendiendoYDecidiendoPresupuesto presupuesto;

    void Start()
    {
        cantidadDeEmpleados = presupuesto.cantidadDeEmpleados;
        sueldoDeEmpleados = presupuesto.sueldoEmpleados;
        riesgos.Add("Riesgo 1: Baja demanda de productos");
        riesgos.Add("Riesgo 2: Aumento de competencia");
        riesgos.Add("Riesgo 3: Problemas de logística");
        ActualizarInformacion();
    }

    // Método para actualizar la información mostrada
    public void ActualizarInformacion()
    {
        cantidadDeClientesText.text = "Cantidad de Clientes: " + cantidadDeClientes.ToString();
        gananciasMensualesText.text = "Ganancias Mensuales: " +  gananciasMensuales.ToString();
        porcentajeDePopularidadText.text = "Porcentaje Popularidad: " + CalcularPorcentajeDePopularidad().ToString("P1");
        porcentajeDeSatisfaccionClientesText.text = "Satisfacción del Cliente: " + CalcularPorcentajeDeSatisfaccionClientes().ToString("P1");
        porcentajeDeSatisfaccionEmpleadosText.text = "Cantidad de Empleados: " + CalcularPorcentajeDeSatisfaccionEmpleados().ToString("P1");
        MostrarRiesgo();
    }

    // Método para calcular el porcentaje de popularidad
    private float CalcularPorcentajeDePopularidad()
    {
        // Convertir el porcentaje de popularidad a decimal
        float factorDePopularidad = porcentajeDePopularidad / 100f;

        // Incrementar el factor de popularidad según las condiciones
        if (hasPublicidad)
        {
            factorDePopularidad *= 1.2f; // Incremento del 20% por tener publicidad
        }

        // Calcular el incremento adicional basado en la satisfacción de clientes
        float incrementoSatisfaccionClientes = CalcularPorcentajeDeSatisfaccionClientes() * 0.1f; // Incremento del 10% por la satisfacción de clientes
        factorDePopularidad += incrementoSatisfaccionClientes;

        // Asegurar que el factor de popularidad no supere el 100%
        factorDePopularidad = Mathf.Min(factorDePopularidad, 1f);

        // Convertir de nuevo a porcentaje y devolver el valor
        return factorDePopularidad ;
    }

    // Método para calcular el porcentaje de satisfacción de clientes
    private float CalcularPorcentajeDeSatisfaccionClientes()
    {
        // Convertir el porcentaje de satisfacción de clientes a decimal
        float factorDeSatisfaccion = porcentajeDeSatisfaccionClientes / 100f;

        // Incrementar el factor de satisfacción según las condiciones
        if (hasMejoresSillas)
        {
            factorDeSatisfaccion *= 1.1f; // Incremento del 10% por tener mejores sillas
        }
        if (hasMejoresInsumos)
        {
            factorDeSatisfaccion *= 1.2f; // Incremento del 20% por tener mejores insumos
        }
        if (hasLocalPropio)
        {
            factorDeSatisfaccion *= 1.1f; // Incremento del 10% por tener local propio
        }
        if (hasLocalAlquiladoExpandido)
        {
            factorDeSatisfaccion *= 1.2f; // Incremento del 20% por tener local alquilado expandido
        }

        // Asegurar que el factor de satisfacción no supere el 100%
        factorDeSatisfaccion = Mathf.Min(factorDeSatisfaccion, 1f);

        // Convertir de nuevo a porcentaje y devolver el valor
        return factorDeSatisfaccion ;
    }


    // Método para calcular el porcentaje de satisfacción de empleados
    private float CalcularPorcentajeDeSatisfaccionEmpleados()
    {
        // Convertir el porcentaje de satisfacción de empleados a decimal
        float factorDeSatisfaccion = porcentajeDeSatisfaccionEmpleados / 100f;

        // Incrementar el factor de satisfacción según la cantidad de empleados y el sueldo
        factorDeSatisfaccion += (cantidadDeEmpleados * 0.01f); // Incremento de 1% por cada empleado
        factorDeSatisfaccion += (sueldoDeEmpleados / 1000f) * 0.1f; // Incremento de 10% por cada $1000 de sueldo

        // Asegurar que el factor de satisfacción no supere el 100%
        factorDeSatisfaccion = Mathf.Min(factorDeSatisfaccion, 1f);

        // Convertir de nuevo a porcentaje y devolver el valor
        return factorDeSatisfaccion;
    }


    //Metodo para calcular el puntaje total del jugador
    public int CalcularPuntaje()
    {
        int puntaje = 0;
        puntaje += (int)gananciasMensuales;
        puntaje += (int)(cantidadDeClientes * 0.5f);
        puntaje += (int)(CalcularPorcentajeDePopularidad() * 10);
        puntaje += (int)(CalcularPorcentajeDeSatisfaccionClientes() * 10);
        puntaje += (int)(CalcularPorcentajeDeSatisfaccionEmpleados() * 10);
        return puntaje;
    }

    public void SetHasLocalPropio(bool value)
    {
        hasLocalPropio = value;
    }

    public void SetHasLocalAlquiladoExpandido(bool value)
    {
        hasLocalAlquiladoExpandido = value;
    }

    public void SetHasPublicidad(bool value)
    {
        hasPublicidad = value;
    }

    public void SetHasMejoresInsumos(bool value)
    {
        hasMejoresInsumos = value;
    }

    public void SetHasMejoresSillas(bool value)
    {
        hasMejoresSillas = value;
    }

    public void SetCantidadEmpleados(int cantidad)
    {
        cantidadDeEmpleados = cantidad;
    }

    public void SetSueldoEmpleados(float sueldo)
    {
        sueldoDeEmpleados = sueldo;
    }

    //funcion para mostrar el riesgo con una probabilidad de 50%
    public void MostrarRiesgo()
    {
        
        if (Random.value > 0.5f)
        {
            riesgo1.text = riesgos[Random.Range(0, riesgos.Count-1)];
        }
        else
        {
            riesgo1.text = "";
        }

        if (Random.value > 0.5f)
        {
            riesgo2.text = riesgos[Random.Range(0, riesgos.Count-1)];
        }
        else
        {
            riesgo2.text = "";
        }
    }
}
