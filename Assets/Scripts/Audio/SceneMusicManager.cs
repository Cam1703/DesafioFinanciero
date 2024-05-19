using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicManager : MonoBehaviour
{
    public AudioClip minigameMusicClip;  // Arrastra aquí el clip de música del minijuego desde el inspector

    private AudioSource musicManagerAudioSource;

    void Start()
    {
        // Encuentra el MusicManager y cambia la música
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManagerAudioSource = musicManager.GetComponent<AudioSource>();
            musicManagerAudioSource.clip = minigameMusicClip;
            musicManagerAudioSource.Play();
        }
    }

}
