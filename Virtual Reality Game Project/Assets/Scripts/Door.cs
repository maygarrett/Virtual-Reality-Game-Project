using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    // door rotation variables
    private Vector3 _rotationAxis = new Vector3(0, 1, 0);
    private float _rotationAngle = -4.0f;
    [SerializeField] private Transform _rotationPoint;

    private bool _isOpen;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        // just for testing
        /*if (!_isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OpenDoor();
            }
        }*/
    }

    public void OpenDoor()
    {
        int tempCounter = 0;
        while (tempCounter < (30 * 100))
        {
            transform.RotateAround(_rotationPoint.position, _rotationAxis, _rotationAngle / 100);
            tempCounter++;
        }
        _isOpen = true;
    }
}
