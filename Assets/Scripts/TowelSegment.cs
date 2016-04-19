using UnityEngine;
using System.Collections;

public class TowelSegment : MonoBehaviour {

    public bool endSegment;
    bool grabbed;
    float drag;
    Rigidbody2D rb;
    bool onPlayer, onHair;
    Vector2 distToPlayer;
    // Use this for initialization
	void Start ()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        drag = rb.drag;
        grabbed = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(grabbed)
        {
            Vector2 newDist = transform.position.XY() - Constants.player.transform.position.XY();
            if ((newDist - distToPlayer).magnitude > 0.5f)
            {               
                distToPlayer = newDist;
                if(onPlayer)
                    Constants.player.status.DryBody();
                if(onHair)
                    Constants.player.status.DryHair();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            rb.drag = 500;
            if (col.tag == "Hair")
                onHair = true;
            else
                onPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            rb.drag = drag;
            if (col.tag == "Hair")
                onHair = false;
            else
               onPlayer = false;
        }
    }

    void SetGrabbed(bool g)
    {
        grabbed = g;
        distToPlayer = transform.position.XY() - Constants.player.transform.position.XY();
    }
}
