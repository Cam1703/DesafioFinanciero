using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    public static Usuario usuarioActual;
    public static Salon salonActual;

    public AudioClip buttonSound; // Asignar el sonido desde el Inspector

    private AudioSource audioSource;

    public static GameManager instance;

    void Awake()
    {
        SaveSystem.Init();
    }

    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayInitialMusic();
        }

        // Inicializar botones en la escena actual
        InitializeButtons();
    }


    public void CambiarEscena(string nombreEscena)
    {
        Debug.Log("Cambiando a escena: " + nombreEscena);
        //Destruir SceneMusicManager si existe
        SceneMusicManager sceneMusicManager = FindObjectOfType<SceneMusicManager>();
        if (sceneMusicManager != null)
        {
            Destroy(sceneMusicManager.gameObject);
        }
        SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }

    public void SetUsuarioActual(Usuario usuario)
    {
        usuarioActual = new Usuario(usuario);
        Debug.Log("Usuario actual: " + usuarioActual);
    }

    public Usuario GetUsuarioActual()
    {
        return usuarioActual;
    }

    public void SetSalonActual(Salon salon)
    {
        salonActual = new Salon(salon);
        Debug.Log("Salon actual: " + salonActual);
    }

    public Salon GetSalonActual()
    {
        return salonActual;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Inicializar botones en la nueva escena cargada
        InitializeButtons();
    }

    public void InitializeButtons()
    {
        // Encontrar todos los botones en la escena
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            AddEventTriggerListener(button);
        }
    }

    private void AddEventTriggerListener(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        entry.callback.AddListener((eventData) => { PlayButtonSound(); });
        trigger.triggers.Add(entry);
    }

    public void PlayButtonSound()
    {
        // Reproducir el sonido cuando se llame a este método
        if (audioSource != null && buttonSound != null)
        {
            audioSource.PlayOneShot(buttonSound);
        }
    }
}
