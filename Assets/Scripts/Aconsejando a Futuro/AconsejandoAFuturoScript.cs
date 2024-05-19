using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NivelAconsejandoAFuturo
{
    public string dialogo;
    public string informacionPersonaje;
    public List<string> opciones;
    public int opcionCorrecta;
    public Sprite personaje;
    public int necesitaSegurosOPension;
}

public class AconsejandoAFuturoScript : MonoBehaviour
{
    // Referencias a los elementos de la interfaz

    [SerializeField] private Button aconsejandoAFuturo_opcion1;
    [SerializeField] private Button aconsejandoAFuturo_opcion2;
    [SerializeField] private Button aconsejandoAFuturo_opcion3;
    [SerializeField] private Image personajeUI;
    [SerializeField] private TMP_Text puntos;
    [SerializeField] private TMP_Text informacionPersonaje;
    [SerializeField] private Image dialogoPersonaje;
    [SerializeField] private List<Sprite> personajes;
    [SerializeField] private GameObject panel;
    // Variables del juego
    private int nivelActual = 0;
    private int puntaje = 0;


    // Variables de configuración
    private int puntosAFavor = 100;
    private int puntosEnContra = -50;
    private bool habilitarTematicaPensiones = true; // Variable para filtrar Los Niveles para que solo sean sobre pensiones
    private bool habilitarTematicaSeguros = true; // Variable para filtrar Los Niveles para que solo sean sobre seguros 
    private int cantidadNiveles = 5; // Variable para elegir la cantidad de niveles del juego
    private int puntajeAprobatorio;
    Usuario usuarioActual;

    // Referencias a los clips de audio
    [SerializeField] private AudioSource soundEffectSource; // Asigna un AudioSource en el inspector
    [SerializeField] private AudioClip correctAnswerSound;
    [SerializeField] private AudioClip wrongAnswerSound;
    [SerializeField] private AudioClip endGameSound;


    // Información de los niveles
    private List<NivelAconsejandoAFuturo> niveles = new List<NivelAconsejandoAFuturo>();
    [SerializeField] GameManager gameManager;

    // Inicialización
    void Start()
    {
        //Obtiene las configuraciones del juego
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        Juego4Configuraciones configuraciones = SaveSystem.GetConfiguracionesJuego4PorSalon(codSalon);

        habilitarTematicaPensiones = configuraciones.habilitarPensiones;
        habilitarTematicaSeguros = configuraciones.habilitarSeguros;
        puntosAFavor = configuraciones.puntosRespuestaCorrecta;
        puntosEnContra = configuraciones.puntosRespuestaIncorrecta;
        cantidadNiveles = configuraciones.cantidadDePreguntas;
        puntajeAprobatorio = configuraciones.puntajeAprobatorio;

        Debug.Log(codSalon + " " + configuraciones.habilitarPensiones + " " + configuraciones.habilitarSeguros + " " + configuraciones.puntosRespuestaCorrecta + " " + configuraciones.puntosRespuestaIncorrecta + " " + configuraciones.puntajeAprobatorio + " " + configuraciones.habilitarPuntosEnContra + " " + configuraciones.cantidadDePreguntas);
        // Inicializa la interfaz
        panel.SetActive(false); // Oculta el panel de fin de juego

        // Inicializa niveles
        InicializarNiveles(habilitarTematicaPensiones,habilitarTematicaSeguros);

        // Inicializa el juego
        puntos.text = "Puntaje: 0";
        ActualizarNivel();


        aconsejandoAFuturo_opcion1.onClick.AddListener(() => TomarDecision(0));
        aconsejandoAFuturo_opcion2.onClick.AddListener(() => TomarDecision(1));
        aconsejandoAFuturo_opcion3.onClick.AddListener(() => TomarDecision(2));
    }

    // Inicializa la lista de niveles

