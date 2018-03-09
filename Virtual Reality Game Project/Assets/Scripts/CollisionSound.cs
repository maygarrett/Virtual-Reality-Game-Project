using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {

    private AudioSource _audioSource;
    private AudioClip _audioClip;

	// Use this for initialization
	void Start () {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioClip = _audioSource.clip;
	}


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("attempting to play sound on " + gameObject.name);
        _audioSource.Play();
    }
}
