using UnityEngine;
using System.Collections;

public class MoveLimb : MonoBehaviour {

	// determines which thumbstick is used
	public bool left;
    public Collider2D tub;
    public Rigidbody2D rb;
    // The spine who is the main rotating body
    public Transform thigh;
	public Transform knee;
	public Transform upperLeg;
	public Transform lowerLeg;
    public Transform leftPt, rightPt;
	public float middleFix = 0.45f;
    public bool arms;
    bool moving;
    bool startsLeft;
    Vector2 movement;
	Vector3 storedLocalPosition;
	public float length = 1.1f;
	GameObject testPoint;
    public PushAway push;
    Vector2 storedMovement;
    Vector2 prevThighPosition;
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
        
        SetMovementVector();
 
        bool pushingAgainst = false;
        if (leftPt != null && rightPt != null)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(leftPt.position.XY() + movement * 5f, rightPt.position.XY() + movement * 5f);

            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform.name == tub.name)
                { pushingAgainst = true; Debug.Log("pushing"); break; }

            }
        }        

        if(pushingAgainst && !stoppedByMax)
        {
            storedMovement += movement; 
        }
        else
        {
            if (storedMovement != Vector2.zero)
            {
                rb.AddForceAtPosition(-storedMovement * 500, transform.position);
                storedMovement = Vector2.zero;
            }
        }
       
        moveLimb();
        SetSegments();           
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
        transform.position += movement.XYZ(0);
        
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
