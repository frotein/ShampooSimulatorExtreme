using UnityEngine;
using System.Collections;


// the point of this script is to try to make a way to have a limb consisting of 2 parts
// can move the 2 wheel joints so the end of the "hand" is at the mouse
public class MoveLimbToPoint : MonoBehaviour {

    public WheelJoint2D upper, lower, ankle;
    float storedAngle;
    Vector2 thigh;
    public float distance;
    public Vector2 desiredPoint;
    public float framesToReachTarget;
    public Transform test;
    public float minimunAngleChange;
    float ankleDistance;
    public float prevUpperMotorSpeed;
    // Use this for initialization
	void Start ()
    {
        storedAngle = Mathf.Atan2(-upper.transform.up.y, -upper.transform.up.x);
        distance = Vector2.Distance(upper.transform.TransformPoint(upper.anchor), lower.transform.TransformPoint(lower.anchor));
        ankleDistance = Vector2.Distance(ankle.transform.position, ankle.transform.TransformPoint(ankle.anchor));
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        thigh = upper.transform.TransformPoint(upper.anchor);
        // first find out how much the speed moves the wheel each frame
        float angle = Mathf.Atan2(-upper.transform.up.y, -upper.transform.up.x);
        
        storedAngle = angle;
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        desiredPoint = mouseWorld - new Vector2(ankleDistance, 0);
        if (Vector2.Distance(thigh, desiredPoint) > distance * 2)
        {
            Vector2 dir = (mouseWorld - thigh).normalized;
            desiredPoint = thigh + (dir * distance * 2);
        }
            

        
        Vector2 middlePoint = Calculate3rdPoint(distance, thigh, desiredPoint,true);
        test.position = new Vector3(middlePoint.x, middlePoint.y, test.position.z); ;
        float upperAngle = AngleFromVector(-upper.transform.up);
        float desiredUpperAngle = AngleFromVector(middlePoint - thigh);
        float lowerAngle = AngleFromVector(-lower.transform.up);
        float desiredLowerAngle = AngleFromVector(desiredPoint - middlePoint);

        
        float changeInUpperAngle = DesiredAngleDifference(upperAngle, desiredUpperAngle);
        float changeInLowerAngle = DesiredAngleDifference(lowerAngle, desiredLowerAngle);
        float changeInAnkleAngle = DesiredAngleDifference(AngleFromVector(ankle.transform.right), 0);

        SetMotorSpeed(upper, changeInUpperAngle, prevUpperMotorSpeed);
        prevUpperMotorSpeed = upper.motor.motorSpeed;
        Debug.Log(prevUpperMotorSpeed);
        SetMotorSpeed(lower, changeInLowerAngle);
        SetMotorSpeed(ankle, changeInAnkleAngle);
       
    }

    public void SetMotorSpeed(WheelJoint2D joint, float changeInAngle, float previousMotorSpeed = 0)
    {
        JointMotor2D motor = joint.motor;
        if (Mathf.Abs(changeInAngle) > minimunAngleChange)
           motor.motorSpeed = (changeInAngle / -0.000349f) / (framesToReachTarget);
        else
            motor.motorSpeed = 0;

        if (previousMotorSpeed * motor.motorSpeed < 0)
        { motor.motorSpeed = 0; Debug.Log("went"); }
        
       
        
        joint.motor = motor;
    }
    public float DesiredAngleDifference(float startingAngle, float newAngle)
    {
        float change = newAngle - startingAngle;
        
        if (change > Mathf.PI)
            change -= Mathf.PI * 2;

        if (change < -Mathf.PI)
            change += Mathf.PI * 2;


        return change;
    }
    // if point c is left of line defined by pts a and b
    public bool isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
    }
     float AngleFromVector(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x);
    }
    // calculates the middle point in a triangle where you know the the two side points and the length of two of the sides, 
    // bool for switch side it should be

    Vector2 Calculate3rdPoint(float length, Vector2 p1, Vector2 p2, bool flip = false)
    {
        float dist = Vector2.Distance(p1, p2);
        float sideA = dist / 2f;
        float sideB = Mathf.Sqrt(Mathf.Abs((length * length) - (sideA * sideA)));
        Vector2 midPoint = (p1 - p2) * .5f + p2;
        Vector2 dir = (p2 - p1).normalized;
        Vector2 perpDir = new Vector2(dir.y, -dir.x);
        Vector2 finalPt1 = midPoint + perpDir * sideB;
        Vector2 finalPt2 = midPoint + -perpDir * sideB;
     
            if (!flip)
                return finalPt1;
            else
                return finalPt2;             
    }
}
