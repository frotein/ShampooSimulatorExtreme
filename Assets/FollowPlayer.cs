using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    Vector2 diff;
    // Use this for initialization
	void Start ()
    {
        diff = transform.position.XY() - player.transform.position.XY(); 
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = (player.transform.position.XY() + diff).XYZ(transform.position.z);
        transform.up = player.transform.up;     
	}
}
