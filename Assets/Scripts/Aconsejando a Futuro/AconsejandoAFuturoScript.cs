using System;
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


    // Variables de configuraci�n
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


    // Informaci�n de los niveles
    private List<NivelAconsejandoAFuturo> niveles = new List<NivelAconsejandoAFuturo>();
    [SerializeField] GameManager gameManager;

    // Inicializaci�n
    void Start()
    {
        //Obtiene las configuraciones del juego
        usuarioActual = gameManager.GetUsuarioActual();
        string codSalon = usuarioActual.codigoDeClase;
        // Cacheado en memoria al iniciar sesión (MenuInicio); no hace falta otra llamada a Firestore.
        Juego4Configuraciones configuraciones = gameManager.GetSalonActual().juego4Configuraciones;

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

    // Ejemplo de inicializaci�n de niveles
    void InicializarNiveles(bool habilitarPensiones, bool habilitarSeguros)
    {
        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "�Hola! Soy Mar�a, una trabajadora que est� buscando opciones para planificar mi jubilaci�n.",
            informacionPersonaje = "Mar�a ha estado trabajando durante 20 a�os en el sector financiero y est� pensando en su futuro financiero.\n" +
                                    "Est� interesada en opciones de inversi�n seguras y estables para su jubilaci�n.",
            opciones = new List<string> { "Plan de Pensiones Privado:\n\n Explorar opciones de planificaci�n de jubilaci�n a trav�s de un plan de pensiones privado con beneficios fiscales y rendimientos garantizados.",
                                      "Fondo de Inversi�n en Seguros:\n\n Considerar la posibilidad de invertir en un fondo de inversi�n en seguros que ofrezca protecci�n y crecimiento a largo plazo.",
                                      "Declinar oferta:\n\n No continuar con la planificaci�n de jubilaci�n en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[0],
            necesitaSegurosOPension = 1 // 1 para plan de pensiones, 0 para seguro de inversi�n
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "Hola, soy Juan y estoy interesado en asegurar mi negocio contra posibles riesgos.",
            informacionPersonaje = "Juan acaba de abrir un peque�o negocio de comida y est� preocupado por los riesgos financieros que puede enfrentar.\n" +
                                    "Busca opciones de seguros que le brinden protecci�n y tranquilidad para su negocio.",
            opciones = new List<string> { "Seguro de Responsabilidad Civil para Empresas:\n\n Protege tu negocio contra reclamaciones por da�os a terceros o lesiones en el lugar de trabajo.",
                                      "Seguro de Da�os a la Propiedad:\n\n Asegura tus activos comerciales, como equipo y edificios, contra da�os por incendio, robo u otros desastres.",
                                      "Declinar oferta:\n\n No continuar con la contrataci�n de seguros en este momento." },
            opcionCorrecta = 1, // La opci�n correcta es la opci�n 2 (�ndice 1)
            personaje = personajes[1],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "�Hola! Soy Luis y estoy buscando opciones de inversi�n en seguros para proteger a mi familia.",
            informacionPersonaje = "Luis es un padre de familia preocupado por el futuro financiero de sus seres queridos.\n" +
                                    "Est� interesado en opciones de seguros que brinden seguridad financiera a su familia en caso de imprevistos.",
            opciones = new List<string> { "Seguro de Vida:\n\n Protege a tu familia con un seguro de vida que brinde beneficios en caso de fallecimiento o invalidez.",
                                      "Seguro de Salud Familiar:\n\n Garantiza el acceso a atenci�n m�dica de calidad para tu familia con un seguro de salud integral.",
                                      "Declinar oferta:\n\n No continuar con la contrataci�n de seguros en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[2],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "�Hola! Soy Pedro, represento a una compa��a de seguros y estoy buscando oportunidades para ofrecer protecci�n financiera a empresas.",
            informacionPersonaje = "Pedro es un agente de seguros con experiencia en la industria y est� interesado en ayudar a empresas a protegerse contra riesgos financieros.\n" +
                                    "Su compa��a ofrece una variedad de productos de seguros dise�ados para empresas de todos los tama�os.",
            opciones = new List<string> { "Ofrecer Seguro de Responsabilidad Civil:\n\n Presentar opciones de seguro de responsabilidad civil para proteger a las empresas contra reclamaciones legales y da�os a terceros.",
                                      "Explorar Seguro de Da�os Patrimoniales:\n\n Discutir opciones de seguro para proteger los activos comerciales de las empresas contra p�rdidas financieras.",
                                      "Declinar oferta:\n\n No continuar con la contrataci�n de seguros en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[3],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "�Hola! Soy Marta y estoy buscando opciones de inversi�n para mi negocio en crecimiento.",
            informacionPersonaje = "Marta es la propietaria de una empresa emergente y est� considerando diversas opciones de inversi�n para expandir su negocio.\n" +
                                "Busca asesoramiento sobre inversiones que puedan proporcionar un retorno significativo y ayudar a alcanzar sus metas comerciales.",
            opciones = new List<string> { "Invertir en Fondos Mutuos:\n\n Explorar la opci�n de invertir en fondos mutuos para diversificar su cartera y obtener rendimientos potencialmente altos.",
                                  "Adquirir Acciones en Bolsa:\n\n Considerar la compra de acciones en empresas en crecimiento que puedan ofrecer un crecimiento sustancial en el valor de las inversiones.",
                                  "Declinar oferta:\n\n No realizar ninguna inversi�n en este momento." },
            opcionCorrecta = 0, // La opci�n correcta es la opci�n 1 (�ndice 0)
            personaje = personajes[4],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });

        // Agrega m�s niveles aqu�

        niveles.Add(new NivelAconsejandoAFuturo
        {
            dialogo = "Hola, soy Juan y estoy interesado en asegurar mi autom�vil contra posibles riesgos.",
            informacionPersonaje = "Juan acaba de adquirir un autom�vil nuevo y quiere protegerlo ante cualquier imprevisto.\n" +
                                    "Est� buscando opciones de seguros que brinden cobertura adecuada y a un precio razonable.",
            opciones = new List<string> { "Seguro de Responsabilidad Civil para Veh�culos:\n\n Protege tu autom�vil contra da�os a terceros en caso de accidente.",
                                  "Seguro de Cobertura Total:\n\n Ofrece una protecci�n completa para tu autom�vil, cubriendo tanto da�os a terceros como da�os propios.",
                                  "Declinar oferta:\n\n No contratar ning�n seguro en este momento." },
            opcionCorrecta = 1, // La opci�n correcta es la opci�n 2 (�ndice 1)
            personaje = personajes[5],
            necesitaSegurosOPension = 0 // 1 para seguro, 0 para plan de pensiones
        });


        // Filtra los niveles seg�n las tem�ticas habilitadas
        if (habilitarPensiones && !habilitarSeguros)
        {
            niveles.RemoveAll(n => n.necesitaSegurosOPension == 0);
        }
        else if (!habilitarPensiones && habilitarSeguros)
        {
            niveles.RemoveAll(n => n.necesitaSegurosOPension == 1);
        }
    }




    // Actualiza la informaci�n del nivel actual
    void ActualizarNivel()
    {
        // Obtiene el nivel actual
        NivelAconsejandoAFuturo nivel = niveles[nivelActual];

        // Actualiza los textos y el personaje
        dialogoPersonaje.GetComponentInChildren<TMP_Text>().text = nivel.dialogo;
        informacionPersonaje.text = nivel.informacionPersonaje;
        personajeUI.sprite = nivel.personaje;
        dialogoPersonaje.gameObject.SetActive(true);

        // Muestra y oculta elementos seg�n la secci�n actual
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



    // Maneja la decisi�n tomada por el jugador en la secci�n de decisiones de la segunda etapa
    void TomarDecision(int opcionTomada)
    {
        // Obtiene el nivel actual
        NivelAconsejandoAFuturo nivel = niveles[nivelActual];

        // Verifica si la decisi�n es correcta
        if (opcionTomada == nivel.opcionCorrecta)
        {

            // Reproduce el sonido de respuesta correcta
            if (correctAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = correctAnswerSound;
                soundEffectSource.Play();
            }
            // Incrementa el puntaje
            puntaje += puntosAFavor;
        }
        else
        {
            // Reproduce el sonido de respuesta incorrecta
            if (wrongAnswerSound != null && soundEffectSource != null)
            {
                soundEffectSource.clip = wrongAnswerSound;
                soundEffectSource.Play();
            }
            // Decrementa el puntaje
            puntaje -= puntosEnContra;
        }

        // Actualiza el texto de puntos
        puntos.text = "Puntaje: " + puntaje;

        // Avanza al siguiente nivel o finaliza el juego
        nivelActual++;
        if (nivelActual < cantidadNiveles)
        {
            ActualizarNivel();
        }
        else
        {
            // Finaliza el juego
            FinalizarJuego();
        }
    }

    // Finaliza el juego y muestra la pantalla de fin de juego
    private void FinalizarJuego()
    {

        // Muestra el panel de fin de juego y reproduce el sonido de fin de juego
        if (panel != null && endGameSound != null && soundEffectSource != null)
        {
            soundEffectSource.Play();
            panel.SetActive(true);
            panel.GetComponentsInChildren<TMP_Text>()[2].text = "Puntaje final: " + puntaje;
            dialogoPersonaje.gameObject.SetActive(false);
            aconsejandoAFuturo_opcion1.gameObject.SetActive(false);
            aconsejandoAFuturo_opcion2.gameObject.SetActive(false);
            aconsejandoAFuturo_opcion3.gameObject.SetActive(false);
            informacionPersonaje.gameObject.SetActive(false);

            // Reproduce el sonido de fin de juego
            soundEffectSource.clip = endGameSound;

        }
        // Guarda el puntaje en el usuario actual
        if (usuarioActual.puntajesMaximos.puntajeMaximoJuego4 < puntaje)
        {
            usuarioActual.puntajesMaximos.puntajeMaximoJuego4 = puntaje;
        }
        if (puntajeAprobatorio <= puntaje)
        {
            usuarioActual.puntajesMaximos.juego4Aprobado = true;
        }

        GuardarProgreso(usuarioActual);
    }

    private async void GuardarProgreso(Usuario usuario)
    {
        try
        {
            await SaveSystem.ModifyUserAsync(usuario);
        }
        catch (Exception e)
        {
            Debug.LogError("No se pudo guardar el progreso de Aconsejando a Futuro: " + e);
        }
    }
}
