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

    [SerializeField] private bool habilitarPrimeraEtapa = true; // Variable para deshabilitar la primera etapa del juego
    [SerializeField] private bool habilitarSegundaEtapa = true; // Variable para deshabilitar la segunda etapa del juego
    [SerializeField] private int cantidadNiveles = 2; // Variable para elegir la cantidad de niveles del juego

    // Información de los niveles
    private List<Nivel> niveles = new List<Nivel>();

    // Inicialización
    void Start()
    {
        panel.SetActive(false); // Oculta el panel de fin de juego

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
            dialogo = "¡Hola! Soy María, propietaria de una tienda de tecnología. Estoy buscando financiamiento para expandir mi inventario y ofrecer productos más variados.",
            informacionPersonaje = "Posee una tienda de tecnología desde hace 2 años\n" +
                                    "Su negocio ha demostrado ser estable y rentable, a pesar de que no obtiene muchos ingresos.\n" +
                                    "En total posee 2 empleados que se encargan de vender los productos.\n" +
                                    "Historial crediticio sólido: María ha mantenido un historial crediticio positivo con pagos a tiempo y sin incumplimientos.",
            opciones = new List<string> { "Préstamo Comercial a Largo Plazo:\n\n Te ofrece un préstamo de monto amplio con tasas competitivas y plazos flexibles para financiar tu expansión.",
                                        "Línea de Crédito para Pequeñas Empresas:\n\n Accede a una línea de crédito renovable para cubrir tus necesidades de capital de trabajo y expansión.",
                                        "Declinar préstamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 1, // La opción correcta es la opción 2 (índice 1)
            personaje = personajes[0],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "Hola, soy Juan y estoy buscando financiamiento para comprar una franquicia de comida rápida.",
            informacionPersonaje = "Egresado recientemente de la carrera de administración de negocios, busca emprender con su negocio propio\n" +
                                    "No posee un ingreso mensual fijo.\n" +
                                    "Historial crediticio deficiente: Juan ha tenido algunos problemas con su historial crediticio en el pasado, por lo cuál ha acumulado varias deudas al rededor de los años.",
            opciones = new List<string> { "Préstamo Comercial para Franquicias:\n\n Obtén financiamiento específico para la adquisición de una franquicia, con tasas competitivas y plazos flexibles.", "Préstamo Personal para Inversiones:\n\n Accede a un préstamo personal con condiciones favorables para invertir en tu negocio propio.", "Declinar préstamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 2, // La opción correcta es la opción 3 (índice 2)
            personaje = personajes[1],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "¡Hola! Soy Luis, y necesito financiamiento para comprar un nuevo vehículo para mi negocio de transporte.",
            informacionPersonaje = "Luis, un emprendedor en el negocio del transporte, busca financiamiento para expandir su flota y mejorar la eficiencia operativa.\n" +
                                    "Su historial crediticio sólido refleja su responsabilidad financiera y capacidad para gestionar sus obligaciones.",
            opciones = new List<string> { "Préstamo Comercial para Vehículos:\n\n Financia la compra de vehículos comerciales con un préstamo a plazos y tasas competitivas.", "Crédito Revolvente para Empresas:\n\n Accede a una línea de crédito renovable para cubrir tus necesidades de capital de trabajo y expansión.", "Declinar préstamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[2],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });


        niveles.Add(new Nivel
        {
            dialogo = "¡Hola! Me llamo Luis y necesito financiamiento para pagar mis estudios de posgrado en el extranjero.",
            informacionPersonaje = "Historial crediticio limitado: Luis tiene un historial crediticio limitado debido a su condición de estudiante. \n" +
                                    "Aunque puede tener ingresos futuros, su capacidad de endeudamiento puede estar limitada por la falta de historial crediticio establecido.",
            opciones = new List<string> { "Préstamo Educativo Internacional:\n\n Financia tus estudios en el extranjero con un préstamo específico para estudiantes internacionales.",
                                    "Línea de Crédito Estudiantil:\n\n Accede a una línea de crédito renovable para cubrir tus gastos educativos durante tus estudios de posgrado.", 
                                    "Declinar préstamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 2, // La opción correcta es la opción 3 (índice 2)
            personaje = personajes[3],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "¡Hola! Soy María, represento a una institución financiera y estoy buscando oportunidades de inversión en el sector del transporte.",
            informacionPersonaje = "María es una ejecutiva experimentada en el sector financiero, buscando oportunidades de inversión en el sector del transporte. \n" +
                                    "Su institución está interesada en financiar proyectos viables y rentables que contribuyan al crecimiento económico y la innovación.",
            opciones = new List<string> { "Ofrecer financiamiento:\n\n Presentar opciones de financiamiento para proyectos de expansión y adquisición de flotas de vehículos.",
                                        "Explorar inversión en acciones:\n\n Discutir la posibilidad de invertir en acciones de empresas del sector del transporte.", 
                                        "Declinar oferta:\n\n No continuar con la oferta de financiamiento en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[4],
            opcionCorrectaOfertantesODemandantes = 0 // Ofertantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "¡Hola! Soy Marco, un empresario en busca de opciones de inversión para diversificar mi cartera financiera.",
            informacionPersonaje = "Marco es un empresaria exitosa que busca oportunidades de inversión para diversificar su cartera financiera.\n" +
                                    "Está interesada en opciones que ofrezcan un buen retorno de la inversión y un riesgo controlado.",
            opciones = new List<string> { "Invertir en bienes inmobiliarios:\n\n Explorar la adquisición de propiedades inmobiliarias como una forma de inversión estable y con potencial de apreciación a largo plazo.", 
                                        "Inversiones en la bolsa:\n\n Recomendar inversiones de alto retorno y riesgo en la bolsa devalores",
                                        "Declinar oferta:\n\n No continuar con la oferta de financiamiento en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[5],
            opcionCorrectaOfertantesODemandantes = 0 // Demandantes
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
        dialogoPersonaje.gameObject.SetActive(true);

        // Muestra y oculta elementos según la sección actual

        if (habilitarPrimeraEtapa)
        {
            ofertantes.gameObject.SetActive(true);
            demandantes.gameObject.SetActive(true);
            decisionesBancarias_opcion1.gameObject.SetActive(false);
            decisionesBancarias_opcion2.gameObject.SetActive(false);
            decisionesBancarias_opcion3.gameObject.SetActive(false);
            informacionPersonaje.gameObject.SetActive(false);
        }
        else if (!habilitarPrimeraEtapa)
        {
            ofertantes.gameObject.SetActive(false);
            demandantes.gameObject.SetActive(false);
            decisionesBancarias_opcion1.gameObject.SetActive(true);
            decisionesBancarias_opcion2.gameObject.SetActive(true);
            decisionesBancarias_opcion3.gameObject.SetActive(true);
            informacionPersonaje.gameObject.SetActive(true);
        }



    }

    // Selecciona una opción de ofertantes o demandantes
    void SeleccionarOpcionOfertantesODemandantes(int opcionSeleccionada) // 0 para ofertantes, 1 para demandantes
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


        if (habilitarPrimeraEtapa && !habilitarSegundaEtapa)
        {
            // Avanza al siguiente nivel
            nivelActual++;

            if (nivelActual < cantidadNiveles)
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
        else
        {
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
        
    }

    // Maneja la decisión tomada por el jugador en la sección de decisiones de la segunda etapa
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

        if (nivelActual < cantidadNiveles)
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