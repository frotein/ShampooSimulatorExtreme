using UnityEngine;
using System.Collections;

public class HandLimiter : MonoBehaviour {

    public MovementLimiter handsLimiter; // dynamic limiter to limit when objects certain objects are grabbed
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(handsLimiter != null)
        {
            handsLimiter.ApplyLimits();
        }
	}
}
