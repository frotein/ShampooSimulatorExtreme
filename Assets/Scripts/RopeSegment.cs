using UnityEngine;
using System.Collections;

public class RopeSegment : MonoBehaviour {

    HingeJoint2D[] joints;
    // Use this for initialization
	void Start ()
    {
        joints = transform.GetComponents<HingeJoint2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
       // Debug.Log(AverageForceOnSegment());
	}

    public float AverageForceOnSegment()
    {
        float final = 0;

        if (joints != null)
        {
            if (joints.Length > 0)
            {
                foreach (HingeJoint2D joint in joints)
                {
                    final += joint.reactionForce.magnitude;
                }

                final /= joints.Length;
            }
        }

        return final;
    }
}
