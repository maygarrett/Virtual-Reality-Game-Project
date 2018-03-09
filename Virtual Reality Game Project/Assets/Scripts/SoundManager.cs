using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{

    public static SoundManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    // scenes references for music
    private Scene _mainMenuScene;
    private Scene _mountainsScene;

    // music clips
    [SerializeField] public AudioClip _menuMusic;
    [SerializeField] public AudioClip _mountainsMusic;

    // sound effects
    [SerializeField] private AudioClip _boxSound;
    [SerializeField] private AudioClip _ballBounce;
    [SerializeField] private AudioClip _keyJingle;
    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private AudioClip _gunRichochet;
    [SerializeField] private AudioClip _brickSound;
    [SerializeField] private AudioClip _doorOpening;


    // audiosources
    [SerializeField] private AudioSource _musicSource;

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

    private void Start()
    {
        // getting references
        _mainMenuScene = SceneManager.GetSceneByBuildIndex(0);
        _mountainsScene = SceneManager.GetSceneByBuildIndex(1);

        // setting music to start
        if (SceneManager.GetActiveScene() == _mainMenuScene)
        {
            PlayMusic(_menuMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }

}