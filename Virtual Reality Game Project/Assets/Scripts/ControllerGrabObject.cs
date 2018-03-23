using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

    // initial setup for tracking a controller and its input
    private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device _controller
    {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    private Quaternion _rotationValue = new Quaternion(-0.2706f, 0.65328f, 0.2706f, -0.65328f);

    [SerializeField] private GameObject _controllerModel;
    // [SerializeField] private GameObject _cameraRig;

    // Object colliding with pickup trigger
    private GameObject _collidingObject;
    // actual object being held in hand
    private GameObject _objectInHand;

    void Awake()
    {
        // setting the tracked object to this objects tracked object
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {


        // if the trigger is hit, and an object is colliding, pick it up
        if (_controller.GetHairTriggerDown())
        {
            if (_collidingObject)
            {
                if (!_objectInHand)
                {
                    GrabObject();
                }

                else if (_objectInHand.tag == "Gun")
                {
                    FireGun();
                }
            }
        }

        // when the trigger is released, check for object in hand, if so release it
        if (_controller.GetHairTriggerUp())
        {
            if (_objectInHand)
            {
                if (_objectInHand.tag != "Gun")
                    ReleaseObject();
            }
        }

        if (_controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if(_objectInHand)
            {
                if (_objectInHand.gameObject.tag == "Gun")
                {

                    _objectInHand.transform.parent = null;
                    ReleaseObject();
                    _controllerModel.SetActive(true);

                }
            }
        }



    }

    // function to set a game object as an object currently resting inside pick up trigger
    private void SetCollidingObject(Collider col)
    {
        // if the object already exists or doesnt have a rigidbody, return
        if (_collidingObject || !col.GetComponent<Rigidbody>() || col.gameObject.tag != "PickUp" && col.gameObject.tag != "Key" && col.gameObject.tag != "Gun")
        {
            return;
        }
        // otherwise set the object as something that may be picked up
        _collidingObject = col.gameObject;

    }

    // when an object enters the pick up trigger this function triggers
    public void OnTriggerEnter(Collider other)
    {
        
        SetCollidingObject(other);
    }

    // when an object stays inside the pick up trigger
    public void OnTriggerStay(Collider other)
    {
        
        SetCollidingObject(other);
    }

    // when an object exits the pick up trigger, clear the colliding object
    public void OnTriggerExit(Collider other)
    {
        if (!_collidingObject)
        {
            return;
        }

        _collidingObject = null;
    }

    // grabbing an object
    private void GrabObject()
    {
        Debug.Log("calling grab object");
        // set object in hand to the object currently inside pick up trigger
        _objectInHand = _collidingObject;
        _collidingObject = null;

        // handling gun case
        if (_objectInHand.tag == "Gun")
        {

            _objectInHand.transform.parent = gameObject.transform;
            _objectInHand.transform.localPosition = _controllerModel.transform.localPosition;
            _objectInHand.transform.localRotation = _rotationValue;



            _controllerModel.gameObject.SetActive(false);
        }


        // connecting the obect to user hand using a joint
        var joint = AddFixedJoint();
        joint.connectedBody = _objectInHand.GetComponent<Rigidbody>();

    }

    // function to add the fixed joint
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    // function to release a held object, either drop or throw functionality
    private void ReleaseObject()
    {
        // check if the joint exists
        if (GetComponent<FixedJoint>())
        {
            // disconnect and destroy the joint
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // set the object that was in hand's velocity and angular velocity to that of the controller, aids with throwing mechanic
            _objectInHand.GetComponent<Rigidbody>().velocity = _controller.velocity;
            _objectInHand.GetComponent<Rigidbody>().angularVelocity = _controller.angularVelocity;
        }
        // clear the variable
        _objectInHand = null;
    }

    private void FireGun()
    {
        Debug.Log("Bang Bang");
        _objectInHand.GetComponent<GunShot>().FireGun();
    }

    public GameObject GetObjectInHand()
    {
        return _objectInHand;
    }
}
