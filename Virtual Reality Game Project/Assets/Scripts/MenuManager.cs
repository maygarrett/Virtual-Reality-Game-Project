using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] private Canvas _MenuCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _MenuCanvas.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SoundManager.instance.PlayMusic(SoundManager.instance._mountainsMusic);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        SoundManager.instance.PlayMusic(SoundManager.instance._menuMusic);
    }
}
