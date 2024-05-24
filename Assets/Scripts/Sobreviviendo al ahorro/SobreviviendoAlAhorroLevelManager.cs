using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SobreviviendoAlAhorroLevelManager : MonoBehaviour
{

    class SobreviviendoAlAhorroLevel
    {
        public int id;
        public Presupuesto presupuestoActual;
        public List<GameObject> enemigosActuales;
        public bool completado;
        public bool Parte1Completado;
        public bool Parte2Completado;

        public SobreviviendoAlAhorroLevel(int id, Presupuesto presupuestoActual, List<GameObject> enemigosActuales, bool completado, bool parte1Completado, bool parte2Completado)
        {
            this.id = id;
            this.presupuestoActual = presupuestoActual;
            this.enemigosActuales = enemigosActuales;
            this.completado = completado;
            Parte1Completado = parte1Completado;
            Parte2Completado = parte2Completado;
        }
    }

    public class Presupuesto
    {
        public float ahorros;
        public float sueldo;
        public float inversiones;
        public float servicios;
        public float comida;
        public float alquiler;
        public float transporte;
        public float seguro;
        public float total;

        public Presupuesto(float ahorros, float sueldo, float inversiones, float servicios, float comida, float alquiler, float transporte, float seguro, float total)
        {
            this.ahorros = ahorros;
            this.sueldo = sueldo;
            this.inversiones = inversiones;
            this.servicios = servicios;
            this.comida = comida;
            this.alquiler = alquiler;
            this.transporte = transporte;
            this.seguro = seguro;
            this.total = total;
        }


    }


    // Start is called before the first frame update
    [SerializeField] private GameObject panelNivelCompletado;
    [SerializeField] private GameObject panelFinDeJuego;
    [SerializeField] private TMP_Text puntajeFinal;
    [SerializeField] private GameObject parte1UI;
    [SerializeField] private GameObject parte2_panelAdministracionDinero;
    private List<SobreviviendoAlAhorroLevel> niveles = new List<SobreviviendoAlAhorroLevel>();
    [SerializeField] public List<GameObject> enemigos;
    [SerializeField] private SobreviviendoAlAhorroGameManager parte1_gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private InversionesManager inversionesManager;
    [SerializeField] private PresupuestoManager presupuestoManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BancoManager bancoManager;

    private int nroNivelActual = 0;
    private int nroNiveles = 3;
    public float dineroInicial = 2000;
    public float sueldo = 1500;
    private SobreviviendoAlAhorroLevel nivelActual;

    void Start()
    {
        parte1_gameManager.monedas = dineroInicial;
        InicializarNiveles(nroNiveles);
        InicializarPresupuesto();
        InicializarDataEnemigos();
        nivelActual = niveles[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InicializarNiveles(int nroNiveles)
    {
        for (int i = 0; i < nroNiveles; i++)
        {
            SobreviviendoAlAhorroLevel nivel = new SobreviviendoAlAhorroLevel(
                i,
                null,
                enemigos,
                false,
                false,
                false
                );
            niveles.Add(nivel);
        }
    }

    private void InicializarPresupuesto()
    {
        float servicios = 0;
        float comida = 0;
        float alquiler = 0;
        float transporte = 0;
        float seguro = 0;

        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Luz") || enemigo.name.Contains("Agua"))
            {

                servicios += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda ? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
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

        Presupuesto presupuesto = new Presupuesto(dineroInicial, sueldo, 0, servicios, comida, alquiler, transporte, seguro, 0);
        niveles[0].presupuestoActual = presupuesto;
    }

    private void ActualizarPresupuesto(SobreviviendoAlAhorroLevel nivelActual)
    {
        float servicios = 0;
        float comida = 0;
        float alquiler = 0;
        float transporte = 0;
        float seguro = 0;
        float inversiones = inversionesManager.CalcularGananciaInversiones();
        inversionesManager.CancelarInversionAltoYBajoRiesgo(); // Se cancelan las inversiones al final de la ronda para que no se acumulen 
        

        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Luz") || enemigo.name.Contains("Agua"))
            {

                servicios += enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda ? enemigo.GetComponent<Enemigo_GastoFijo>().vida : 0;
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
        float total = parte1_gameManager.monedas + sueldo + inversiones - servicios - comida - alquiler - transporte - seguro;
        Presupuesto presupuesto = new Presupuesto(parte1_gameManager.monedas, sueldo, inversiones, servicios, comida, alquiler, transporte, seguro, total);
        presupuestoManager.ActualizarValoresPresupuesto(presupuesto);
        nivelActual.presupuestoActual = presupuesto;
        parte1_gameManager.monedas += inversiones;
    }



    public void FinalizarParte2()
    {
        nivelActual.Parte2Completado = true;
        nivelActual.completado = true;
        bancoManager.UpdateMontoGanadoHastaLaFecha();
        nroNivelActual++;
        Debug.Log("Parte 2 completada!");
        Debug.Log(nroNivelActual);
        if (nroNivelActual < nroNiveles)
        {
            CambiarNivel(nroNivelActual);
        }
        else
        {
            panelFinDeJuego.SetActive(true);
            parte2_panelAdministracionDinero.SetActive(false);
            puntajeFinal.text = "Puntaje final: " + parte1_gameManager.monedas;
        }

    }

    public void CambiarNivel(int idNuevoNivel)
    {
        nivelActual = niveles[idNuevoNivel];
        parte2_panelAdministracionDinero.SetActive(false);
        parte1_gameManager.ReanudarNivel();
    }

    public void Parte1Completado()
    {
        nivelActual.Parte1Completado = true;
        Debug.Log("Nivel completado!");
        parte1UI.SetActive(false);
        player.gameObject.SetActive(false);
        panelNivelCompletado.SetActive(true);
        parte1_gameManager.DestruirEnemigosYHormigas();
    }

    public void IniciarParte2()
    {
        parte2_panelAdministracionDinero.SetActive(true);
        gameManager.InitializeButtons();

        panelNivelCompletado.SetActive(false);
        ActualizarPresupuesto(nivelActual);
        presupuestoManager.EscribirPresupuesto();
    }

    public void InicializarDataEnemigos()
    {
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Luz"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
            }
            else if (enemigo.name.Contains("Agua"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
            }
            else if (enemigo.name.Contains("Comida"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
            }
            else if (enemigo.name.Contains("Alquiler"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
            }
            else if (enemigo.name.Contains("Transporte"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = false;
            }
            else if (enemigo.name.Contains("Seguro"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = false;
            }
        }
    }

    public void ActivarDesactivarEnemigoTransporte(bool isActive)
    {
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Transporte"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = isActive;
            }
        }
    }

    public void ActivarDesactivarEnemigoSeguro(bool isActive)
    {
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Seguro"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = isActive;
            }
        }
    }

    public void ActualizarPrecioComida(int precio)
    {
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.name.Contains("Comida"))
            {
                enemigo.GetComponent<Enemigo_GastoFijo>().vida = precio;
            }
        }
    }
}