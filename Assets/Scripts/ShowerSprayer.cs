using UnityEngine;
using System.Collections;

public class ShowerSprayer : MonoBehaviour {

    public WaterManager manager;
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
            manager.SpawnDrop(transform.position.XY());
            timeElapsed = 0;
        }
    }
}
