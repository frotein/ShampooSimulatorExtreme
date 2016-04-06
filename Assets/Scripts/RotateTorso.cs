using UnityEngine;
using System.Collections;

public class RotateTorso : MonoBehaviour {

    // Use this for initialization
    public Transform leftThigh, rightThigh;
    public Collider2D chestCol;
    public MoveLimb left, right;
    public float leanRate;
    float lastZ;
    void Start ()
    {
        lastZ = transform.eulerAngles.z;

    }
	
	// Update is called once per frame
	void Update ()
    {
        float leanDir = Input.GetAxisRaw("Lean");

      //  if (chestCol.OverlapPoint(leftThigh.position.XY()))
      //      Debug.Log("touching left thigh");
        lastZ = transform.eulerAngles.z;
        if ( (!chestCol.OverlapPoint(leftThigh.position.XY()) || leanDir < 0) && (!chestCol.OverlapPoint(rightThigh.position.XY()) || leanDir > 0))
            transform.eulerAngles += new Vector3(0, 0, leanRate * leanDir * Time.deltaTime);

        Transform limit = right.GetWrongSideLimit();
        if (limit != null)
            right.transform.position = PointOnLine(limit.position.XY(), limit.position.XY() + limit.right.XY(), right.transform.position.XY());

        limit = left.GetWrongSideLimit();
        if (limit != null)
            left.transform.position = PointOnLine(limit.position.XY(), limit.position.XY() + limit.right.XY(), left.transform.position.XY());

    }

    void LateUpdate()
    {
      
    }

    // gets the closest point p2 to a line defined by pt0 and pt1
    Vector2 PointOnLine(Vector2 pt0, Vector2 pt1, Vector2 pt2)
    {
        Vector2 dir = (pt0 - pt1);
        Vector2 perpDir = new Vector2(-dir.y, dir.x);
        Vector2 pt3 = pt2 + perpDir;
        float storedX = (pt3.x - pt2.x);
        float storedY = (pt3.y - pt2.y);
        float nx = ((pt0.y - pt2.y) * storedX) - ((pt0.x - pt2.x) * storedY);
        float t = nx / ((storedY * (pt1.x - pt0.x)) - (storedX * (pt1.y - pt0.y)));

        return (pt1 - pt0) * t + pt0;
    }
}
