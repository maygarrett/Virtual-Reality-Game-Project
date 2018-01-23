using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubstitutePlayerMovement : MonoBehaviour {

    private Transform _subPlayer;

    private float _movementSpeed = 3.0f;
    private float _rotationSpeed = 30.0f;

	// Use this for initialization
	void Start () {
        _subPlayer = gameObject.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.UpArrow))
        {
            _subPlayer.Translate(Vector3.forward * _movementSpeed * 1/30);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            _subPlayer.Rotate(-transform.up * _rotationSpeed * 1/30);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _subPlayer.Rotate(transform.up * _rotationSpeed * 1/30);
        }
    }
}
