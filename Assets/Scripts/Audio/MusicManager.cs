using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioClip initialMusicClip;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            initialMusicClip = audioSource.clip;  // Guardar la música inicial
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayInitialMusic()
    {
        if (audioSource.clip != initialMusicClip)
        {
            audioSource.clip = initialMusicClip;
            audioSource.Play();
        }

    }
}
