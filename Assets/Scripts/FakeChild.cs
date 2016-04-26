using UnityEngine;
using System.Collections;

public class FakeChild : MonoBehaviour {

    public Transform fakeChild;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        fakeChild.position = transform.position; 
        fakeChild.up = transform.up;
	}
}
