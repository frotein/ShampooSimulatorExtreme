﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveLimb : MonoBehaviour {

	// determines which thumbstick is used
	public bool left;
    public Rigidbody2D rb;
    // The spine who is the main rotating body
    public Transform thigh;
	public Transform knee;
	public Transform upperLeg;
	public Transform lowerLeg;
    public float minimumMovement; // the smallest amount of movement needed to trigger the joint mover
    public Transform leftPt, rightPt;
    public Transform handReset;
    public Transform flipArms; // a transform designation when the arms flip their elbow to look natural

    public Transform maxLegs; // a transform designationg how high you can lift the legs up

    public List<Transform> movementLimits; // hard coded limitations for the limbs
    List<bool> startingLimitSides; // what side each limb starts on, used to check if we are going beyond a movement limit
    public float middleFix = 0.45f;

    public bool arms; // are these the hands?
    bool moving;
    bool startsLeft;   
    Vector2 movement; // the movement of this frame
    Vector3 storedLocalPosition; // the local position ... stored at the beginning
	public float length = 1.1f;
	public GameObject testPoint;
    public PushAway push;
    Vector2 storedKneePosition;
    Vector2 storedMovement;
    Vector2 prevPosition;
    bool stoppedByMax;
    bool legsStartLeft;
    bool moved;
    MoveLimbToPoint jointMover;
    float minimumMovementSqr;
    public bool classicMovement;
	// Use this for initialization
	void Start () 
	{
        storedLocalPosition = transform.localPosition;
        minimumMovementSqr = minimumMovement * minimumMovement;
        startsLeft = false;// isLeft(thigh.position.XY(), transform.position.XY(), knee.position.XY());
        if(movementLimits != null)
        {
            startingLimitSides = new List<bool>();
            foreach (Transform t in movementLimits)
            {
                startingLimitSides.Add(isLeft(t.position.XY(), t.position.XY() + t.right.XY(), t.position.XY()));
            }
        }
        if (!arms) // if these are the legs , set the default bool of what side of the line the legs are on for lift up limit;
            legsStartLeft = isLeft(maxLegs.position.XY(), maxLegs.position.XY() + maxLegs.right.XY(), transform.position.XY());

        moving = arms;
        jointMover = transform.GetComponent<MoveLimbToPoint>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        // Get the movement vector from the corrosponding analog stick
        SetMovementVector();
        if (classicMovement && jointMover != null)
        {
            jointMover.SetUpperMotorSpeed(movement.y * 10f);
            jointMover.SetLowerMotorSpeed(movement.x * -10f);
        }
        else
        {
            bool pushingAgainst = false;

            RaycastHit2D hit = Physics2D.Linecast(leftPt.position.XY() + movement * 5f,
                                                  rightPt.position.XY() + movement * 5f,
                                                  Constants.player.obstacleLayer | Constants.player.grabbableLayer);
            // if we are pushing against the ground ... 
            if (hit)
            {
                pushingAgainst = true; //signal that we are pushing
            }

            // if we are pushing against, store up movement, like preparing to push off of ground
            if (pushingAgainst && !stoppedByMax)
            {
                storedMovement += movement;
            }
            else
            {
                if (storedMovement != Vector2.zero) // if we are no longer pushing, release the power, causing something like a natural jump
                {
                    //   rb.AddForceAtPosition(-storedMovement * 1000, transform.position);
                    storedMovement = Vector2.zero;
                }
            }

            if (!arms) // if these are the legs, we dont want them to be able to clip through the player, so slide along any surface that is the player
            {
                // also if these are legs, check to make sure the new height isnt over the max height, if it is slide along the max height
                if (isLeft(maxLegs.position.XY(), maxLegs.position.XY() + maxLegs.right.XY(), transform.position.XY() + movement) != legsStartLeft)
                {
                    movement = Vector3.Project(movement.XYZ(0), maxLegs.right.XY().XYZ(0)).XY();
                }
            }
            movement = LimitMovement(5f * movement);
            // move the linbs from the movement vector
            // moveLimb();
            //Debug.Log(movement.magnitude);
            if (jointMover != null)
            {
                if (movement.magnitude > minimumMovement)
                { jointMover.MoveToPoint((Vector2)(transform.position) + movement); }
                else
                { jointMover.StopMoving(); }
            }

            if (testPoint != null)
                testPoint.transform.position = transform.position + (Vector3)movement;
        }

        moved = movement != Vector2.zero;        
	}


    public Transform GetWrongSideLimit()
    {
        int i = 0;
        foreach (Transform limit in movementLimits)
        {
            if (isLeft(limit.position.XY(), limit.position.XY() + limit.right.XY(), transform.position.XY())
                != startingLimitSides[i])
                return limit;
            i++;
        }
        return null;
    }
    void LateUpdate()
    {
        // move the limb segments so it look correct
       // SetSegments();
        storedKneePosition = knee.position.XY();
        prevPosition = transform.position.XY();
    }
    // sets the 2 segments of the limb based on the knee position
    void SetSegments()
    {
        Vector2 newPt = Calculate3rdPoint(length, thigh.position.XY(), transform.position.XY(), knee.position.XY());
     
        knee.position = newPt.XYZ(knee.position.z);
        SetToMiddleAndAngled(upperLeg, thigh.position.XY(), knee.position.XY());
        SetToMiddleAndAngled(lowerLeg, knee.position.XY(), transform.position.XY(), middleFix);
               
        transform.up = lowerLeg.up;
    }

    void SetMovementVector()
    {
        float dTime = Constants.player.limbSpeed * Time.deltaTime;
        movement = Vector2.zero;
        if (left)
        {
            if (Input.GetButton("UseLeftLeg"))
                moving = !arms;
            else
                moving = arms;
        }
        else
        {
            if (Input.GetButton("UseRightLeg"))
                moving = !arms;
            else
                moving = arms;
        }

        if (moving)
        {
            if (left)
                movement = new Vector2(Input.GetAxis("LeftStickX") * dTime, Input.GetAxis("LeftStickY") * dTime);
            else
                movement = new Vector2(Input.GetAxis("RightStickX") * dTime, Input.GetAxis("RightStickY") * dTime);
        }

    }

    // moves the limb based on the movement vector
    void moveLimb()
    {
        stoppedByMax = false;        
        transform.position += movement.XYZ(0);

        float dist = Vector2.Distance(thigh.position.XY(), transform.position.XY());
        // if the arm is bigger than the 2 limbs, then scale it back so it looks ok
        if (dist > length + length)
        {
            Vector2 dir = (transform.position.XY() - thigh.position.XY()).normalized;
            transform.position = (thigh.position.XY() + dir * (length + length)).XYZ(transform.position.z);
            stoppedByMax = true;
        }        
    }

    // if point c is left of line defined by pts a and b
    public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
    }
    
    public bool Moved()
    {
        return moved;
    }
	// calculates the middle point in a triangle where you know the the two side points and the length of two of the sides, 
	// also has the current 3rd point so the triangle doesnt flip
	Vector2 Calculate3rdPoint(float length, Vector2 p1, Vector2 p2, Vector2 currentP3)
	{
		float dist = Vector2.Distance(p1,p2);
		float sideA = dist / 2f;
		float sideB = Mathf.Sqrt(Mathf.Abs((length * length) - (sideA * sideA)));
		Vector2 midPoint = (p1 - p2) * .5f + p2;
		Vector2 dir = (p2 - p1).normalized;
		Vector2 perpDir = new Vector2(dir.y,-dir.x);
		Vector2 finalPt1 = midPoint + perpDir * sideB;
		Vector2 finalPt2 = midPoint + -perpDir * sideB;
        
        if (arms)
        {
            bool left = isLeft(flipArms.position.XY(), flipArms.position.XY() + flipArms.right.XY(), transform.position.XY()) != isLeft(p1, p2, finalPt1);
            if (left == startsLeft)
                return finalPt1;
            else
                return finalPt2;
        }
        else
        {
            if (isLeft(p1, p2, finalPt1) == startsLeft)
                return finalPt1;
            else
                return finalPt2;
        }
    }

	// Sets the transform in between the two points angled in the direction of them
	public void SetToMiddleAndAngled(Transform piece, Vector2 pt1, Vector2 pt2, float mid = 0.5f)
	{
		Vector2 midPt = (pt1 - pt2) * mid + pt2;
		Vector2 dir = (pt1 - pt2).normalized;
		
		piece.position = midPt.XYZ(piece.position.z);
		piece.up = dir;
	}

	Vector2 LimitMovement(Vector2 move)
    {
        int i = 0;
        foreach (Transform movementLimit in movementLimits)
        {
            if (startingLimitSides != null)
            {
                if (movementLimit != null && i < startingLimitSides.Count)
                {
                    bool left = isLeft(movementLimit.position.XY(), movementLimit.position.XY() + movementLimit.right.XY(), transform.position.XY() + move);
                    if (left != startingLimitSides[i])
                    {
                        move = Vector3.Project(movement.XYZ(0), movementLimit.right).XY();
                    }
                    i++;
                }
            }
        }
        
        return move;
    }

    public float FindLengthToKinematic(GameObject grabbed)
    {
        float lnth = 0;
        float lnth1 = 0;
        float lnth2 = 0;
        /*while ((starting1 != null && starting2 != null) && !foundEnd )
        {
            Rigidbody2D rb1,rb2;
            int i = 0;
            foreach (HingeJoint2D hinge in hinges)
            {
                if(i == 0)
                starting1 = 
            }
        }*/

        return lnth;
    }

}
