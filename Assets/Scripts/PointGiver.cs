using UnityEngine;
using System.Collections;

public class PointGiver : MonoBehaviour {

    public int bounces;
    float distanceTraveled;
    float angleChanged;
    public float timeOnGround;
    public bool onGround;
    Vector3 lastPosition;

    // Use this for initialization
	void Start () {
        lastPosition = transform.position;
        distanceTraveled = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(!onGround)
        {
            distanceTraveled += Vector3.Distance(lastPosition, transform.position);
        }

        lastPosition = transform.position;
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        bounces++;
        onGround = true;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        timeOnGround += Time.deltaTime;
        if(timeOnGround > 0.1f)
        {
            bounces = 0;
            distanceTraveled = 0;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        timeOnGround = 0;
        onGround = false;
    }

    public void Caught()
    {
        if(!onGround)
            PointDisplayManager.instance.CatchResponse(bounces, 0, distanceTraveled, transform.position);

        distanceTraveled = 0;
        bounces = 0;
    }
}
