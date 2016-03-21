using UnityEngine;
using System.Collections;

public class PushAway : MonoBehaviour {

    public Vector2 movement;

    Vector2 storedMovement;
    Vector2 prevPosition;
    Rigidbody2D rb;
    bool pushing;
    bool started; // when we started pushing down to jump
    bool ended; // when we stopped pushing d0own to jump
    Vector2 jumpForce; // the total force of pushing down to jump
    float jumpTime; // how many frames we have been pushing down to jump
    // Use this for initialization
	void Start ()
    {
        rb = transform.GetComponent<Collider2D>().attachedRigidbody;
        pushing = false;
        prevPosition = transform.position.XY();
        Debug.Log(transform.name);
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*  if(ended || movement.magnitude == 0)
          {
              if (jumpForce.magnitude > 0)
              {

                  Vector2 totalForce = (jumpForce / ((jumpTime + 1) / 2f)) * 10000f;
                  //rb.AddForceAtPosition(totalForce, transform.position.XY());
                  //Debug.Log("finished pushing");
              }
              started = false;
              ended = false;
              jumpTime = 0;
              jumpForce = Vector2.zero;
          }*/
        //Debug.Log(rb.velocity);
        storedMovement = transform.position.XY() - prevPosition;
    }

    void FixedUpdate()
    {
        if (started)
        {
            jumpForce += movement;
            jumpTime++;
        }

        if (!pushing)
        {
           //  Debug.Log("pushed");
        }
    }
    void LateUpdate()
    {

        prevPosition = transform.position.XY();
        pushing = false;
    }
    void OnCollisionStay2D(Collision2D col)
    {
        pushing = true;
       
        foreach (ContactPoint2D cp in col.contacts)
        {
            Debug.Log(Vector2.Dot(cp.normal, storedMovement.normalized));
            //  rb.AddForceAtPosition((movement / col.contacts.Length) * 1000f, cp.point);
        }
      //  col.
        //Debug.Log("in col");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        started = true;
        Debug.Log("Started");

    }

    void OnCollisionExit2D(Collision2D col)
    {
        ended = true;
    }
}
