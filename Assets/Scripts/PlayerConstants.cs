using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstants : MonoBehaviour {

    public float limbSpeed;
    public float grabAmount;
    public float closeAmount;
    public float squeezeAmount;
    public bool onGround;
    public PlayersStatus status;
    public LayerMask obstacleLayer, grabbableLayer;
    public LayerMask playerLayer, grabCheckLayer, ignorePlayerLayer;
    public GameObject[] grabbableObjects;
    public List<WetTintCircleController> wetTintControllers;
    public RotateTorso torsoRotator;
    MoveLimb[] limbs;
    HandCloser[] closer;
    bool controllingLeftArm = true;
    bool controllingRightArm = true;
    bool LeftCanGrab;
    bool rightCanGrab;
    // Use this for initialization
	void Start ()
    {
        Constants.player = this;
        wetTintControllers = new List<WetTintCircleController>();
        limbs  = transform.GetComponentsInChildren<MoveLimb>();
        closer = transform.GetComponentsInChildren<HandCloser>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LeftHorizontalMovement(float axis)
    {
        foreach(MoveLimb limb in limbs)
        {
            if (limb.left && limb.moving) limb.SetHorizontalMovement(axis);
        }
    }

    public void LeftVerticalMovement(float axis)
    {

        foreach (MoveLimb limb in limbs)
        {
            if (limb.left && limb.moving) limb.SetVerticalMovement(axis);
        }
    }

    public void RightHorizontalMovement(float axis)
    {
        foreach (MoveLimb limb in limbs)
        {
            if (!limb.left && limb.moving) limb.SetHorizontalMovement(axis);
        }
    }

    public void RightVerticalMovement(float axis)
    {
        foreach (MoveLimb limb in limbs)
        {
            if (!limb.left && limb.moving) limb.SetVerticalMovement(axis);
        }
    }

    public void ToggleGrabbingLeft()
    {
        foreach(HandCloser c in closer)
        {
            if (c.left)
                c.ToggleGrabbing();
        }
    }

    public void ToggleGrabbingLeftAxis(float axis)
    {
        if(axis > 0.8f && LeftCanGrab)
        {
            ToggleGrabbingLeft();
            LeftCanGrab = false;
        }

        if (axis < 0.2f) LeftCanGrab = true;
    }

    public void ToggleGrabbingRightAxis(float axis)
    {
        if (axis > 0.8f && rightCanGrab)
        {
            ToggleGrabbingRight();
            rightCanGrab = false;
        }
        if (axis < 0.2f) rightCanGrab = true;
        
    }

    public void ToggleGrabbingRight()
    {
        foreach (HandCloser c in closer)
        {
            if (!c.left)
                c.ToggleGrabbing();
        }
    }

    public void ToggleLeftControl()
    {
        foreach(MoveLimb m in limbs)
        {
            if (m.left)
                m.moving = !m.moving;
        }
    }

    public void ToggleRightControl()
    {
        foreach (MoveLimb m in limbs)
        {
            if (!m.left)
                m.moving = !m.moving;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Ground")
        {
            onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.tag == "Ground")
        {
            onGround = false;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.tag == "Ground")
        {
            onGround = true;
        }
    }
}
