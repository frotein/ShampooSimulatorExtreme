using UnityEngine;
using System.Collections;

public class HandGrabber : MonoBehaviour {

    public HandCloser hand;
    public Transform grabPosition; // the position and rotation you set the grabbed object to
    public bool left;
    float waitToBringBackCollider = 0.01f;
    bool grabbed;
    float wait;
    public GameObject grabbedGO;
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
            float distFromGrab = 999;
            GameObject tempGrabbedGO = null;
            foreach (GameObject grabbable in Constants.player.grabbableObjects)
            {
                // and if you are touching a grabbable object 
                if (handCollider.IsTouching(grabbable.GetComponent<Collider2D>())) 
                {
                    float dist = Vector2.Distance(grabbable.transform.position.XY(), transform.position.XY());
                    // make sure we are grabbing the closest object
                    if (dist < distFromGrab)
                    {
                        tempGrabbedGO = grabbable;
                        distFromGrab = dist;
                    }
                    
                   
                }                
            }
            // if we have grabbed an object
            if (tempGrabbedGO != null)
            {
                // grab it
                Grabbed(tempGrabbedGO);
            }
            else // otherwise, do a secondary grab check
            {
                // secondary check for grabbing
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position.XY() + handCollider.offset, 0.05f);
                foreach (Collider2D col in cols)
                {
                    if (col.tag == "Grabbable")
                    {
                        float dist = Vector2.Distance(col.transform.position.XY(), transform.position.XY());
                        if (dist < distFromGrab) // make sure we are grabbing the closest object
                        {
                            tempGrabbedGO = col.gameObject;
                            distFromGrab = dist;
                        }
                    }
                }

                if(tempGrabbedGO != null)
                    Grabbed(tempGrabbedGO);
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
                hand.grabbing = false;
                grabbedGO.GetComponent<Collider2D>().enabled = true;
                grabbedGO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                grabbedGO = null;
            }

            if(hand.closedThisFrame())
            {                
                if (grabbedGO.name == "SoapBar") // if youare holding soap, fling it from your hand
                {
                    Vector2 SlipDirAndPower = grabbedGO.transform.right * 10;
                    // randomly sets power and direction of flinging soap
                    SlipDirAndPower *= Random.Range(0.5f, 1.5f);
                    if (Random.value > 0.5f)
                        SlipDirAndPower *= -1;

                    grabbedGO.GetComponent<Rigidbody2D>().velocity = SlipDirAndPower;
                    wait = 0;
                    grabbed = false;
                    hand.grabbing = false;
                   
                }             
            }
            if (grabbedGO != null)
            {
                float squeezeAmt = 0;
                if (left)
                    squeezeAmt = Input.GetAxis("LeftHand");
                else
                    squeezeAmt = Input.GetAxis("RightHand");

                if (grabbedGO.name == "shampoo" &&  squeezeAmt > Constants.player.squeezeAmount)
                {
                    ShampooBottle bottle = grabbedGO.GetComponent<ShampooBottle>();
                    

                    bottle.SqueezedBottle(squeezeAmt);
                }
            }
        }

        // waits a frame to turn on grabbed collider so it shoots in the correct direction
        if(grabbedGO != null && !grabbed)
        {
            if (wait >= waitToBringBackCollider)
            {
                grabbedGO.GetComponent<Collider2D>().enabled = true;
                grabbedGO = null;
            }
        }

        if(hand.isClosed())
            hand.GetComponent<Collider2D>().enabled = true;
        else
            hand.GetComponent<Collider2D>().enabled = false;
    }

    // determines if the grab objects is the closest angle or 180 + is
    bool IsClosestAngle(Transform grabbedObject)
    {
        float ang = Mathf.Abs(grabbedObject.eulerAngles.z - transform.eulerAngles.z);
        return (ang < 90 || ang > 270);
    }

    // grab the given object
    void Grabbed(GameObject grabbable)
    {
        grabbed = true;
        grabbedGO = grabbable;
        grabbedGO.GetComponent<Collider2D>().enabled = false;
        hand.grabbing = true;
        if (IsClosestAngle(grabbedGO.transform))
            plusAngle = 0;
        else plusAngle = 180;
    }
    
}
