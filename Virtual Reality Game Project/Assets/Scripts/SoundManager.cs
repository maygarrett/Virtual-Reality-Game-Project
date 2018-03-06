using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{

    public static SoundManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    // music clips
    public static AudioClip g_MenuMusic;
    public static AudioClip g_MountainsMusic;

    // sound effects
    

    //Awake is always called before any Start functions
    void Awake()
    {
        //Setting up Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioSource source, AudioClip clip)
    {
        AudioSource.Play(clip);
    }

}