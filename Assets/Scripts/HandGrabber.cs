using UnityEngine;
using System.Collections;

public class HandGrabber : MonoBehaviour {

    public HandCloser hand;
    public HandLimiter handLimiter;
    public LayerMask defaultLayer, towelLayer;
    public Transform handCenter; // used to check if we can grab
    public bool left;
    float waitToBringBackCollider = 0.01f;
    bool grabbed;
    float wait;
    public GameObject grabbedGO;
    Collider2D handCollider;
    float plusAngle;
    bool grabbedStatic;
    MovementLimiter moveLimit;
    Transform previousParent;
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
        // if you are grabbing this frame and you arent holding anything, and it isnt something the other hand is holding
        if(hand.grabbedThisFrame() && grabbedGO == null)
        {
            float distFromGrab = 999;
            GameObject tempGrabbedGO = null;
            // grab check
            Collider2D[] cols = Physics2D.OverlapCircleAll(handCenter.position.XY(), 0.15f);
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

            if (tempGrabbedGO != null)
            {
                if(!OtherHandIsHolding(tempGrabbedGO.transform))
                    Grabbed(tempGrabbedGO);
            }
        }

        // If you are grabbing an object
        if(grabbed && grabbedGO != null)
        {
            //grabbedGO.transform.position = new Vector3(grabPosition.position.x, grabPosition.position.y, grabbedGO.transform.position.z);
            //grabbedGO.transform.eulerAngles = new Vector3(0, 0, -grabPosition.transform.right.XY().Angle() + 90 + plusAngle);

            if (hand.openedThisFrame())
            {

                // if you grabbed the soap, check the apply soap as grabbed
                ApplySoap soap = grabbedGO.GetComponent<ApplySoap>();
                if (soap != null)
                {
                    soap.grabbed = false;
                }

                if (moveLimit != null)
                {
                    handLimiter.handsLimiter = null;
                    moveLimit = null;
                }

                Release(Vector2.zero);
            }

            if(hand.closedThisFrame())
            {                
                if (grabbedGO.name == "SoapBar") // if youare holding soap, fling it from your hand
                {
                    // if you grabbed the soap, check the apply soap as grabbed
                    ApplySoap soap = grabbedGO.GetComponent<ApplySoap>();
                    if (soap != null)
                    {
                        soap.grabbed = false;
                    }

                    Vector2 SlipDirAndPower = grabbedGO.transform.right * 10;
                    // randomly sets power and direction of flinging soap
                    SlipDirAndPower *= Random.Range(0.5f, 1.5f);
                    if (Random.value > 0.5f)
                        SlipDirAndPower *= -1;

                    Release(SlipDirAndPower);
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
            Collider2D col = Physics2D.OverlapCircle(grabbedGO.transform.position.XY(), 0.5f, Constants.player.playerLayer); 
            if (wait >= waitToBringBackCollider && (col == null || grabbedGO.layer == LayerMask.NameToLayer("Towel")) )
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
        previousParent = grabbedGO.transform.parent;
        grabbedStatic = grabbedGO.GetComponent<Rigidbody2D>().isKinematic;
        

        if (left)
            handLimiter.leftGrabbed = grabbedGO.transform;
        else
            handLimiter.rightGrabbed = grabbedGO.transform;

        grabbedGO.GetComponent<Rigidbody2D>().isKinematic = true;
        // if you grabbed the soap, check the apply soap as grabbed
        ApplySoap soap = grabbedGO.GetComponent<ApplySoap>();
        if(soap != null)
        {
            soap.grabbed = true;
        }

        grabbedGO.transform.parent = transform;
        grabbedGO.GetComponent<Collider2D>().enabled = false;
        hand.grabbing = true;
        if (IsClosestAngle(grabbedGO.transform))
            plusAngle = 0;
        else plusAngle = 180;
    }


    void Release(Vector2 velocity)
    {
        grabbedGO.GetComponent<Rigidbody2D>().isKinematic = grabbedStatic;
        grabbedGO.GetComponent<Rigidbody2D>().velocity = velocity;
        wait = 0;

        if (left)
            handLimiter.leftGrabbed = null;
        else
            handLimiter.rightGrabbed = null;

        grabbed = false;
        hand.grabbing = false;
        grabbedGO.transform.parent = previousParent;
    }

    bool OtherHandIsHolding(Transform grabbed)
    {
        bool otherHandHas = false;
        if (left)
            otherHandHas = handLimiter.rightGrabbed == grabbed;
        else
            otherHandHas = handLimiter.leftGrabbed == grabbed;    
        
       return otherHandHas;
    }

    // crreates limiter for towel if grabbed, not used and may be moved
    void CreateLimiter()
    {
        // if you grab the towel, set grabbed section to kinematic
        if (grabbedGO.transform.parent != null)
        {
            if (grabbedGO.transform.parent.name == "Towel" || grabbedGO.transform.parent.name.Contains("Curtain"))
            {
                LengthFinder lf = grabbedGO.GetComponent<LengthFinder>();
                if (lf != null)
                {
                    if (lf.staticLength != 0)
                    {
                        //  moveLimit = new MovementLimiter(transform, lf.staticTransform, lf.staticLength + .25f, true, false);
                        //  handLimiter.handsLimiter = moveLimit;
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
