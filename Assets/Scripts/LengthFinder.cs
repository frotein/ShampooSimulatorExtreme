using UnityEngine;
using System.Collections;

public class LengthFinder : MonoBehaviour {

    public Transform staticTransform; // is length is static, this object is the rigidbody that is always kinematic
    public float staticLength;
    public TransformAndLength[] dynamicLengths; // the maximum lengths to all the other children of the parent of this; 
    // Use this for initialization
	void Start ()
    {
        if(staticTransform != null) // if this is a ststic length get that
        staticLength = Vector2.Distance(transform.position.XY(), staticTransform.position.XY());
        else // otherwise, get the dynamic lengths
        {
            dynamicLengths = new TransformAndLength[transform.parent.childCount - 1];
            int index = 0;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Transform temp = transform.parent.GetChild(i);
                if(temp != transform)
                {
                    TransformAndLength tempTL;
                    tempTL.transform = temp;
                    tempTL.length = Vector2.Distance(temp.position.XY(), transform.position.XY());
                    dynamicLengths[index] = tempTL;
                    index++;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

   
}

[System.Serializable]
public struct TransformAndLength
{
    public Transform transform;
    public float length;
}
