using UnityEngine;
using System.Collections;

public class DoubleTargetJoint
{
    TargetJoint2D joint1, joint2;
    Vector2 joint1localOffset, joint2LocalOffset;

    public DoubleTargetJoint(TargetJoint2D j1, TargetJoint2D j2, Vector3 worldPos)
    {
        this.joint1 = j1;
        this.joint2 = j2;
        joint1.autoConfigureTarget = false;
        joint2.autoConfigureTarget = false;
        joint1localOffset = joint2.transform.InverseTransformPoint(worldPos);
        joint2LocalOffset = joint1.transform.InverseTransformPoint(worldPos);
    }

    public void Update()
    {
        Debug.Log(joint1 + " " + joint2);
        joint1.target = joint2.transform.TransformPoint(joint1localOffset);
        joint2.target = joint1.transform.TransformPoint(joint2LocalOffset);
    }

    public void Destroy()
    {
        GameObject.Destroy(joint1);
        GameObject.Destroy(joint2);
    }
	
}
