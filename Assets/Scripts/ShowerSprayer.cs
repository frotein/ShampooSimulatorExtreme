using UnityEngine;
using System.Collections;

public class ShowerSprayer : MonoBehaviour {

    public WaterManager manager;
    public Transform spawnPosition,left, right;
    public float spawnWait;
    float timeElapsed;

    // Use this for initialization
	void Start ()
    {
        timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= spawnWait)
        {
            float rt = Random.Range(0.0f, 1f);
              
            Vector2 vec = RandomVelocity(rt);
            manager.startingVelocity = vec;
            manager.SpawnDrop(spawnPosition.position.XY());

            rt = Random.Range(0f, 1f);
            manager.startingVelocity = RandomVelocity(rt);
            manager.SpawnDrop(spawnPosition.position.XY());

            //manager.startingVelocity = RandomVelocity(0.5f);
            //manager.SpawnDrop(spawnPosition.position.XY());

            timeElapsed = 0;
        }
    }

    Vector2 RandomVelocity(float t)
    {
        
        Vector2 pos = (left.position.XY() - right.position.XY()) * t + right.position.XY();
        Vector2 dir = (pos - spawnPosition.position.XY()).normalized;
        return dir;
    }
}
