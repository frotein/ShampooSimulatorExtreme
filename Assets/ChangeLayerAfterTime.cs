using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerAfterTime : MonoBehaviour {

    public string layerName;
    public float timeToChange;
    float startTime;
    bool changed;
    // Use this for initialization
	void Start ()
    {
        changed = false;
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!changed)
        {
            if (Time.time > startTime + timeToChange)
            {
                changed = true;
                gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
	}
}
