using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SobreviviendoAlAhorroGameManager : MonoBehaviour
{
    // Parte 1
    [SerializeField] private TMP_Text monedasTexto;
    [SerializeField] private TMP_Text vidasTexto;
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
    private int vidas = 3;
    private int indiceEnemigoActual = 0;
    private bool parte1Completado = false;



    private void Start()
    {
        monedas = nivelManager.dineroInicial;
        enemigos = nivelManager.enemigos;
        monedasTexto.text = "Monedas: " + monedas.ToString();
        vidasTexto.text = "Vidas: " + vidas.ToString();
        //InicializarDataEnemigos();
        StartCoroutine(SpawnEnemigos());
        SpawnEnemigo(enemigoHormiga);
    }

    private IEnumerator SpawnEnemigos()
    {
        while (!parte1Completado)
        {
            // Verifica si todos los enemigos han sido destruidos
            if (enemigosActivos() == 0 && indiceEnemigoActual >= NumeroDeEnemigosEnRonda())
            {
                // Todos los enemigos han sido destruidos, finaliza el nivel
                nivelManager.Parte1Completado();

                yield break; // Sale del Coroutine
            }

            // Si no hay ningún enemigo en pantalla, spawnea uno
            if (!HayEnemigoEnPantalla() && indiceEnemigoActual < NumeroDeEnemigosEnRonda())
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
        Instantiate(enemigo, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);
    }

    private bool HayEnemigoEnPantalla()
    {
        // Verifica si hay algún enemigo activo en la escena
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    public void RestarMonedas(int cantidad)
    {
        monedas -= cantidad;
        monedasTexto.text = "Monedas: " + monedas.ToString();
        if (monedas <= 0)
        {
            Debug.Log("El jugador ha perdido todas sus monedas. Game Over.");
            player.gameObject.SetActive(false);
            puntajeFinal.text = "Tu puntaje final es: " + monedas.ToString();
            panelFinDeJuego.SetActive(true);
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
            player.gameObject.SetActive(false);
            puntajeFinal.text = "Tu puntaje final es: " + monedas.ToString();
            panelFinDeJuego.SetActive(true);
        }
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
        vidas = 3; // Reinicia el número de vidas
        monedas = nivelManager.dineroActual; // Reinicia el número de monedas
        // Actualiza el texto del UI de vidas
        vidasTexto.text = "Vidas: " + vidas.ToString();


        // Desactiva el panel de fin de juego y otros paneles relevantes
        panelFinDeJuego.SetActive(false);
        panelNivelCompletado.SetActive(false);

        // Reactiva el jugador si estaba desactivado
        player.gameObject.SetActive(true);

        // Reinicia la corrutina de spawn de enemigos
        parte1UI.SetActive(true);
        indiceEnemigoActual = 0;
        StopCoroutine(SpawnEnemigos());
        StartCoroutine(SpawnEnemigos());
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

    public void ActivarDesactivarEnemigoTransporte(bool isActive) {         
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

    

    private int NumeroDeEnemigosEnRonda()
    {
        int count = 0;
        foreach (GameObject enemigo in enemigos)
        {
            if (enemigo.GetComponent<Enemigo_GastoFijo>().spawnearEstaRonda)
            {
                count++;
            }
        }
        return count;
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
