using UnityEngine;
using System.Collections;

public class TargetJointMovable : MonoBehaviour {

    public TargetJoint2D joint;
    public Transform target;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        joint.target = target.position;
	}
}
