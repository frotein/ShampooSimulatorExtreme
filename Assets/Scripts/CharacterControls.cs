using UnityEngine;
using System.Collections;

// This script takes in the input from the controller, or keyboaed for testing, and moves the character on screen
// Controls include, moving arms, legs, jumping, and leaning left and right
public class CharacterControls : MonoBehaviour
{
    public Transform player;
    public float sensitivity;
    Transform torso, upperLeftLeg,upperRightLeg, lowerLeftLeg, lowerRightLeg;
    Transform leftThigh, rightThigh, leftKnee, rightKnee, leftFoot, rightFoot;

    float legLength;
	// Use this for initialization
	void Start ()
    {
        if (player == null)
            player = transform;

        torso = player.GetChild(0);
        leftThigh = torso.GetChild(2);
        rightThigh = torso.GetChild(3);
        upperLeftLeg = leftThigh.GetChild(0);
        upperRightLeg = rightThigh.GetChild(0);
        leftKnee = upperLeftLeg.GetChild(0);
        legLength = Vector2.Distance(leftThigh.position.XY(), leftKnee.position.XY()); //2.7f;
        Debug.Log(legLength);
        rightKnee = upperRightLeg.GetChild(0);
        lowerLeftLeg = leftKnee.GetChild(0);
        lowerRightLeg = rightKnee.GetChild(0);
        leftFoot = lowerLeftLeg.GetChild(0);
        rightFoot = lowerRightLeg.GetChild(0);


    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 LeftFootMovement = new Vector2(Input.GetAxis("LeftStickX"), -Input.GetAxis("LeftStickY"));
        Vector2 RightFootMovement = new Vector2(Input.GetAxis("RightStickX"), -Input.GetAxis("RightStickY"));

        LeftFootMovement *= Time.deltaTime;
        RightFootMovement *= Time.deltaTime;

        leftThigh.localEulerAngles += new Vector3(0, 0, LeftFootMovement.y * -sensitivity);
        leftKnee.localEulerAngles += new Vector3(0, 0, LeftFootMovement.x * sensitivity);

        rightThigh.localEulerAngles += new Vector3(0, 0, RightFootMovement.y * sensitivity);
        rightKnee.localEulerAngles += new Vector3(0, 0, RightFootMovement.x * sensitivity);

    }

    // calculates the middle point in a triangle where you know the the two side points and the length of two of the sides, 
    // also has the current 3rd point so the triangle doesnt flip
    Vector2 Calculate3rdPoint(float length, Vector2 p1, Vector2 p2, Vector2 currentP3)
    {
        float dist = Vector2.Distance(p1, p2);
        float sideA = dist / 2f;
        float sideB = Mathf.Sqrt(Mathf.Abs((length * length) - (sideA * sideA)));
        Vector2 midPoint = (p1 - p2) * .5f + p2;
        Vector2 dir = (p2 - p1).normalized;
        Vector2 perpDir = new Vector2(dir.y, -dir.x);
        Vector2 finalPt1 = midPoint + perpDir * sideB;
        Vector2 finalPt2 = midPoint + -perpDir * sideB;

        float dist1 = currentP3.DistanceSqr(finalPt1);
        float dist2 = currentP3.DistanceSqr(finalPt2);
        if (dist1 < dist2)
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
        piece.right = dir;
    }
}
