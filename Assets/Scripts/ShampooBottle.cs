using UnityEngine;
using System.Collections;

public class ShampooBottle : MonoBehaviour {

    public ShampooManager manager;
    public Transform spawnPosition;
    public float spawnDelay;
    public float spawnT;
    // Use this for initialization
	void Start ()
    {
        spawnT = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        spawnT -= Time.deltaTime;
	}

    public void SqueezedBottle(float squeezeAmt)
    {
        if (spawnT <= 0)
        {
            Vector2 velocity = spawnPosition.right * squeezeAmt * squeezeAmt * 7f;
            //Debug.Log(velocity);
            manager.startingVelocity = velocity;
            manager.SpawnDrop(spawnPosition.position.XY());
            spawnT = spawnDelay / (2 * squeezeAmt);
        }
    }
}
