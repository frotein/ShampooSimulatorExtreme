using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopThrower : MonoBehaviour {

    public GameObject poop;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("y"))
            ThrowPoop(300);	
	}

    public void ThrowPoop(float power)
    {
        poop.transform.position = transform.position;
        poop.SetActive(true);
        poop.transform.right = transform.up;
        poop.GetComponent<Rigidbody2D>().AddForce(transform.up * power);
    }
}
