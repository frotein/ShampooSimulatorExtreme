using UnityEngine;
using System.Collections;

public class ShampooManager : MonoBehaviour {

    public Metaball2DTextureShader shaderController;
    public Transform activePool;
    public Transform sleepingPool;
    public Vector2 startingVelocity;
    public float despawnTime;
    public float drawingRadius;
    // Use this for initialization
    void Start()
    {
        startingVelocity = Vector2.zero;
       // shaderController.poolSize = sleepingPool.childCount;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnDrop(Vector2 position)
    {
        if (sleepingPool.childCount > 0)
        {
            Transform dropT = sleepingPool.GetChild(0);
            dropT.localScale = new Vector3(1, 1, 1);
            dropT.gameObject.SetActive(true);
            dropT.parent = activePool;
            dropT.position = position.XYZ(dropT.position.z);
            dropT.gameObject.layer = LayerMask.NameToLayer("Water");
            shaderController.balls.Add(dropT);
            dropT.GetComponent<ShampooDrop>().manager = this;
            dropT.GetComponent<ShampooDrop>().despawnTime = despawnTime;
            dropT.GetComponent<ShampooDrop>().drawingRadius = drawingRadius;
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
