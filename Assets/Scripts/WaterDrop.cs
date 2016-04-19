using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterDrop : MonoBehaviour {

    
    public int index;
    public float despawnTime;
    public float dripSpeed;
    public WaterManager manager;
    public LayerMask player;
    public LayerMask ignorePlayer;
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
        inHair = false;
        despawnTime -= Time.deltaTime;
        if (dripping)
        {
            
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position.XY(),radius, player);
            if (cols.Length == 0)
            {
                dripping = false;
                transform.parent = manager.activePool;
                transform.localScale = new Vector3(1, 1, 1);
                rb.simulated = true;

                foreach(Transform t in currentlyOn)
                {
                  //  Debug.Log("Exited " + t.name);
                }
                currentlyOn.Clear();
            }
            else
            {
                bool onFeet = false;
                Transform topTransform = null;
                List<Transform> tempCurrentlyOn = new List<Transform>(currentlyOn);
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

                    if (col.tag != "Soap Bubbles" && col.tag != "dirt")
                    {
                        if (topTransform != null)
                        {
                            if (topTransform.position.z > col.transform.position.z)
                                topTransform = col.transform;
                        }
                        else
                            topTransform = col.transform;
                        tempCurrentlyOn.Remove(col.transform);

                        if(!currentlyOn.Contains(col.transform))
                        {
                           // Debug.Log("Entered " + col.transform.name);
                            currentlyOn.Add(col.transform);
                        }
                    }
                }

                foreach(Transform t in tempCurrentlyOn)
                {
                    //Debug.Log("Exited " + t.name);
                    currentlyOn.Remove(t);                   
                }
                
                transform.parent = topTransform;
                

               
            }
            
        }
        else
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position.XY(), radius,player);
            foreach (Collider2D col in cols)
            {
                if (col.tag != "Soap Bubbles" && col.tag != "dirt")
                {
                    dripping = true;
                    transform.parent = col.transform;
                    rb.velocity = new Vector2(0, 0);
                    rb.simulated = false;
                }
            }   
        }

        if (despawnTime <= 0)
            manager.DespawnDrop(transform);

	}

    void FixedUpdate()
    {
        foreach(Transform t in currentlyOn)
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
                        point.transform.position = transform.position;
                        circleController.positionTransforms.Add(point.transform); 
                        point.transform.parent = circleController.pointHolder;
                        wait = tintWait;
                    }
                }
            }
        }

        wait--;

        if (dripping)
        {
            Vector2 newPosition = transform.position.XY() + new Vector2(0, -dripSpeed);
            RaycastHit2D hit = Physics2D.Linecast(transform.position.XY(), newPosition, Constants.player.obstacleLayer);
            if(hit.normal != Vector2.zero)
            {
                if(transform.position.x < 0)
                    newPosition = transform.position + new Vector3(dripSpeed,0,0);// + Vector3.Project(new Vector2(0, -dripSpeed), hit.normal).XY();
                else
                    newPosition = transform.position + new Vector3(-dripSpeed, 0, 0);
            }
           
            transform.position = newPosition;           
        }
        else
        {
            if (!rb.simulated) rb.simulated = true;
        }

        if (Physics2D.OverlapPoint(transform.position.XY(), Constants.player.obstacleLayer))
            inWallCheck++;
        else
            inWallCheck = 0;

        if(inWallCheck > 50)
            manager.DespawnDrop(transform);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
      
        if(col.transform.tag == "Ground" || col.gameObject.layer == LayerMask.NameToLayer("Water Ignore Player"))
        {
            if(gameObject.layer != LayerMask.NameToLayer("Water Ignore All"))
            gameObject.layer = LayerMask.NameToLayer("Water Ignore Player");         
        }
        
    }

   
}
