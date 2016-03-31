using UnityEngine;
using System.Collections;

public class AllowPassThrough : MonoBehaviour {

    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Water")
        {
            col.gameObject.layer = LayerMask.NameToLayer("Water Ignore All");
        }
    }
}
