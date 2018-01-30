using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {

    [SerializeField] private GameObject _door;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Key")
        {
            _door.gameObject.GetComponent<Door>().OpenDoor();
            Destroy(c.gameObject);
            Destroy(gameObject);
        }
    }
}
