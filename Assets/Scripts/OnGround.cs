using UnityEngine;
using System.Collections;

public class OnGround : MonoBehaviour {

    public bool onGround;
    bool hittingSomething;
    // Use this for initialization
	void Start ()
    {
	
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
