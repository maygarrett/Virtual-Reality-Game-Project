﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserPointer : MonoBehaviour {

    private SteamVR_TrackedObject _trackedObj;

    private SteamVR_Controller.Device _controller
    {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    //laserpointer stuff
    [SerializeField] private GameObject _laserPrefab;
    private GameObject _laser;
    private Transform _laserTransform;
    private Vector3 _hitPoint;


    // teleportation stuff
    [SerializeField] private Transform _cameraRigTransform;
    [SerializeField] private GameObject _teleportReticlePrefab;
    private GameObject _reticle;
    private Transform _teleportReticleTransform;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Vector3 _teleportReticleOffset;
    [SerializeField] private LayerMask _teleportMask;
    private bool _shouldTeleport;

    [SerializeField] private bool _isTeleportEnabled;

    // pause menu stuff
    private bool _isPaused = false;
    [SerializeField] private Canvas _pauseMenuCanvas;
    [SerializeField] private Transform _menuPositionPoint;
    [SerializeField] private LayerMask _buttonMask;
    [SerializeField] private MenuManager _menuManager;

    // main menu variables
    [SerializeField] private bool _isMenuScene;

    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        _laser = Instantiate(_laserPrefab);
        _laserTransform = _laser.transform;

        _reticle = Instantiate(_teleportReticlePrefab);
        _teleportReticleTransform = _reticle.transform;
    }

    // Update is called once per frame
    void Update () {


        if (_controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && !_isPaused)
        {
            RaycastHit hit;

            // raycast for teleport
            if (Physics.Raycast(_trackedObj.transform.position, transform.forward, out hit, 100, _teleportMask))
            {
                _hitPoint = hit.point;
                ShowLaser(hit);

                _reticle.SetActive(true);
                _teleportReticleTransform.position = _hitPoint + _teleportReticleOffset;
                _shouldTeleport = true;
            }

        }
        else
        {
            _laser.SetActive(false);
            _reticle.SetActive(false);
        }

        if (_isTeleportEnabled)
        {
            if (_controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && _shouldTeleport)
            {
                Teleport();
            }
        }

        if (!_isMenuScene)
        {
            if (_controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                PauseToggle();
            }
        }


        // pause menu interactions
        if(_isPaused || _isMenuScene)
        {
            // always show laser pointer
            RaycastHit hit;
            if (Physics.Raycast(_trackedObj.transform.position, transform.forward, out hit, 100))
            {
                _hitPoint = hit.point;
                ShowLaser(hit);
            }

            // if player aims at a button object
            if(hit.transform.gameObject.layer == 9)
            {
                // Debug.Log("Hitting Button with raycast");
                if(_controller.GetHairTriggerDown())
                {
                    GameObject _buttonPressed = hit.transform.gameObject;
                    // check what the button is
                    if (_buttonPressed.name == "QuitGameButton")
                    {
                        _menuManager.QuitGame();
                    }
                    else if (_buttonPressed.name == "ResumeButton")
                    {
                        _menuManager.ResumeGame();
                    }
                    else if (_buttonPressed.name == "StartGameButton")
                    {
                        _menuManager.StartGame();
                    }
                    else Debug.LogError("something weird going on with button " + _buttonPressed.name);
                }
            }
        }


    }

    private void ShowLaser(RaycastHit hit)
    {
        _laser.SetActive(true);
        _laserTransform.position = Vector3.Lerp(_trackedObj.transform.position, _hitPoint, .5f);
        _laserTransform.LookAt(_hitPoint);
        _laserTransform.localScale = new Vector3(_laserTransform.localScale.x, _laserTransform.localScale.y,
            hit.distance);
    }

    private void Teleport()
    {
        _shouldTeleport = false;
        _reticle.SetActive(false);
        Vector3 tempDifference = _cameraRigTransform.position - _headTransform.position;
        tempDifference.y = 0;
        _cameraRigTransform.position = _hitPoint + tempDifference;
    }

    private void PauseToggle()
    {
        _isPaused = !_isPaused;


        if(_isPaused)
        {
            _pauseMenuCanvas.transform.position = _menuPositionPoint.position;
            _pauseMenuCanvas.transform.LookAt(_headTransform);
            Time.timeScale = 0;
            _pauseMenuCanvas.gameObject.SetActive(true);
        }

        if(!_isPaused)
        {
            Time.timeScale = 1;
            _pauseMenuCanvas.gameObject.SetActive(false);
        }
    }
}
