using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowelSegment : MonoBehaviour {

    public bool endSegment;
    bool grabbed;
    float drag;
    Rigidbody2D rb;
    Vector2 distToPlayer;
   // WetTintCircleController tintController;
    Collider2D col;
    // Use this for initialization
	void Start ()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        drag = rb.drag;
        grabbed = false;
        col = transform.GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool moving = false;
        //if (grabbed)
        {
            Vector2 newDist = transform.position.XY() - Constants.player.transform.position.XY();
            if ((newDist - distToPlayer).magnitude > 0.25f)
            {               
                distToPlayer = newDist;
                moving = true;        
            }
        }
        if (moving)
        {
            foreach (WetTintCircleController tintController in Constants.player.wetTintControllers)
            {
                List<Transform> removedPoints = new List<Transform>();
                foreach (Transform t in tintController.positionTransforms)
                {
                    if (col.OverlapPoint(t.position.XY()))
                    {
                        t.GetComponent<WetnessPoint>().despawnTime -= Time.deltaTime * 1.5f;
                        if (t.GetComponent<WetnessPoint>().despawnTime <= 0)
                            removedPoints.Add(t);
                    }
                }

                foreach (Transform t in removedPoints)
                { tintController.positionTransforms.Remove(t); Destroy(t.gameObject); }


            }
        }
	}

    
    void SetGrabbed(bool g)
    {
        grabbed = g;
        distToPlayer = transform.position.XY() - Constants.player.transform.position.XY();
    }
}
