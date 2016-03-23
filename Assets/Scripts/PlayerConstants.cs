﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstants : MonoBehaviour {

    public float limbSpeed;
    public float grabAmount;
    public float closeAmount;
    public LayerMask obstacleLayer;
    public GameObject[] grabbableObjects;
    // Use this for initialization
	void Start ()
    {
        Constants.player = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
