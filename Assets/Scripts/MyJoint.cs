using UnityEngine;
using System.Collections;

public class MyJoint : MonoBehaviour {

    public Rigidbody2D connectedRB;
    public float threshold;
    Rigidbody2D myRB;
    public Vector2 offset;
    public Vector2 currentOffset;
    // Use this for initialization
	void Start ()
    {
        myRB = transform.GetComponent<Rigidbody2D>();
        offset = connectedRB.position - myRB.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    currentOffset = connectedRB.position - myRB.position;
        Vector2 diff = (offset - currentOffset);
       
        if (diff.magnitude > threshold)
        {
             myRB.velocity = -diff;
            //myRB.position = connectedRB.position - offset;
        }
    }
}
