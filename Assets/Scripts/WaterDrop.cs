using UnityEngine;
using System.Collections;

public class WaterDrop : MonoBehaviour {

    public int index;
    public float despawnTime;
    public float dripSpeed;
    public WaterManager manager;
    public LayerMask player;
    public LayerMask ignorePlayer;
    Rigidbody2D rb;
    bool dripping;
    float radius;
    // Use this for initialization
	void Start ()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        dripping = false;
        radius = transform.GetComponent<CircleCollider2D>().radius;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (dripping)
        {
            transform.position -= new Vector3(0, dripSpeed * Time.deltaTime, 0);
            Collider2D col =Physics2D.OverlapCircle(transform.position.XY(), radius, player);
            if (col == null)
            {
                dripping = false;
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.velocity = new Vector3(0, dripSpeed * Time.deltaTime, 0);
                rb.useAutoMass = true;
                transform.parent = manager.activePool;
            }
        }

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Player")
        {
            Destroy(rb);
            dripping = true;
            transform.parent = col.transform;
        }

        if(col.transform.tag == "Ground" || col.gameObject.layer == LayerMask.NameToLayer("Water Ignore Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("Water Ignore Player");
        }
    }
}
