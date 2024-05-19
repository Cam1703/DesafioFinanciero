using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    public AudioClip minigameMusicClip;  // Arrastra aqu� el clip de m�sica del minijuego desde el inspector

    private AudioSource musicManagerAudioSource;

    void Start()
    {
        // Encuentra el MusicManager y cambia la m�sica
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManagerAudioSource = musicManager.GetComponent<AudioSource>();
            musicManagerAudioSource.clip = minigameMusicClip;
            musicManagerAudioSource.Play();
        }
    }

}
