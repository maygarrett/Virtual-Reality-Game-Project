using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryRemoval : MonoBehaviour {

    [SerializeField] private GameObject[] _triggers;

    private int _counter;

	// Use this for initialization
	void Start () {
		if(_triggers.Length == 0)
        {
            Debug.LogWarning("No Triggers set on " + this.gameObject.name);
        }
	}

    // Update is called once per frame
    void Update() {

        if (_counter == 20)
        {
            foreach (GameObject trigger in _triggers)
            {
                if(trigger == null)
                {
                    Destroy(this.gameObject);
                }
            }

            _counter = 0;
        }

        _counter++;
	}
}
