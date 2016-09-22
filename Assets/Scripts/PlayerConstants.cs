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
    public LayerMask playerLayer, chestLayer;
    public GameObject[] grabbableObjects;
    public List<WetTintCircleController> wetTintControllers;
    MoveLimb[] limbs;
    bool controllingLeftArm = true;
    bool controllingRightArm = true;
    // Use this for initialization
	void Start ()
    {
        Constants.player = this;
        wetTintControllers = new List<WetTintCircleController>();
        limbs  = transform.GetComponentsInChildren<MoveLimb>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LeftHorizontalMovement(float axis)
    {
        foreach(MoveLimb limb in limbs)
        {
            if (limb.left && limb.arms == controllingLeftArm) limb.SetHorizontalMovement(axis);
        }
    }

    public void LeftVerticalMovement(float axis)
    {
        foreach (MoveLimb limb in limbs)
        {
            if (limb.left && limb.arms == controllingLeftArm) limb.SetVerticalMovement(axis);
        }
    }

    public void RightHorizontalMovement(float axis)
    {
        foreach (MoveLimb limb in limbs)
        {
            if (!limb.left && limb.arms == controllingLeftArm) limb.SetHorizontalMovement(axis);
        }
    }

    public void RightVerticalMovement(float axis)
    {
        foreach (MoveLimb limb in limbs)
        {
            if (!limb.left && limb.arms == controllingLeftArm) limb.SetVerticalMovement(axis);
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
