using UnityEngine;
using System.Collections;

public class Drain : MonoBehaviour {

    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Water")
            col.GetComponent<WaterDrop>().manager.DespawnDrop(col.transform);
        if(col.tag == "Shampoo")
            col.GetComponent<ShampooDrop>().manager.DespawnDrop(col.transform);
    }
}
