using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

    public float breakingForce; // the minimum force needed for object to break
    public int maximumBreaks, minimumBreaks;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnCollisionEnter2D(Collision2D col)
    {
        if(col.relativeVelocity.magnitude > breakingForce)
        {
            SpriteSlicer2D.ExplodeSprite(gameObject, Random.Range(minimumBreaks,maximumBreaks),col.relativeVelocity.magnitude / 2f);
            
        }
    }
}
