using UnityEngine;
using System.Collections;

public class PlayerConstants : MonoBehaviour {

    public float limbSpeed;
    public float grabAmount;
    public float closeAmount;
    public LayerMask obstacleLayer;
    // Use this for initialization
	void Start ()
    {
        Constants.player = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
