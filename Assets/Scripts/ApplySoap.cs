using UnityEngine;
using System.Collections;

public class ApplySoap : MonoBehaviour {

    public bool grabbed;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if we are grabbing the soap
        if (grabbed)
        {
            // check what colliders we are overlapping, if we are on the player and not ontop of a soap bubble, apply soap bubbles
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position.XY());
            bool applySoap = false;
            foreach (Collider2D col in cols)
            {
                if (col.transform.tag == "Player")
                    applySoap = true;
                if(col.transform.tag == "Soap Bubbles")
                {
                    applySoap = false;
                    break;
                }

            }
        }
    }
}
