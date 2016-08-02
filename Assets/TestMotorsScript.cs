using UnityEngine;
using System.Collections;

public class TestMotorsScript : MonoBehaviour {

    public float totalSpeed;
    public MotorTest[] testMotors;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (MotorTest m in testMotors)
            m.SetMotorSpeed(totalSpeed);
	}
}

[System.Serializable]
public class MotorTest
{
    public WheelJoint2D joint;
    public float speed;

    public void SetMotorSpeed(float acc)
    {
        JointMotor2D m = joint.motor;
        m.motorSpeed = acc * speed;
        joint.motor = m;
    }
}
