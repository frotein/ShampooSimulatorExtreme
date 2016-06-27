using UnityEngine;
using System.Collections;

public class MovableJoint : MonoBehaviour {

    public HingeJoint2D joint;
    public Transform target;
  //  public FixedJoint2D startJoint;
    // Use this for initialization
	void Start ()
    {
  //      startJoint.anchor = startJoint.transform.InverseTransformPoint(transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        joint.anchor = joint.transform.InverseTransformPoint(transform.position);
	}
}
