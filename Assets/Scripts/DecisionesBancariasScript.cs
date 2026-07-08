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


    // Variables de configuraci�n
    private int puntosAFavor = 100;
    private int puntosEnContra = -50;
    private bool habilitarPrimeraEtapa = true; // Variable para deshabilitar la primera etapa del juego
    private bool habilitarSegundaEtapa = true; // Variable para deshabilitar la segunda etapa del juego
    private int cantidadNiveles = 6; // Variable para elegir la cantidad de niveles del juego
    private int puntajeAprobatorio;
    Usuario usuarioActual;

    // Informaci�n de los niveles
    private List<Nivel> niveles = new List<Nivel>();
    [SerializeField] GameManager gameManager;

    [SerializeField] private AudioSource soundEffectSource; // Asigna un AudioSource en el inspector
    [SerializeField] private AudioClip correctAnswerSound;
    [SerializeField] private AudioClip wrongAnswerSound;

    // Inicializaci�n
    void Start()
    {
        //Obtiene las configuraciones del juego
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        // El salón (y su configuración) ya se cacheó en memoria al iniciar sesión (MenuInicio),
        // así que se lee directo de ahí en vez de volver a consultar Firestore.
        Juego1Configuraciones configuraciones = gameManager.GetSalonActual().juego1Configuraciones;

        habilitarPrimeraEtapa = configuraciones.habilitarSeccionOfertantesYDemandantes;
        habilitarSegundaEtapa = configuraciones.habilitarSeccionRecomendarPlanesFinancieros;
        puntosAFavor = configuraciones.puntosRespuestaCorrecta;
        puntosEnContra = configuraciones.puntosRespuestaIncorrecta;
        cantidadNiveles = configuraciones.cantidadDePreguntas;
        puntajeAprobatorio = configuraciones.puntajeAprobatorio;

        Debug.Log(codSalon + " " + configuraciones.habilitarSeccionOfertantesYDemandantes + " " + configuraciones.habilitarSeccionRecomendarPlanesFinancieros + " " + configuraciones.puntosRespuestaCorrecta + " " + configuraciones.puntosRespuestaIncorrecta + " " + configuraciones.puntajeAprobatorio + " " + configuraciones.habilitarPuntosEnContra + " " + configuraciones.cantidadDePreguntas);
        // Inicializa la interfaz
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
        // Ejemplo de inicializaci�n de niveles
        niveles.Add(new Nivel
        {
            dialogo = "�Hola! Soy Mar�a, propietaria de una tienda de tecnolog�a. Estoy buscando financiamiento para expandir mi inventario y ofrecer productos m�s variados.",
            informacionPersonaje = "Posee una tienda de tecnolog�a desde hace 2 a�os\n" +
                                    "Su negocio ha demostrado ser estable y rentable, a pesar de que no obtiene muchos ingresos.\n" +
                                    "En total posee 2 empleados que se encargan de vender los productos.\n" +
                                    "Historial crediticio s�lido: Mar�a ha mantenido un historial crediticio positivo con pagos a tiempo y sin incumplimientos.",
            opciones = new List<string> { "Pr�stamo Comercial a Largo Plazo:\n\n Te ofrece un pr�stamo de monto amplio con tasas competitivas y plazos flexibles para financiar tu expansi�n.",
                                        "L�nea de Cr�dito para Peque�as Empresas:\n\n Accede a una l�nea de cr�dito renovable para cubrir tus necesidades de capital de trabajo y expansi�n.",
                                        "Declinar pr�stamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 1, // La opci�n correcta es la opci�n 2 (�ndice 1)
            personaje = personajes[0],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "Hola, soy Juan y estoy buscando financiamiento para comprar una franquicia de comida r�pida.",
            informacionPersonaje = "Egresado recientemente de la carrera de administraci�n de negocios, busca emprender con su negocio propio\n" +
                                    "No posee un ingreso mensual fijo.\n" +
                                    "Historial crediticio deficiente: Juan ha tenido algunos problemas con su historial crediticio en el pasado, por lo cu�l ha acumulado varias deudas al rededor de los a�os.",
            opciones = new List<string> { "Pr�stamo Comercial para Franquicias:\n\n Obt�n financiamiento espec�fico para la adquisici�n de una franquicia, con tasas competitivas y plazos flexibles.", "Pr�stamo Personal para Inversiones:\n\n Accede a un pr�stamo personal con condiciones favorables para invertir en tu negocio propio.", "Declinar pr�stamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 2, // La opci�n correcta es la opci�n 3 (�ndice 2)
            personaje = personajes[1],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "�Hola! Soy Mar�a, represento a una instituci�n financiera y estoy buscando oportunidades de inversi�n en el sector del transporte.",
            informacionPersonaje = "Mar�a es una ejecutiva experimentada en el sector financiero, buscando oportunidades de inversi�n en el sector del transporte. \n" +
                                    "Su instituci�n est� interesada en financiar proyectos viables y rentables que contribuyan al crecimiento econ�mico y la innovaci�n.",
            opciones = new List<string> { "Ofrecer financiamiento:\n\n Presentar opciones de financiamiento para proyectos de expansi�n y adquisici�n de flotas de veh�culos.",
                                        "Explorar inversi�n en acciones:\n\n Discutir la posibilidad de invertir en acciones de empresas del sector del transporte.",
                                        "Declinar oferta:\n\n No continuar con la oferta de financiamiento en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[4],
            opcionCorrectaOfertantesODemandantes = 0 // Ofertantes
        });

        niveles.Add(new Nivel
        {
            dialogo = "�Hola! Soy Luis, y necesito financiamiento para comprar un nuevo veh�culo para mi negocio de transporte.",
            informacionPersonaje = "Luis, un emprendedor en el negocio del transporte, busca financiamiento para expandir su flota y mejorar la eficiencia operativa.\n" +
                                    "Su historial crediticio s�lido refleja su responsabilidad financiera y capacidad para gestionar sus obligaciones.",
            opciones = new List<string> { "Pr�stamo Comercial para Veh�culos:\n\n Financia la compra de veh�culos comerciales con un pr�stamo a plazos y tasas competitivas.", "Cr�dito Revolvente para Empresas:\n\n Accede a una l�nea de cr�dito renovable para cubrir tus necesidades de capital de trabajo y expansi�n.", "Declinar pr�stamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[2],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });


        niveles.Add(new Nivel
        {
            dialogo = "�Hola! Me llamo Lucio y necesito financiamiento para pagar mis estudios de posgrado en el extranjero.",
            informacionPersonaje = "Historial crediticio limitado: Lucio tiene un historial crediticio limitado debido a su condici�n de estudiante. \n" +
                                    "Aunque puede tener ingresos futuros, su capacidad de endeudamiento puede estar limitada por la falta de historial crediticio establecido.",
            opciones = new List<string> { "Pr�stamo Educativo Internacional:\n\n Financia tus estudios en el extranjero con un pr�stamo espec�fico para estudiantes internacionales.",
                                    "L�nea de Cr�dito Estudiantil:\n\n Accede a una l�nea de cr�dito renovable para cubrir tus gastos educativos durante tus estudios de posgrado.", 
                                    "Declinar pr�stamo:\n\n No ofrecer financiamiento en este momento." },
            opcionCorrecta = 2, // La opci�n correcta es la opci�n 3 (�ndice 2)
            personaje = personajes[3],
            opcionCorrectaOfertantesODemandantes = 1 // Demandantes
        });

        

        niveles.Add(new Nivel
        {
            dialogo = "�Hola! Soy Marco, un empresario en busca de opciones de inversi�n para diversificar mi cartera financiera.",
            informacionPersonaje = "Marco es un empresario exitoso que busca oportunidades de inversi�n para diversificar su cartera financiera.\n" +
                                    "Est� interesado en opciones que ofrezcan un buen retorno de la inversi�n y un riesgo controlado.",
            opciones = new List<string> { "Invertir en bienes inmobiliarios:\n\n Explorar la adquisici�n de propiedades inmobiliarias como una forma de inversi�n estable y con potencial de apreciaci�n a largo plazo.", 
                                        "Inversiones en la bolsa:\n\n Recomendar inversiones de alto retorno y riesgo en la bolsa devalores",
                                        "Declinar oferta:\n\n No continuar con la oferta de financiamiento en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[5],
            opcionCorrectaOfertantesODemandantes = 0 // Demandantes
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
        dialogoPersonaje.gameObject.SetActive(true);

        // Muestra y oculta elementos seg�n la secci�n actual

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

    // Selecciona una opci�n de ofertantes o demandantes
    void SeleccionarOpcionOfertantesODemandantes(int opcionSeleccionada) // 0 para ofertantes, 1 para demandantes
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Verifica si la elecci�n es correcta
        if (opcionSeleccionada == nivel.opcionCorrectaOfertantesODemandantes)
        {
            // Incrementa el puntaje
            // Reproduce el sonido de respuesta correcta
            if (correctAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = correctAnswerSound;
                soundEffectSource.Play();
            }
            puntaje += puntosAFavor;
        }
        else
        {
            // Decrementa el puntaje
            // Reproduce el sonido de respuesta incorrecta
            if (wrongAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = wrongAnswerSound;
                soundEffectSource.Play();
            }
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
        else
        {
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
        
    }

    // Maneja la decisi�n tomada por el jugador en la secci�n de decisiones de la segunda etapa
    void TomarDecision(int opcionTomada)
    {
        // Obtiene el nivel actual
        Nivel nivel = niveles[nivelActual];

        // Verifica si la decisi�n es correcta
        if (opcionTomada == nivel.opcionCorrecta)
        {
            // Incrementa el puntaje
            // Reproduce el sonido de respuesta correcta
            if (correctAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = correctAnswerSound;
                soundEffectSource.Play();
            }
            puntaje += puntosAFavor;
        }
        else
        {
            // Decrementa el puntaje
            // Reproduce el sonido de respuesta incorrecta
            if (wrongAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = wrongAnswerSound;
                soundEffectSource.Play();
            }
            puntaje += puntosEnContra;
        }

        // Actualiza el texto de puntos
        puntos.text = "Puntaje: " + puntaje;

        // Avanza al siguiente nivel
        nivelActual++;

        if (nivelActual < cantidadNiveles)
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

            // Guarda el puntaje en el usuario actual
            if(usuarioActual.puntajesMaximos.puntajeMaximoJuego1 < puntaje)
            {
                usuarioActual.puntajesMaximos.puntajeMaximoJuego1 = puntaje;
            }
            if (puntajeAprobatorio <= puntaje)
            {
                usuarioActual.puntajesMaximos.juego1Aprobado = true;
            }

            GuardarProgreso(usuarioActual);
        }
    }

    private async void GuardarProgreso(Usuario usuario)
    {
        try
        {
            await SaveSystem.ModifyUserAsync(usuario);
        }
        catch (Exception e)
        {
            Debug.LogError("No se pudo guardar el progreso de Decisiones Bancarias: " + e);
        }
    }
}