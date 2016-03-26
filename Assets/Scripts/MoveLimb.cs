using UnityEngine;
using System.Collections;

public class MoveLimb : MonoBehaviour {

	// determines which thumbstick is used
	public bool left;
    public Rigidbody2D rb;
    // The spine who is the main rotating body
    public Transform thigh;
	public Transform knee;
	public Transform upperLeg;
	public Transform lowerLeg;
    public Transform leftPt, rightPt;
    public Transform handReset;
    public Transform flipArms;
    public Collider2D chestCollider;
    public Collider2D movableArea;
    public float middleFix = 0.45f;
    public bool arms;
    bool moving;
    bool startsLeft;
    Vector2 movement;
	Vector3 storedLocalPosition;
	public float length = 1.1f;
	GameObject testPoint;
    public PushAway push;
    Vector2 storedKneePosition;
    Vector2 storedMovement;
    Vector2 prevPosition;
    bool stoppedByMax;
    
	// Use this for initialization
	void Start () 
	{
		storedLocalPosition = transform.localPosition;
        //Debug.Log(upperLeg.lossyScale.y);
        startsLeft = isLeft(thigh.position.XY(), transform.position.XY(), knee.position.XY());
        moving = arms;
	}
	
	// Update is called once per frame
	void Update () 
	{
        // Get the movement vector from the corrosponding analog stick
        SetMovementVector();
        bool pushingAgainst = false;

        // if we are pushing against the ground, signal that we are
        if (Physics2D.Linecast(leftPt.position.XY() + movement * 5f, rightPt.position.XY() + movement * 5f, Constants.player.obstacleLayer))
            pushingAgainst = true;

        // if we are pushing against, store up movement, like preparing to push off of ground
        if (pushingAgainst && !stoppedByMax)
        {
            storedMovement += movement; 
        }
        else
        {
            if (storedMovement != Vector2.zero) // if we are no longer pushing, release the power, causing something like a natural jump
            {
                rb.AddForceAtPosition(-storedMovement * 400, transform.position);
                storedMovement = Vector2.zero;
            }
        }

        if (!arms) // if these are the elegs, we dont want them to be able to clip through the player, so slide along any surface that is the player
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position.XY(), transform.position.XY() + movement, Constants.player.playerLayer);
           if(hit)
            {
                Vector2 perp = new Vector2(-hit.normal.y, hit.normal.x);
                Vector2 newMovement = Vector3.Project(movement.XYZ(0), perp.XYZ(0)).XY();
                Debug.Log(movement + " " + newMovement);
                movement = newMovement;
            }
        }

        // move the linbs from the movement vector
        moveLimb();

        // move the limb segments so it look correct
        SetSegments();           
	}

    void LateUpdate()
    {
        // check if we clipped through anything
        if(Physics2D.Linecast(transform.position.XY(), prevPosition, Constants.player.obstacleLayer))
        {
            Debug.Log("went through");
          //  transform.position = prevPosition.XYZ(transform.position.z);
        }
        storedKneePosition = knee.position.XY();
        prevPosition = transform.position.XY();
    }

    void SetSegments()
    {
        Vector2 newPt = Calculate3rdPoint(length, thigh.position.XY(), transform.position.XY(), knee.position.XY());
     
        knee.position = newPt.XYZ(0);
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

    void moveLimb()
    {
        stoppedByMax = false;
        bool canMoveTo = true;
            
        /*if (movableArea != null)
        {
            if (!movableArea.OverlapPoint(transform.position.XY() + movement)) { canMoveTo = false; stoppedByMax = true; }
        }*/

        if (canMoveTo)
        {
            transform.position += movement.XYZ(0);

            float dist = Vector2.Distance(thigh.position.XY(), transform.position.XY());
            if (dist > length + length)
            {
                Vector2 dir = (transform.position.XY() - thigh.position.XY()).normalized;
                transform.position = (thigh.position.XY() + dir * (length + length)).XYZ(transform.position.z);
                stoppedByMax = true;
            }
        }
    }

    // if point c is left of line defined by pts a and b
    public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
    }
    void UpdatePosition()
	{
		//transform.localPosition = storedLocalPosition;
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

	

}
