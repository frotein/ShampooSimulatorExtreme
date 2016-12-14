using UnityEngine;
using System.Collections;

public class ShootOutLiquid : MonoBehaviour {

    public Transform left, right;
    public WaterManager manager;
    public float spawnDelay;
    public int amountSpawnedAtOnce;
    float startTime;
    float runTime;
    float timeElapsed;
    float power;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeElapsed += Time.deltaTime;

        if (Time.time < startTime + runTime)
        {
            if (timeElapsed >= spawnDelay)
            {
                for (int i = 0; i < amountSpawnedAtOnce; i++)
                {
                    float rt = Random.Range(0.0f, 1f);

                    manager.startingVelocity = RandomVelocity(rt) * power;
                    //Debug.Log(manager.startingVelocity);
                    manager.SpawnDrop(transform.position.XY());
                }

                timeElapsed = 0;
            }
        }
    }

    public void StartSpraying(float time, float powr)
    {
        startTime = Time.time;
        runTime = time;
        power = powr;
    }

    Vector2 RandomVelocity(float t)
    {

        Vector2 pos = (left.position.XY() - right.position.XY()) * t + right.position.XY();
        Vector2 dir = (pos - transform.position.XY()).normalized;

        return dir;
    }
}
