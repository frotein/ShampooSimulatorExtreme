using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
public class WaterManager : MonoBehaviour {

    public Draw2DMetaballs shaderController;
    public Transform activePool;
    public Transform sleepingPool;
    public Vector2 startingVelocity;  
  
    // Use this for initialization
	void Start ()
    {
        startingVelocity = Vector2.zero;    
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnDrop(Vector2 position)
    {
        if (sleepingPool.childCount > 0)
        {
            Transform dropT = sleepingPool.GetChild(0);
            dropT.gameObject.SetActive(true);
            dropT.parent = activePool;
            dropT.position = position.XYZ(dropT.position.z);
            shaderController.metaballs.Add(dropT);
            dropT.GetComponent<WaterDrop>().index = shaderController.metaballs.Count - 1;
            Rigidbody2D rb = dropT.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = startingVelocity;
        }
    }

    public void DespawnDrop(Transform drop)
    {
        shaderController.metaballs.Remove(drop);
        drop.gameObject.SetActive(false);
        drop.parent = sleepingPool;
    }    
}
