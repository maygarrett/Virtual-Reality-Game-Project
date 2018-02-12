using UnityEngine;
using System.Collections;

public class LevelChanger : MonoBehaviour
{

    ScreenFader fadeScr;

    // the scene to fade to
    // 0 = mountains (game scene)
    // 1 = main menu
   [SerializeField] private int SceneNumb = 1;

    private bool _isDone = false;

    void Awake()
    {
        fadeScr = GameObject.FindObjectOfType<ScreenFader>();
    }

    void OnTriggerStay(Collider col)
    {
    
        if (!_isDone)
        {
            if (col.gameObject.tag == "Player")
            {
                Debug.Log("Player Colliding with level changer");
                fadeScr.EndScene(SceneNumb);
                _isDone = true;
            }
        }
    }
}