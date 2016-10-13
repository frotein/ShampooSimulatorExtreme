using UnityEngine;
using System.Collections;

public class JointStabalizer : MonoBehaviour {

    Rigidbody2D rb; 
    public Rigidbody2D connectedBody;
    Vector3 localPositionInConnected;
    Vector3 localDirectionInConnected;

    // Use this for initialization
	void Start ()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        if(connectedBody == null)
            connectedBody = transform.GetComponent<Joint2D>().connectedBody;
        localPositionInConnected = connectedBody.transform.InverseTransformPoint(transform.position);
        localDirectionInConnected = connectedBody.transform.InverseTransformDirection(transform.up);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(rb.velocity.sqrMagnitude > 600)
        {
            rb.velocity = new Vector2(0, 0);
            transform.position = connectedBody.transform.TransformPoint(localPositionInConnected);
            transform.up = connectedBody.transform.TransformDirection(localDirectionInConnected);
            Debug.Log(rb.velocity.sqrMagnitude);

        }
    }
}
