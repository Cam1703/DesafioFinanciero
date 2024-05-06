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

    // Información de los niveles
    private List<Nivel> niveles = new List<Nivel>();

    // Inicialización
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
        // Ejemplo de inicialización de niveles
        niveles.Add(new Nivel
        {
            dialogo = "Hola, soy María y necesito dinero para hacer crecer mi negocio.",
            informacionPersonaje = "Historial crediticio sólido y necesidad de financiamiento para expansión.",
            opciones = new List<string> { "Prestamo1", "Prestamo2", "Prestamo3" },
            opcionCorrecta = 1, // La opción correcta es la opción 2 (índice 1)
            personaje = personajes[0],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "Soy Juan, necesito un préstamo para comprar una casa.",
            informacionPersonaje = "Estabilidad laboral y buenos ingresos mensuales.",
            opciones = new List<string> { "Prestamo Hipotecario", "Prestamo Personal", "Prestamo de Consumo" },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[1],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        // Continúa agregando más niveles según sea necesario.
    }

    // Actualiza la información del nivel actual
    void ActualizarNivel()
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Actualiza los textos y el personaje
        dialogoPersonaje.GetComponentInChildren<TMP_Text>().text = nivel.dialogo;
        informacionPersonaje.text = nivel.informacionPersonaje;
        personajeUI.sprite = nivel.personaje;

        // Muestra y oculta elementos según la sección actual
        dialogoPersonaje.gameObject.SetActive(true);
        ofertantes.gameObject.SetActive(true);
        demandantes.gameObject.SetActive(true);
        decisionesBancarias_opcion1.gameObject.SetActive(false);
        decisionesBancarias_opcion2.gameObject.SetActive(false);
        decisionesBancarias_opcion3.gameObject.SetActive(false);
        informacionPersonaje.gameObject.SetActive(false);
    }

    // Selecciona una opción de ofertantes o demandantes
    void SeleccionarOpcionOfertantesODemandantes(int opcionSeleccionada)
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Verifica si la elección es correcta
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

        // Cambia a la siguiente sección
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

    // Maneja la decisión tomada por el jugador
    void TomarDecision(int opcionTomada)
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Verifica si la decisión es correcta
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
            // Si hay más niveles, actualiza al siguiente nivel
            ActualizarNivel();
        }
        else
        {
            // Si no hay más niveles, el juego termina
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