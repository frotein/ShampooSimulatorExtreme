using UnityEngine;
using System.Collections;

public class MoveLimb : MonoBehaviour {

	// determines which thumbstick is used
	public bool left;
    public Collider2D tub;
    public Rigidbody2D rb;
    // The spine who is the main rotating body
    public Transform chest;
	public Transform thigh;
	public Transform knee;
	public Transform upperLeg;
	public Transform lowerLeg;
	public float middleFix = 0.45f;
    public bool arms;
    bool moving;
    bool startsLeft;
    Vector2 prevChestPosition;
    Vector2 movement;
	Vector3 storedLocalPosition;
	public float length = 1.1f;
	GameObject testPoint;
    public PushAway push;
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
        prevChestPosition = chest.position.XY();  
	}
	
	// Update is called once per frame
	void Update () 
	{
        stoppedByMax = false;
        moveLimb();


        //if (movement.magnitude > 0)
        {
            Vector2 newPt = Calculate3rdPoint(length, thigh.position.XY(), transform.position.XY(), knee.position.XY());
		    knee.position = newPt.XYZ(0);
            SetToMiddleAndAngled(upperLeg, thigh.position.XY(), knee.position.XY());
            SetToMiddleAndAngled(lowerLeg, knee.position.XY(), transform.position.XY(), middleFix);
            transform.up = lowerLeg.up;
        }
            
       
        if (push != null)
            push.movement = -movement;
        

	}

    void FixedUpdate()
    {
        //Debug.Log(movement);
        
        bool pushingAgainst = false;

        if(Mathf.Abs(movement.x) > 0)
        {
            float xDiff = transform.position.x - prevPosition.x;
            if (Mathf.Abs(xDiff) < 0.001f) pushingAgainst = true;
        }

        if (Mathf.Abs(movement.y) > 0)
        {
            float yDiff = transform.position.y - prevPosition.y;
            if (Mathf.Abs(yDiff) < 0.001f) pushingAgainst = true;
        }
        if (pushingAgainst && !stoppedByMax)
        {
            Vector2 diff = chest.position.XY() - prevChestPosition;
            //Debug.Log("pushing");
            if (diff.magnitude > 0.001f)
            {
                
              //  if (rb.IsTouching(tub))
                {
                 //   Debug.Log("going");
                    storedMovement += -movement;
                }
            }
 
        }
        else
        {
            if (storedMovement.magnitude > 0)
            {
                Debug.Log(storedMovement);
                rb.AddForceAtPosition(storedMovement * 5000f, transform.position);
                storedMovement = Vector2.zero;
            }
        }
        prevChestPosition = chest.position.XY();
        prevPosition = transform.position.XY();
    }
    void moveLimb()
    {
        float dTime = Constants.player.limbSpeed;

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

            transform.position += movement.XYZ(0);
        }
        float dist = Vector2.Distance(thigh.position.XY(), transform.position.XY());
        if (dist > length + length)
        {
            Vector2 dir = (transform.position.XY() - thigh.position.XY()).normalized;
            transform.position = (thigh.position.XY() + dir * (length + length)).XYZ(transform.position.z);
            stoppedByMax = true;
        }
    }
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
        if (isLeft(p1, p2, finalPt1) == startsLeft)
            return finalPt1;
        else
            return finalPt2;
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
