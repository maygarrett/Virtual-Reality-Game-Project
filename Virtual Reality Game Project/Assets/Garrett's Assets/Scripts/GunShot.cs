﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour {

    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private GameObject _handgun;

    [SerializeField] private GameObject _gunSparks1;
    [SerializeField] private GameObject _gunSparks2;
    private int _gunSparksCounter = 1;
    private ParticleSystem _gunSparks1System;
    private ParticleSystem _gunSparks2System;

    [SerializeField] private GameObject _explosion;

    private AudioSource _audioSource;



    private Animation _gunAnimation;

    // Use this for initialization
    void Start () {

        _gunAnimation = _handgun.GetComponent<Animation>();

        _gunSparks1System = _gunSparks1.GetComponent<ParticleSystem>();
        _gunSparks2System = _gunSparks2.GetComponent<ParticleSystem>();

        _audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            FireGun();
                
        }

        Debug.DrawLine(_raycastOrigin.position, _raycastOrigin.forward * 100, Color.red);

    }

    public void FireGun()
    {
        _audioSource.Play();

        RaycastHit hit;

        Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out hit, 1000.0f);

        _gunAnimation.Play();

        if (hit.transform.gameObject.tag == "Metal")
        {
            if (_gunSparksCounter == 1)
            {
                _gunSparks1.transform.position = hit.point;
                _gunSparks1System.Play();
                _gunSparksCounter = 2;
            }
            if (_gunSparksCounter == 2)
            {
                _gunSparks2.transform.position = hit.point;
                _gunSparks2System.Play();
                _gunSparksCounter = 1;
            }

            // if object is movable apply force from gun shot
        }

        if(hit.transform.gameObject.tag == "Explosive")
        {
            // instantiate an explosion
            Instantiate(_explosion, hit.transform.gameObject.transform.position, hit.transform.rotation);

            // destroy the hit.gameobject
            Destroy(hit.transform.gameObject);
        }

        if(hit.transform.gameObject.tag == "Rope")
        {
            Destroy(hit.transform.gameObject);
        }
    }
}
