using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    Collider2D triggerCollider;
    MoveLimb mover;
    LayerMask storedLayer;
    RelativeJoint2D rj;
    Rigidbody2D handRB;
    Vector3 storedLocal;
    List<Rigidbody2D> connectedBodies;
    public float scaler = 1.1f;
    // Use this for initialization
	void Start ()
    {
        grabbed = false;
        handCollider = transform.GetComponent<Collider2D>();
        plusAngle = 0;
        connectedBodies = new List<Rigidbody2D>();
        waitToBringBackCollider = 0.05f;
        mover = transform.GetComponent<MoveLimb>();
        handRB = transform.GetComponentInChildren<Rigidbody2D>();
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
            GameObject staticGrabbed = null;
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

                if(col.tag == "Ground")
                {
                    staticGrabbed = col.gameObject;
                }
            }

            if (tempGrabbedGO != null) // if the other hand is not holding the object, grab it
            {
                if(!OtherHandIsHolding(tempGrabbedGO.transform))
                    Grabbed(tempGrabbedGO);
            }
            else // if we grabbed nothing, open hand
            {
                if (staticGrabbed != null)
                    mover.GrabbedStatic();
                    //       Grabbed(staticGrabbed);
                hand.grabbing = false;

            }
        }

        // If you are grabbing an object
        if(grabbed && grabbedGO != null)
        {
            
            if (hand.openedThisFrame()) // if you open your hand, release the object
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

                Release(1000);
            }

            if(rj != null)
            {
                if(Vector2.Distance(handRB.position, grabbedGO.transform.position.XY()) > 0.75f)
                {
                    Release();
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


    void LateUpdate()
    {
       
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
        storedLocal = handRB.transform.localPosition;
        storedLayer = grabbable.layer;
        handRB.transform.position = grabbedGO.transform.position;
        
        grabbable.layer = LayerMask.NameToLayer("Ignore Player");

        if (left)
            handLimiter.leftGrabbed = grabbedGO.transform;
        else
            handLimiter.rightGrabbed = grabbedGO.transform;
        Joint2D[] connectedJoints = grabbedGO.GetComponentsInChildren<Joint2D>();
        foreach (Joint2D joint in connectedJoints)
        {
            connectedBodies.Add(joint.connectedBody);
            joint.connectedBody.gameObject.layer = LayerMask.NameToLayer("Ignore Player");
        }
        rj = grabbedGO.AddComponent<RelativeJoint2D>();
        rj.connectedBody = handRB;
        rj.autoConfigureOffset = false;
        //rj.angularOffset = -3.36f;
        rj.linearOffset = new Vector2(0, 0);
        rj.breakForce = 6000;
        rj.correctionScale = 0.7f;
        hand.grabbing = true;
       
    }


    void Release(float scale = 0)
    {
        if (grabbedGO.GetComponent<Rigidbody2D>() != null)
        {
            grabbedGO.GetComponent<Rigidbody2D>().isKinematic = grabbedStatic;
            grabbedGO.GetComponent<Rigidbody2D>().AddForce(mover.Movement() * scale);
        }
        wait = 0;
        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
        }

        if (left)
            handLimiter.leftGrabbed = null;
        else
            handLimiter.rightGrabbed = null;

        grabbed = false;
        hand.grabbing = false;
        Destroy(rj);
        grabbedGO.layer = LayerMask.NameToLayer("Grabbable");
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
