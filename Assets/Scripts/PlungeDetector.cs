﻿using UnityEngine;
using System.Collections;

public class PlungeDetector : Detector {

    Vector2 prevPositionOnLine;
    public PoopThrower thrower;
    bool once;
    // Use this for initialization
	void Start ()
    {
        once = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(completed && once)
        {
            thrower.ThrowPoop(Random.Range(225f, 350f));
            once = false;
        }
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
                float dist = Vector2.Distance(newPos, prevPositionOnLine);

                if (dist > 0.1f)
                {
                    value += dist;
                  
                    transform.GetComponent<ShootOutLiquid>().StartSpraying(0.3f, dist * 50f);
                }
               prevPositionOnLine = newPos;
            }
        }
        /*
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
        if (rb != null)
            transform.GetComponent<ShootOutLiquid>().StartSpraying(0.2f, Mathf.Abs(rb.velocity.magnitude) * 1.75f); */
    }


}
