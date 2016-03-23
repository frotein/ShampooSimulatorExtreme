using UnityEngine;
using System.Collections;

public class HandGrabber : MonoBehaviour {

    public HandCloser hand;
    public Transform grabPosition;
    bool grabbed;
    GameObject grabbedGO;
    Collider2D handCollider;
    // Use this for initialization
	void Start ()
    {
        grabbed = false;
        handCollider = transform.GetComponent<Collider2D>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if you are grabbing this frame
        if(hand.grabbedThisFrame())
        {
            foreach (GameObject grabbable in Constants.player.grabbableObjects)
            {
                // and if you are touching a grabbable object 
                if (handCollider.IsTouching(grabbable.GetComponent<Collider2D>())) 
                {
                    // grab it
                    grabbed = true;
                    grabbedGO = grabbable;
                    grabbedGO.GetComponent<Collider2D>().enabled = false;
                    //Destroy(grabbedGO.GetComponent<Rigidbody2D>());
                }
            }
        }

        // If you are grabbing an object
        if(grabbed && grabbedGO != null)
        {
            grabbedGO.transform.position = new Vector3(grabPosition.position.x, grabPosition.position.y, grabbedGO.transform.position.z);
            grabbedGO.transform.eulerAngles = new Vector3(0, 0, -grabPosition.transform.right.XY().Angle() + 90);
        }
    }

    
}
