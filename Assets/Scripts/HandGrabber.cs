using UnityEngine;
using System.Collections;

public class HandGrabber : MonoBehaviour {

    public HandCloser hand;
    public Transform grabPosition;

    float waitToBringBackCollider = 0.01f;
    bool grabbed;
    float wait;
    GameObject grabbedGO;
    Collider2D handCollider;
    float plusAngle;
    
    // Use this for initialization
	void Start ()
    {
        grabbed = false;
        handCollider = transform.GetComponent<Collider2D>();
        plusAngle = 0;
        waitToBringBackCollider = 0.05f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        wait += Time.deltaTime;
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
                    
                    if (IsClosestAngle(grabbedGO.transform))
                        plusAngle = 0;
                    else plusAngle = 180;
                }
            }
        }

        // If you are grabbing an object
        if(grabbed && grabbedGO != null)
        {
            grabbedGO.transform.position = new Vector3(grabPosition.position.x, grabPosition.position.y, grabbedGO.transform.position.z);
            grabbedGO.transform.eulerAngles = new Vector3(0, 0, -grabPosition.transform.right.XY().Angle() + 90 + plusAngle);

            if (hand.openedThisFrame())
            {
                grabbed = false;
                grabbedGO.GetComponent<Collider2D>().enabled = true;
                grabbedGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                grabbedGO = null;
            }

            if(hand.closedThisFrame())
            {
                grabbed = false;
                //
               // grabbedGO.GetComponent<Collider2D>().enabled = true;

                
                Vector2 SlipDirAndPower = grabbedGO.transform.right * 10;
                // randomly sets power and direction of flinging soap
                SlipDirAndPower *= Random.Range(0.5f, 2f);
                if (Random.value > 0.5f)
                    SlipDirAndPower *= -1;

                grabbedGO.GetComponent<Rigidbody2D>().velocity = SlipDirAndPower ;
                wait = 0;
            }
        }

        // waits a frame to turn on grabbed collider so it shoots in the correct direction
        if(grabbedGO != null && !grabbed)
        {
            if (wait >= waitToBringBackCollider)
            {
                grabbedGO.GetComponent<Collider2D>().enabled = true;
                grabbedGO = null;Debug.Log("Turned on Collider");
            }
        }

        if(hand.isClosed())
            hand.GetComponent<Collider2D>().enabled = true;
        else
            hand.GetComponent<Collider2D>().enabled = false;
    }

    // determions if the grab objects is the closest angle or 180 + is
    bool IsClosestAngle(Transform grabbedObject)
    {
        //Debug.Log((Mathf.Abs(grabbedObject.eulerAngles.z - transform.eulerAngles.z)));
        return (Mathf.Abs( grabbedObject.eulerAngles.z - transform.eulerAngles.z) < 90);
    }
    
}
