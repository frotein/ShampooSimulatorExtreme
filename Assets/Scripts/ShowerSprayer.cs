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
            Vector2 vec = RandomVelocity();
            manager.startingVelocity = vec;
            manager.SpawnDrop(spawnPosition.position.XY());

            manager.startingVelocity = new Vector2(-vec.x, vec.y);
            manager.SpawnDrop(spawnPosition.position.XY());

            timeElapsed = 0;
        }
    }

    Vector2 RandomVelocity()
    {
        float rt = Random.Range(0f, 1f);
        Vector2 pos = (left.position.XY() - right.position.XY()) * rt + right.position.XY();
        Vector2 dir = (pos - spawnPosition.position.XY()).normalized;
        return dir;
    }
}
