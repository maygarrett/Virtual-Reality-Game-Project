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
                GrabObject();
            }
        }

        // when the trigger is released, check for object in hand, if so release it
        if (_controller.GetHairTriggerUp())
        {
            if (_objectInHand)
            {
                if (_objectInHand.tag != "Gun")
                    ReleaseObject();
                else FireGun();
            }
        }

        if (_controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if(_objectInHand)
            {
                if(_objectInHand.gameObject.tag == "Gun")
                {
                    ReleaseObject();
                }
            }
        }



    }

    // function to set a game object as an object currently resting inside pick up trigger
    private void SetCollidingObject(Collider col)
    {
        // if the object already exists or doesnt have a rigidbody, return
        if (_collidingObject || !col.GetComponent<Rigidbody>())
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
        // set object in hand to the object currently inside pick up trigger
        _objectInHand = _collidingObject;
        _collidingObject = null;
        // connecting the obect to user hand using a joint
        var joint = AddFixedJoint();
        joint.connectedBody = _objectInHand.GetComponent<Rigidbody>();

        if(_objectInHand.tag == "Gun")
        {
            _objectInHand.transform.position = _controller.transform.pos;
            _objectInHand.transform.rotation = _controller.transform.rot;
        }
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
    }
}
