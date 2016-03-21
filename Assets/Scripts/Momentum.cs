using UnityEngine;
using System.Collections;

public class Momentum : MonoBehaviour {

    Vector2 lastPosition;
    Rigidbody2D rb;
    Vector2 storedMomentum;
    public Collider2D tub;
    // Use this for initialization
	void Start ()
    {
        rb = transform.GetComponent<Collider2D>().attachedRigidbody;
        lastPosition = transform.position.XY();
        storedMomentum = Vector2.zero;
        
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 diff = transform.position.XY() - lastPosition;
        if (diff.magnitude > 0.01f)
        {
            storedMomentum += diff;
        }
        else
        {
            Debug.Log(storedMomentum);
            if (rb.IsTouching(tub))
            {
                rb.AddForce(storedMomentum * 1000f);
                storedMomentum = Vector2.zero;
            }
        }

        lastPosition = transform.position.XY();
	}
    
    
}
