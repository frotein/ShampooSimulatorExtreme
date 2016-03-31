using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
public class WaterManager : MonoBehaviour {

    public Metaball2DTextureShader shaderController;
    public Transform activePool;
    public Transform sleepingPool;
    public Vector2 startingVelocity;
    public float despawnTime;
    // Use this for initialization
	void Start ()
    {
        startingVelocity = Vector2.zero;
        shaderController.poolSize = sleepingPool.childCount;
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
            dropT.gameObject.layer = LayerMask.NameToLayer("Water");
            shaderController.balls.Add(dropT);
            dropT.GetComponent<WaterDrop>().index = shaderController.balls.Count - 1;
            dropT.GetComponent<WaterDrop>().manager = this;
            dropT.GetComponent<WaterDrop>().despawnTime = despawnTime;
            Rigidbody2D rb = dropT.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = startingVelocity;
        }
    }

    public void DespawnDrop(Transform drop)
    {
        shaderController.balls.Remove(drop);
        drop.gameObject.layer = LayerMask.NameToLayer("Water");
        drop.gameObject.SetActive(false);
        drop.parent = sleepingPool;
    }    
}
