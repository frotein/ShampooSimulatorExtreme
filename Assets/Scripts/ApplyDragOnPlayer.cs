using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ApplyDragOnPlayer : MonoBehaviour {

    public float drag;
    float startingDrag;
    Rigidbody2D rb;
    bool onHair;
    bool onPlayer;
    public Transform startingParent;
    //public List<Transform> onTopList;
    // Use this for initialization
	void Start ()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        startingDrag = rb.drag;
        startingParent = transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        //rb.drag = startingDrag;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            FakeChild fakeC = col.GetComponent<FakeChild>();

            //if (!onTopList.Contains(col.transform))
            //    onTopList.Add(col.transform);
            

            if (fakeC != null)
                transform.parent.parent = fakeC.fakeChild;

            rb.drag = drag;
        }      
    }
    void ResetParent()
    {
        //transform.parent.parent = startingParent;
        //Debug.Log("Reset Parent");
    }
    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            rb.drag = startingDrag;
            transform.parent.parent = startingParent;            
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            rb.drag = drag;
            FakeChild fakeC = col.GetComponent<FakeChild>();

           // if (fakeC != null)
            //    transform.parent.parent = fakeC.fakeChild;
        }
    }

}
