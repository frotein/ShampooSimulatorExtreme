using UnityEngine;
using System.Collections;

public class DirtPile : MonoBehaviour {

    CircleCollider2D col;
    public LayerMask playerLayer;
    public Vector2 localPos;
    // Use this for initialization
	void Start ()
    {
        col = transform.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position.XY() + col.offset, col.radius, playerLayer);
        int overlaps = 0;
        foreach (Collider2D col2 in cols)
        {
            if(col2.tag == "Soap Bubbles" && col2.transform.parent == transform.parent)
            {
                overlaps++;
            }
        }

        if (overlaps >= 2)
            gameObject.SetActive(false);     
	}
}
