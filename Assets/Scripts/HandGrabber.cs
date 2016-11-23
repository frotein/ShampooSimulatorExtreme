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
    bool setLayerBackToGrabbable;
    float angleOffset;
    bool autoCorrectNextFrame;
    float changeInAngle;
    float changeInAngleTime;
    float previouseAngle;
    bool stillRotates;
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
       // handRB.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        wait += Time.deltaTime;
        if(autoCorrectNextFrame && rj != null)
        {
            rj.autoConfigureOffset = false;
            autoCorrectNextFrame = false;
        }
        
        
        // if you are grabbing this frame and you arent holding anything, and it isnt something the other hand is holding
        if (hand.grabbedThisFrame() && (grabbedGO == null || grabbedGO.layer == LayerMask.NameToLayer("Ignore Player")))
        {
            float distFromGrab = 999;
            GameObject tempGrabbedGO = null;
            // grab check
            Collider2D[] cols = Physics2D.OverlapCircleAll(handCenter.position.XY(), 0.15f);
            GameObject staticGrabbed = null;
            foreach (Collider2D col in cols)
            {
                if (col.tag == "Grabbable" || col.tag == "GrabbableKeepRotation")
                {
                   // Debug.Log("over grabbable");
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
                    Grabbed(staticGrabbed, true);
                else
                    hand.grabbing = false;

            }
        }

        // If you are grabbing an object
        if(grabbed && grabbedGO != null)
        {

            
           // Debug.Log(changeInAngle);
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
                //if(!grabbedStatic)
                Release(1000, grabbedStatic);
            }

            if(rj != null && !grabbedStatic)
            {
                if(Vector2.Distance(handRB.position, grabbedGO.transform.position.XY()) > 0.75f)
                {
                    //Release();
                }
            }
            
        }

        // waits a frame to turn on grabbed collider so it shoots in the correct direction
        if(grabbedGO != null && !grabbed)
        {
            Collider2D col = Physics2D.OverlapCircle(grabbedGO.transform.position.XY(), 0.5f, Constants.player.playerLayer); 
            if (wait >= waitToBringBackCollider && col == null)
            {
                if(!grabbedStatic)
                    grabbedGO.layer = LayerMask.NameToLayer("Grabbable");


                grabbedGO.GetComponent<Rigidbody2D>().freezeRotation = false;
                grabbedGO = null;
            }
        }

        if(grabbedGO != null && grabbed && !grabbedStatic && !stillRotates)
        {
            grabbedGO.transform.eulerAngles = new Vector3(grabbedGO.transform.eulerAngles.x, grabbedGO.transform.eulerAngles.y, -Extensions.Angle(-transform.up) - angleOffset);
        }

        if(hand.isClosed())
            hand.GetComponent<Collider2D>().enabled = true;
        else
            hand.GetComponent<Collider2D>().enabled = false;
    }


    void LateUpdate()
    {
        if (grabbedGO != null && Time.time - changeInAngleTime > 0.1f)
        {
            changeInAngle = grabbedGO.transform.eulerAngles.z - previouseAngle;
            previouseAngle = grabbedGO.transform.eulerAngles.z;
            changeInAngleTime = Time.time;
        }
    }
    // determines if the grab objects is the closest angle or 180 + is
    bool IsClosestAngle(Transform grabbedObject)
    {
        float ang = Mathf.Abs(grabbedObject.eulerAngles.z - transform.eulerAngles.z);
        return (ang < 90 || ang > 270);
    }

    // grab the given object
    void Grabbed(GameObject grabbable, bool gStatic = false)
    {
        grabbed = true;
        grabbedGO = grabbable;
        grabbedStatic = gStatic;
       
       // handRB.gameObject.SetActive(true);
        if (!grabbedStatic)
        {
            previousParent = grabbedGO.transform.parent;
            storedLocal = handRB.transform.localPosition;
            storedLayer = grabbable.layer;
            //handRB.transform.position = grabbedGO.transform.position;

            grabbable.layer = LayerMask.NameToLayer("Ignore Player");

            grabbedGO.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            Joint2D[] connectedJoints = grabbedGO.GetComponentsInChildren<Joint2D>();
            foreach (Joint2D joint in connectedJoints)
            {
                connectedBodies.Add(joint.connectedBody);
                  if (grabbedGO.tag != "GrabbableKeepRotation")
                joint.connectedBody.gameObject.layer = LayerMask.NameToLayer("Ignore Player");
            }
            //Debug.Log("grabbed");
            if (grabbedGO.tag != "GrabbableKeepRotation")
            {
                rj = grabbedGO.AddComponent<RelativeJoint2D>();
                rj.connectedBody = handRB;
                autoCorrectNextFrame = true;
                angleOffset = -Extensions.Angle(-transform.up) - grabbedGO.transform.eulerAngles.z;
            }
            else
            {
                //TargetJoint2D tj = gr
            }
            if (grabbedGO.tag != "GrabbableKeepRotation")
                grabbedGO.GetComponent<Rigidbody2D>().freezeRotation = true;
            else
                stillRotates = true;
            changeInAngleTime = Time.time;
            previouseAngle = grabbedGO.transform.eulerAngles.z;
           // rj.breakForce = 6000;
            rj.correctionScale = 0.7f;
           
        }
        else
        {
            mover.GrabbedStatic(grabbedGO);
        }

        if (left)
            handLimiter.leftGrabbed = grabbedGO.transform;
        else
            handLimiter.rightGrabbed = grabbedGO.transform;

        hand.grabbing = true;
        previouseAngle = grabbedGO.transform.eulerAngles.z;

        PointGiver pg = grabbedGO.GetComponent<PointGiver>();
        if(pg !=null)
        {
            pg.Caught();
        }
    }


    public void Release(float scale = 0, bool GrabbedStatic = false)
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
        if (GrabbedStatic)
            mover.ReleasedStatic();

        grabbedGO.GetComponent<Rigidbody2D>().freezeRotation = false;
        Debug.Log(changeInAngle);
        grabbedGO.GetComponent<Rigidbody2D>().angularVelocity = changeInAngle * 10f;
        stillRotates = false;
        //  handRB.gameObject.SetActive(false);
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
