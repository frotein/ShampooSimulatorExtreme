using UnityEngine;
using System.Collections;

public class StutterDetector : MonoBehaviour {

    float previousAngle;
    // Use this for initialization
	void Start ()
    {
        previousAngle = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float changeInAngle = Mathf.Abs(transform.eulerAngles.z - previousAngle);
        if (changeInAngle > 7.5f && changeInAngle < 180f)
            Debug.Log("stuttering?");

        previousAngle = transform.eulerAngles.z;
    }


    void LateUpdate()
    {
       
    }
}
