using UnityEngine;
using System.Collections;

public class SmallHop : MonoBehaviour
{
    public Rigidbody2D rb;
    public float power;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    /*if(Input.GetButtonDown("Hop") && Constants.player.onGround)
        {
            rb.AddForce(transform.up.XY() * power);
        }*/
	}
}
