using UnityEngine;
using System.Collections;

public class ShampooBottle : ActivateableObject {

    public ShampooManager manager;
    public Transform spawnPosition;
    public float spawnDelay;
    public float spawnT;
    float angle = 15f;
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
            float ang = spawnPosition.right.XY().Angle();

            float fAng = ang + Random.Range(-angle, angle) - 90;

            Vector2 velocity = fAng.DegreeToVector2() * squeezeAmt * squeezeAmt * 7f;
            //Debug.Log(velocity);
            manager.startingVelocity = velocity;
            manager.SpawnDrop(spawnPosition.position.XY());
            spawnT = spawnDelay / (2 * squeezeAmt);
        }
    }

    public override void Activate()
    {
        SqueezedBottle(0.7f);
    }
}
