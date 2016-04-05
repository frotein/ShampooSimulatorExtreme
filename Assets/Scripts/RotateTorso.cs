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
        
     //   Debug.Log(transform.localEulerAngles);
	}

    void LateUpdate()
    {
      
    }
}
