using UnityEngine;
using System.Collections;

public class RopeManager : MonoBehaviour {

    RopeSegment[] segments;
    // Use this for initialization
	void Start ()
    {
        segments = transform.GetComponentsInChildren<RopeSegment>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(HighestForce());
	}

    public float AverageForce()
    {
        float final = 0;

        if(segments != null)
        {
            
            foreach(RopeSegment segment in segments)
            {
                final += segment.AverageForceOnSegment();
            }

            final /= segments.Length;
        }

        return final;
    }

    public float HighestForce()
    {
        float final = 0;

        if(segments != null)
        {
            foreach(RopeSegment s in segments)
            {
                float force = s.AverageForceOnSegment();
                if (force > final)
                    final = force;
            }
        }
        return final;
    } 
}
