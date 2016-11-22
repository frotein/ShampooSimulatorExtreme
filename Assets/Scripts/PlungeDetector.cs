﻿using UnityEngine;
using System.Collections;

public class PlungeDetector : Detector {

    Vector2 prevPositionOnLine;
    // Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.sharedMaterial != null)
        {
            if (col.sharedMaterial.name == "PlungerHead")
            {
                prevPositionOnLine = (Vector2)Vector3.Project(col.bounds.center, transform.up);
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.sharedMaterial != null)
        {
            if (col.sharedMaterial.name == "PlungerHead")
            {
                Vector2 newPos = (Vector2)Vector3.Project(col.bounds.center, transform.up);
                value += Vector2.Distance(newPos, prevPositionOnLine);

                prevPositionOnLine = newPos;
            }
        }
    }
}