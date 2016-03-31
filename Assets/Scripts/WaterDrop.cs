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
        if(dripping)
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position.XY(), radius, player);
            if (col == null)
            {
                dripping = false;
                transform.parent = manager.activePool;
            }
        }
        else
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position.XY(), radius,player);
            foreach (Collider2D col in cols)
            {
                dripping = true;
                transform.parent = col.transform;
                rb.velocity = new Vector2(0, 0);
            }   
        }

        if (despawnTime <= 0)
            manager.DespawnDrop(transform);

	}

    void FixedUpdate()
    {
        despawnTime -= Time.deltaTime;
        if (dripping)
        {
            rb.MovePosition(transform.position.XY() + new Vector2(0, -dripSpeed));           
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
      
        if(col.transform.tag == "Ground" || col.gameObject.layer == LayerMask.NameToLayer("Water Ignore Player"))
        {
            if(gameObject.layer != LayerMask.NameToLayer("Water Ignore All"))
            gameObject.layer = LayerMask.NameToLayer("Water Ignore Player");         
        }
        
    }
}
