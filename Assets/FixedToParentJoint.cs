using UnityEngine;
using System.Collections;

public class FixedToParentJoint : MonoBehaviour {

    public Rigidbody2D playerRB;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localPosition = new Vector3(0, 0, 0);
	}

    void ResetJoitntConnection()
    {

    }
}
