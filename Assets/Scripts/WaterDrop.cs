﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterDrop : MonoBehaviour {

    
    public int index;
    public float despawnTime;
    public float dripSpeed;
    public WaterManager manager;
    public LayerMask player;
    public LayerMask ignorePlayer;
    public WaterTile currentTile;
    Rigidbody2D rb;
    bool dripping;
    float radius;
    int inWallCheck;
    bool inHair;
    int tintWait = 3;
    int wait;
    List<Transform> currentlyOn;
    // Use this for initialization
	void Start ()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        dripping = false;
        radius = transform.GetComponent<CircleCollider2D>().radius;
        inWallCheck = 0;
        inHair = false;
        currentlyOn = new List<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (despawnTime <= 0)
            manager.DespawnDrop(transform);

	}

    void DrippingPhysics()
    {
        inHair = false;
        despawnTime -= Time.deltaTime;
        if (dripping)
        {
            currentlyOn.Clear();
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position.XY(), player);
            if (cols.Length == 0)
            {
                dripping = false;
                transform.parent = manager.activePool;
                transform.localScale = new Vector3(1, 1, 1);
                //rb.simulated = true;
                rb.drag = 0;
                currentlyOn.Clear();
            }
            else
            {

                bool onFeet = false;
                Transform topTransform = null;
                foreach (Collider2D col in cols)
                {
                    SoapBubbles bubbles = col.GetComponent<SoapBubbles>();
                    if (bubbles != null)
                    {
                        bubbles.Shrink();
                    }
                    if (col.tag == "Limb End")
                        onFeet = true;

                    if (col.tag == "Hair")
                        inHair = true;

                    if (inHair)
                        Constants.player.status.AddToHairWetness();
                    else
                    { if (!onFeet) Constants.player.status.AddToBodyWetness(); }

                    if (col.tag != "Soap Bubbles" && col.tag != "dirt" && col.tag != "Tile")
                    {
                        if (topTransform != null)
                        {
                            if (topTransform.position.z > col.transform.position.z)
                                topTransform = col.transform;
                        }
                        else
                            topTransform = col.transform;

                        currentlyOn.Add(col.transform);

                    }
                }

                transform.parent = topTransform;



            }

        }
        else
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position.XY(), radius, player);
            foreach (Collider2D col in cols)
            {
                if (col.tag != "Soap Bubbles" && col.tag != "dirt")
                {
                    dripping = true;
                    transform.parent = col.transform;
                    rb.drag = 1.5f;
                    rb.velocity = new Vector2(0, -1f);
                    //  rb.simulated = false;
                }
            }
        }

    }
    void FixedUpdate()
    {
        DrippingPhysics();
        foreach (Transform t in currentlyOn)
        {
            WetTintCircleController circleController = t.GetComponent<WetTintCircleController>();
            if(circleController != null && wait <= 0)
            {
                if (!circleController.AtMax())
                {
                    bool farEngoughAway = true;
                    foreach (Transform t2 in circleController.positionTransforms)
                    {
                        if (Vector2.Distance(transform.position.XY(), t2.position.XY()) < 0.1f)
                        { farEngoughAway = false; break; }
                    }
                    if (farEngoughAway)
                    {
                        GameObject point = new GameObject("Point");
                        point.AddComponent<WetnessPoint>();
                        point.transform.position = transform.position;
                        circleController.positionTransforms.Add(point.transform); 
                        point.transform.parent = circleController.pointHolder;
                        wait = tintWait;
                    }
                }
            }
        }

        wait--;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
      
        if(col.transform.tag == "Ground" || col.gameObject.layer == LayerMask.NameToLayer("Water Ignore Player"))
        {
            if(gameObject.layer != LayerMask.NameToLayer("Water Ignore All"))
            gameObject.layer = LayerMask.NameToLayer("Water Ignore Player");         
        }
        
    }

    public void SwitchWaterTile(WaterTile newTile)
    {
        if(currentTile != null)
            currentTile.water.Remove(transform);

        if (!newTile.water.Contains(transform))
            newTile.water.Add(transform);

        currentTile = newTile;
    }
   
}
