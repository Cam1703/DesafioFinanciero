using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SobreviviendoAlAhorroGameManager : MonoBehaviour
{
    // Parte 1
    [SerializeField] private TMP_Text monedasTexto;
    [SerializeField] public TMP_Text vidasTexto;
    [SerializeField] private PlayerController player;
    private List<GameObject> enemigos;
    [SerializeField] private GameObject panelFinDeJuego;
    [SerializeField] private TMP_Text puntajeFinal;
    [SerializeField] private float spawnInterval = 3f; // Intervalo entre cada spawn
    [SerializeField] private GameObject enemigoHormiga;
    [SerializeField] private GameObject parte1UI;
    [SerializeField] private GameObject panelNivelCompletado;
    //[SerializeField] private int nroNiveles = 3;
    [SerializeField] private SobreviviendoAlAhorroLevelManager nivelManager;

    public float monedas = 2000;
    public int totalVidas = 3;
    private int vidas = 3;
    private int indiceEnemigoActual = 0;
    private bool parte1Completado = false;
    public bool hasSeguro = false;
    private Coroutine spawnRoutine;


    private void Start()
    {
        monedas = nivelManager.dineroInicial;
        enemigos = nivelManager.enemigos;
        monedasTexto.text = "Monedas: " + monedas.ToString();
        vidasTexto.text = "Vidas: " + vidas.ToString();
        //InicializarDataEnemigos();
        spawnRoutine = StartCoroutine(SpawnEnemigos());
        SpawnEnemigo(enemigoHormiga);
    }

    private IEnumerator SpawnEnemigos()
    {
        while (!parte1Completado)
        {
            // Verifica si todos los enemigos han sido destruidos
            if (enemigosActivos() == 0 && indiceEnemigoActual >= enemigos.Count)
            {
                // Todos los enemigos han sido destruidos, finaliza el nivel
                nivelManager.Parte1Completado();

                yield break; // Sale del Coroutine
            }

            // El índice recorre la lista completa de prefabs y SpawnEnemigo salta los que
            // no aparecen esta ronda. Antes se comparaba con <= contra la CANTIDAD de
            // enemigos habilitados: con los 6 gastos activos (Taxi + Seguro comprados en
            // la tienda) se accedía a enemigos[6] → IndexOutOfRangeException, el coroutine
            // moría y el nivel nunca terminaba.
            if (indiceEnemigoActual < enemigos.Count)
            {
                SpawnEnemigo(enemigos[indiceEnemigoActual]);
                indiceEnemigoActual++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemigo(GameObject enemigo)
    {
        // Instancia el enemigo en el spawn point

        //si el enemigo es un gasto fijo, se verifica si es que se debe instanciar
        if (enemigo.GetComponent<Enemigo_GastoFijo>() != null)
        {
            if (!enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda)
            {
                return;
            }
        }

        Instantiate(enemigo, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);


    }

    // Nota: existía un HayEnemigoEnPantalla() que consultaba activeInHierarchy sobre la
    // lista de PREFABS (assets, no instancias), por lo que siempre devolvía false y la
    // condición que lo usaba era letra muerta. Se eliminó al corregir el spawn.

    public void RestarMonedas(int cantidad)
    {
        monedas -= cantidad;
        monedasTexto.text = "Monedas: " + monedas.ToString();
        if (monedas <= 0)
        {
            Debug.Log("El jugador ha perdido todas sus monedas. Game Over.");
            GameOver();
        }
    }

    public void RestarVida()
    {
        vidas--;
        vidasTexto.text = "Vidas: " + vidas.ToString();
        Debug.Log("El jugador ha sido golpeado. Vida restante: " + vidas);
        if (vidas <= 0)
        {
            Debug.Log("El jugador ha perdido todas sus vidas. Game Over.");
            GameOver();
        }
    }

    private void GameOver()
    {
        // Detiene el spawn: antes el coroutine seguía generando enemigos detrás del panel de fin de juego.
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
        player.gameObject.SetActive(false);
        puntajeFinal.text = "Tu puntaje final es: " + monedas.ToString();
        panelFinDeJuego.SetActive(true);
    }



    // Función para contar el número de enemigos activos en la escena
    private int enemigosActivos()
    {
        GameObject[] enemigosEnEscena = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;
        foreach (GameObject enemigo in enemigosEnEscena)
        {
            if (enemigo.activeInHierarchy && enemigo.GetComponent<Enemigo_GastoFijo>() != null)
            {
                count++;
            }
        }
        Debug.Log("Enemigos activos: " + count);
        return count;
    }

    public void ReanudarNivel()
    {
        parte1Completado = false;
        // Reinicia las vidas del jugador
        vidas = totalVidas; // Reinicia el número de vidas
        monedas += nivelManager.sueldo; // Reinicia el número de monedas
        Debug.Log("monedas: " + monedas);
        // Actualiza el texto del UI de vidas
        vidasTexto.text = "Vidas: " + vidas.ToString();
        monedasTexto.text = "Monedas: " + monedas.ToString();

        // Desactiva el panel de fin de juego y otros paneles relevantes
        panelFinDeJuego.SetActive(false);
        panelNivelCompletado.SetActive(false);

        // Reactiva el jugador si estaba desactivado
        player.gameObject.SetActive(true);

        // Reinicia la corrutina de spawn de enemigos. Se usa la referencia guardada:
        // StopCoroutine(SpawnEnemigos()) creaba un IEnumerator nuevo y no detenía nada,
        // así que al reintentar quedaban dos corrutinas generando enemigos en paralelo.
        parte1UI.SetActive(true);
        indiceEnemigoActual = 0;
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = StartCoroutine(SpawnEnemigos());
    }

    //public void InicializarDataEnemigos()
    //{
    //    foreach (GameObject enemigo in enemigos)
    //    {
    //        if (enemigo.name.Contains("Luz"))
    //        {
    //            enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
    //            enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
    //        }
    //        else if (enemigo.name.Contains("Agua"))
    //        {
    //            enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
    //            enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
    //        }
    //        else if (enemigo.name.Contains("Comida"))
    //        {
    //            enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
    //            enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
    //        }
    //        else if (enemigo.name.Contains("Alquiler"))
    //        {
    //            enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
    //            enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = true;
    //        }
    //        else if (enemigo.name.Contains("Transporte"))
    //        {
    //            enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
    //            enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = false;
    //        }
    //        else if (enemigo.name.Contains("Seguro"))
    //        {
    //            enemigo.GetComponent<Enemigo_GastoFijo>().vida = 100;
    //            enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda = false;
    //        }
    //    }
    //}

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



    public void DestruirEnemigosYHormigas()
    {
        GameObject[] enemigosEnEscena = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigosEnEscena)
        {
            Destroy(enemigo);
        }

    }




}
