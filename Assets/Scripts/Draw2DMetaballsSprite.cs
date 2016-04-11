using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Draw2DMetaballsSprite : MonoBehaviour {

    public float radius;
    public Color waterColor;
    public Vector2[] positionsArray;
    public List<Transform> balls;
    public int poolSize;

    //public int arrayWidth;
    ComputeBuffer buffer;
    Material mat;

    // Use this for initialization
    void Start ()
    {
        positionsArray = new Vector2[poolSize];
        mat = transform.GetComponent<SpriteRenderer>().material;
        mat.SetColor("_WaterColor", waterColor);
        buffer = new ComputeBuffer(positionsArray.Length, sizeof(float) * 2, ComputeBufferType.Default);
        if (balls == null)
            balls = new List<Transform>();

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnDestroy()
    {
        buffer.Release();
    }
}
