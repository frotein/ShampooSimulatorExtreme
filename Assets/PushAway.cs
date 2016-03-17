using UnityEngine;
using System.Collections;

public class PushAway : MonoBehaviour {

    public Vector2 movement;
    
    Rigidbody2D rb;
    bool started; // when we started pushing down to jump
    bool ended; // when we stopped pushing d0own to jump
    Vector2 jumpForce; // the total force of pushing down to jump
    float jumpTime; // how many frames we have been pushing down to jump
    // Use this for initialization
	void Start ()
    {
        rb = transform.GetComponent<Collider2D>().attachedRigidbody;
      
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(ended || movement.magnitude == 0)
        {
            if (jumpForce.magnitude > 0)
            {
             
                Vector2 totalForce = (jumpForce / ((jumpTime + 1) / 2f)) * 10000f;
                rb.AddForceAtPosition(totalForce, transform.position.XY());
                Debug.Log("applied " + totalForce);
            }
            started = false;
            ended = false;
            jumpTime = 0;
            jumpForce = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (started)
        {
            jumpForce += movement;
            jumpTime++;
        }
        
    }
   
    void OnCollisionStay2D(Collision2D col)
    {
        foreach (ContactPoint2D cp in col.contacts)
        {
          //  rb.AddForceAtPosition((movement / col.contacts.Length) * 1000f, cp.point);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        started = true;

    }

    void OnCollisionExit2D(Collision2D col)
    {
        ended = true;
    }
}