    // Ejemplo de inicialización de niveles
    void InicializarNiveles(bool habilitarPensiones, bool habilitarSeguros)
    {
        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "¡Hola! Soy María, una trabajadora que está buscando opciones para planificar mi jubilación.",
            informacionPersonaje = "María ha estado trabajando durante 20 años en el sector financiero y está pensando en su futuro financiero.\n" +
                                    "Está interesada en opciones de inversión seguras y estables para su jubilación.",
            opciones = new List<string> { "Plan de Pensiones Privado:\n\n Explorar opciones de planificación de jubilación a través de un plan de pensiones privado con beneficios fiscales y rendimientos garantizados.",
                                      "Fondo de Inversión en Seguros:\n\n Considerar la posibilidad de invertir en un fondo de inversión en seguros que ofrezca protección y crecimiento a largo plazo.",
                                      "Declinar oferta:\n\n No continuar con la planificación de jubilación en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[0],
            necesitaSegurosOPension = 1 // 1 para plan de pensiones, 0 para seguro de inversión
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "Hola, soy Juan y estoy interesado en asegurar mi negocio contra posibles riesgos.",
            informacionPersonaje = "Juan acaba de abrir un pequeño negocio de comida y está preocupado por los riesgos financieros que puede enfrentar.\n" +
                                    "Busca opciones de seguros que le brinden protección y tranquilidad para su negocio.",
            opciones = new List<string> { "Seguro de Responsabilidad Civil para Empresas:\n\n Protege tu negocio contra reclamaciones por daños a terceros o lesiones en el lugar de trabajo.",
                                      "Seguro de Daños a la Propiedad:\n\n Asegura tus activos comerciales, como equipo y edificios, contra daños por incendio, robo u otros desastres.",
                                      "Declinar oferta:\n\n No continuar con la contratación de seguros en este momento." },
            opcionCorrecta = 1, // La opción correcta es la opción 2 (índice 1)
            personaje = personajes[1],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "¡Hola! Soy Luis y estoy buscando opciones de inversión en seguros para proteger a mi familia.",
            informacionPersonaje = "Luis es un padre de familia preocupado por el futuro financiero de sus seres queridos.\n" +
                                    "Está interesado en opciones de seguros que brinden seguridad financiera a su familia en caso de imprevistos.",
            opciones = new List<string> { "Seguro de Vida:\n\n Protege a tu familia con un seguro de vida que brinde beneficios en caso de fallecimiento o invalidez.",
                                      "Seguro de Salud Familiar:\n\n Garantiza el acceso a atención médica de calidad para tu familia con un seguro de salud integral.",
                                      "Declinar oferta:\n\n No continuar con la contratación de seguros en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[2],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "¡Hola! Soy Pedro, represento a una compañía de seguros y estoy buscando oportunidades para ofrecer protección financiera a empresas.",
            informacionPersonaje = "Pedro es un agente de seguros con experiencia en la industria y está interesado en ayudar a empresas a protegerse contra riesgos financieros.\n" +
                                    "Su compañía ofrece una variedad de productos de seguros diseñados para empresas de todos los tamaños.",
            opciones = new List<string> { "Ofrecer Seguro de Responsabilidad Civil:\n\n Presentar opciones de seguro de responsabilidad civil para proteger a las empresas contra reclamaciones legales y daños a terceros.",
                                      "Explorar Seguro de Daños Patrimoniales:\n\n Discutir opciones de seguro para proteger los activos comerciales de las empresas contra pérdidas financieras.",
                                      "Declinar oferta:\n\n No continuar con la contratación de seguros en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[3],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "¡Hola! Soy Marta y estoy buscando opciones de inversión para mi negocio en crecimiento.",
            informacionPersonaje = "Marta es la propietaria de una empresa emergente y está considerando diversas opciones de inversión para expandir su negocio.\n" +
                                "Busca asesoramiento sobre inversiones que puedan proporcionar un retorno significativo y ayudar a alcanzar sus metas comerciales.",
            opciones = new List<string> { "Invertir en Fondos Mutuos:\n\n Explorar la opción de invertir en fondos mutuos para diversificar su cartera y obtener rendimientos potencialmente altos.",
                                  "Adquirir Acciones en Bolsa:\n\n Considerar la compra de acciones en empresas en crecimiento que puedan ofrecer un crecimiento sustancial en el valor de las inversiones.",
                                  "Declinar oferta:\n\n No realizar ninguna inversión en este momento." },
            opcionCorrecta = 0, // La opción correcta es la opción 1 (índice 0)
            personaje = personajes[4],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        // Agrega más niveles aquí

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "Hola, soy Juan y estoy interesado en asegurar mi automóvil contra posibles riesgos.",
            informacionPersonaje = "Juan acaba de adquirir un automóvil nuevo y quiere protegerlo ante cualquier imprevisto.\n" +
                                    "Está buscando opciones de seguros que brinden cobertura adecuada y a un precio razonable.",
            opciones = new List<string> { "Seguro de Responsabilidad Civil para Vehículos:\n\n Protege tu automóvil contra daños a terceros en caso de accidente.",
                                  "Seguro de Cobertura Total:\n\n Ofrece una protección completa para tu automóvil, cubriendo tanto daños a terceros como daños propios.",
                                  "Declinar oferta:\n\n No contratar ningún seguro en este momento." },
            opcionCorrecta = 1, // La opción correcta es la opción 2 (índice 1)
            personaje = personajes[5],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });


        // Filtra los niveles según las temáticas habilitadas
        if (habilitarPensiones && !habilitarSeguros)
        {
            niveles.RemoveAll(n => n.necesitaSegurosOPension == 0);
        }
        else if (!habilitarPensiones && habilitarSeguros)
        {
            niveles.RemoveAll(n => n.necesitaSegurosOPension == 1);
        }
    }




    // Actualiza la información del nivel actual
    void ActualizarNivel()
    {
        // Obtiene el nivel actual
        NivelAconsejandoAFuturo nivel = niveles[nivelActual];

        // Actualiza los textos y el personaje
        dialogoPersonaje.GetComponentInChildren<TMP_Text>().text = nivel.dialogo;
        informacionPersonaje.text = nivel.informacionPersonaje;
        personajeUI.sprite = nivel.personaje;
        dialogoPersonaje.gameObject.SetActive(true);

        // Muestra y oculta elementos según la sección actual
        // Actualiza las opciones disponibles
        aconsejandoAFuturo_opcion1.GetComponentInChildren<TMP_Text>().text = nivel.opciones[0];
        aconsejandoAFuturo_opcion2.GetComponentInChildren<TMP_Text>().text = nivel.opciones[1];
        aconsejandoAFuturo_opcion3.GetComponentInChildren<TMP_Text>().text = nivel.opciones[2];
        informacionPersonaje.text = nivel.informacionPersonaje;

        aconsejandoAFuturo_opcion1.gameObject.SetActive(true);
        aconsejandoAFuturo_opcion2.gameObject.SetActive(true);
        aconsejandoAFuturo_opcion3.gameObject.SetActive(true);
        informacionPersonaje.gameObject.SetActive(true);

    }



    // Maneja la decisión tomada por el jugador en la sección de decisiones de la segunda etapa
    void TomarDecision(int opcionTomada)
    {
        // Obtiene el nivel actual
        NivelAconsejandoAFuturo nivel = niveles[nivelActual];

        // Verifica si la decisión es correcta
        if (opcionTomada == nivel.opcionCorrecta)
        {
            // Incrementa el puntaje
            puntaje += puntosAFavor;

            // Reproduce el sonido de respuesta correcta
            if (correctAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = correctAnswerSound;
                soundEffectSource.Play();
            }
        }
        else
        {
            // Decrementa el puntaje
            puntaje -= puntosEnContra;

            // Reproduce el sonido de respuesta incorrecta
            if (wrongAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = wrongAnswerSound;
                soundEffectSource.Play();
            }
        }

        // Actualiza el texto de puntos
        puntos.text = "Puntaje: " + puntaje;

        // Avanza al siguiente nivel o finaliza el juego
        // Código omitido para brevedad...

        // Muestra el panel de fin de juego y reproduce el sonido de fin de juego
        if (panel != null && endGameSound != null && soundEffectSource != null)
        {
            panel.SetActive(true);
            panel.GetComponentsInChildren<TMP_Text>()[2].text = "Puntaje final: " + puntaje;
            dialogoPersonaje.gameObject.SetActive(false);
            aconsejandoAFuturo_opcion1.gameObject.SetActive(false);
            aconsejandoAFuturo_opcion2.gameObject.SetActive(false);
            aconsejandoAFuturo_opcion3.gameObject.SetActive(false);
            informacionPersonaje.gameObject.SetActive(false);

            // Reproduce el sonido de fin de juego
            soundEffectSource.clip = endGameSound;
            soundEffectSource.Play();
        }
    }
}
