using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterTile : MonoBehaviour {

    public List<Transform> water;
    // Use this for initialization
	void Start ()
    {
        water = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void FixedUpdate()
    {
        List <Transform>  removing = new List<Transform>();
        foreach (Transform t in water)
        {
            if (!t.gameObject.activeSelf) removing.Add(t);
        }
        foreach(Transform t in removing)
            water.Remove(t);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(!water.Contains(col.transform))
        water.Add(col.transform);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        water.Remove(col.transform);
    }
}
