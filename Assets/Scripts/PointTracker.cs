using UnityEngine;
using System.Collections;

public class PointTracker : MonoBehaviour {

    public int points;
    float startingAngle;
    public float changeInAngle;
    public Rigidbody2D player;
    // Use this for initialization
	void Start ()
    {
        points = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Constants.player.onGround)
        {
            if (Mathf.Abs(changeInAngle) > 150)
                points += (int)(Mathf.Abs(changeInAngle) / 2);
            changeInAngle = 0;
        }
        else
        {
            float change = (startingAngle - player.transform.eulerAngles.z);
            if(change < 150)
                changeInAngle += change;

        }

        startingAngle = player.transform.eulerAngles.z;
    }
}
