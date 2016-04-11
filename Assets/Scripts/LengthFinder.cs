using UnityEngine;
using System.Collections;

public class LengthFinder : MonoBehaviour {

    public Transform staticTransform; // is length is static, this object is the rigidbody that is always kinematic
    public float staticLength;
    // Use this for initialization
	void Start ()
    {
        if(staticTransform != null)
        staticLength = Vector2.Distance(transform.position.XY(), staticTransform.position.XY());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
