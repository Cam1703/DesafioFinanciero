using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Nivel
{
    public string dialogo;
    public string informacionPersonaje;
    public List<string> opciones;
    public int opcionCorrecta;
    public Sprite personaje;
    public int opcionCorrectaOfertantesODemandantes;
}

public class DecisionesBancariasScript : MonoBehaviour
{
    // Referencias a los elementos de la interfaz
    [SerializeField] private Button ofertantes;
    [SerializeField] private Button demandantes;
    [SerializeField] private Button decisionesBancarias_opcion1;
    [SerializeField] private Button decisionesBancarias_opcion2;
    [SerializeField] private Button decisionesBancarias_opcion3;
    [SerializeField] private Image personajeUI;
    [SerializeField] private TMP_Text puntos;
    [SerializeField] private TMP_Text informacionPersonaje;
    [SerializeField] private Image dialogoPersonaje;
    [SerializeField] private List<Sprite> personajes;
    [SerializeField] private GameObject panel;
    // Variables del juego
    private int nivelActual = 0;
    private int puntaje = 0;
    [SerializeField] private int puntosAFavor = 10;
    [SerializeField] private int puntosEnContra = -5; 

    // Informaci�n de los niveles
    private List<Nivel> niveles = new List<Nivel>();

    // Inicializaci�n
    void Start()
    {
        // Inicializa niveles
        InicializarNiveles();

        // Inicializa el juego
        puntos.text = "Puntaje: 0";
        ActualizarNivel();

        // Asigna manejadores de eventos a los botones
        ofertantes.onClick.AddListener(() => SeleccionarOpcionOfertantesODemandantes(0));
        demandantes.onClick.AddListener(() => SeleccionarOpcionOfertantesODemandantes(1));
        decisionesBancarias_opcion1.onClick.AddListener(() => TomarDecision(0));
        decisionesBancarias_opcion2.onClick.AddListener(() => TomarDecision(1));
        decisionesBancarias_opcion3.onClick.AddListener(() => TomarDecision(2));
    }

    // Inicializa la lista de niveles
    void InicializarNiveles()
    {
        // Ejemplo de inicializaci�n de niveles
        niveles.Add(new Nivel
        {
            dialogo = "Hola, soy Mar�a y necesito dinero para hacer crecer mi negocio.",
            informacionPersonaje = "Historial crediticio s�lido y necesidad de financiamiento para expansi�n.",
            opciones = new List<string> { "Prestamo1", "Prestamo2", "Prestamo3" },
            opcionCorrecta = 1, // La opci�n correcta es la opci�n 2 (�ndice 1)
            personaje = personajes[0],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "Soy Juan, necesito un pr�stamo para comprar una casa.",
            informacionPersonaje = "Estabilidad laboral y buenos ingresos mensuales.",
            opciones = new List<string> { "Prestamo Hipotecario", "Prestamo Personal", "Prestamo de Consumo" },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[1],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        // Contin�a agregando m�s niveles seg�n sea necesario.
    }

    // Actualiza la informaci�n del nivel actual
    void ActualizarNivel()
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Actualiza los textos y el personaje
        dialogoPersonaje.GetComponentInChildren<TMP_Text>().text = nivel.dialogo;
        informacionPersonaje.text = nivel.informacionPersonaje;
        personajeUI.sprite = nivel.personaje;

        // Muestra y oculta elementos seg�n la secci�n actual
        dialogoPersonaje.gameObject.SetActive(true);
        ofertantes.gameObject.SetActive(true);
        demandantes.gameObject.SetActive(true);
        decisionesBancarias_opcion1.gameObject.SetActive(false);
        decisionesBancarias_opcion2.gameObject.SetActive(false);
        decisionesBancarias_opcion3.gameObject.SetActive(false);
        informacionPersonaje.gameObject.SetActive(false);
    }

    // Selecciona una opci�n de ofertantes o demandantes
    void SeleccionarOpcionOfertantesODemandantes(int opcionSeleccionada)
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Verifica si la elecci�n es correcta
        if (opcionSeleccionada == nivel.opcionCorrectaOfertantesODemandantes)
        {
            // Incrementa el puntaje
            puntaje += puntosAFavor;
        }
        else
        {
            // Decrementa el puntaje
            puntaje += puntosEnContra;
        }

        // Actualiza el texto de puntos
        puntos.text = "Puntaje: " + puntaje;

        // Cambia a la siguiente secci�n
        dialogoPersonaje.gameObject.SetActive(false);
        ofertantes.gameObject.SetActive(false);
        demandantes.gameObject.SetActive(false);
        decisionesBancarias_opcion1.gameObject.SetActive(true);
        decisionesBancarias_opcion2.gameObject.SetActive(true);
        decisionesBancarias_opcion3.gameObject.SetActive(true);
        informacionPersonaje.gameObject.SetActive(true);

        // Actualiza las opciones disponibles
        decisionesBancarias_opcion1.GetComponentInChildren<TMP_Text>().text = nivel.opciones[0];
        decisionesBancarias_opcion2.GetComponentInChildren<TMP_Text>().text = nivel.opciones[1];
        decisionesBancarias_opcion3.GetComponentInChildren<TMP_Text>().text = nivel.opciones[2];
        informacionPersonaje.text = nivel.informacionPersonaje;
    }

    // Maneja la decisi�n tomada por el jugador
    void TomarDecision(int opcionTomada)
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Verifica si la decisi�n es correcta
        if (opcionTomada == nivel.opcionCorrecta)
        {
            // Incrementa el puntaje
            puntaje += puntosAFavor;
        }
        else
        {
            // Decrementa el puntaje
            puntaje += puntosEnContra;
        }

        // Actualiza el texto de puntos
        puntos.text = "Puntaje: " + puntaje;

        // Avanza al siguiente nivel
        nivelActual++;

        if (nivelActual < niveles.Count)
        {
            // Si hay m�s niveles, actualiza al siguiente nivel
            ActualizarNivel();
        }
        else
        {
            // Si no hay m�s niveles, el juego termina
            panel.SetActive(true);
            panel.GetComponentsInChildren<TMP_Text>()[2].text = "Puntaje final: " + puntaje;
            dialogoPersonaje.gameObject.SetActive(false);
            decisionesBancarias_opcion1.gameObject.SetActive(false);
            decisionesBancarias_opcion2.gameObject.SetActive(false);
            decisionesBancarias_opcion3.gameObject.SetActive(false);
            informacionPersonaje.gameObject.SetActive(false);
        }
    }
}