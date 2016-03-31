using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstants : MonoBehaviour {

    public float limbSpeed;
    public float grabAmount;
    public float closeAmount;
    public bool onGround;
    public LayerMask obstacleLayer, grabbableLayer;
    public LayerMask playerLayer;
    public GameObject[] grabbableObjects;
    // Use this for initialization
	void Start ()
    {
        Constants.player = this;
	}
	
	// Update is called once per frame
	void Update () {
	
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
