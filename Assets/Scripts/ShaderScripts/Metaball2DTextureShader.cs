using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Metaball2DTextureShader : MonoBehaviour {

    public float radius;
    public Color waterColor;
    public Vector2[] positionsArray;
    public List<Transform> balls;
    public int poolSize;
    //public int arrayWidth;
    ComputeBuffer buffer;
    Material mat;

    // test variables
    public Transform test1, test2;

    // Use this for initialization
    void Start () 
    {
        positionsArray = new Vector2[poolSize];
        mat = transform.GetComponent<MeshRenderer>().material;
        mat.SetColor("_WaterColor", waterColor);
        buffer = new ComputeBuffer(positionsArray.Length, sizeof(float) * 2, ComputeBufferType.Default);
        if(balls == null)
        balls = new List<Transform>();     
    }
	void SetArrayData()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            positionsArray[i] = balls[i].position.XY();
        }
    }
	// Update is called once per frame
	void Update ()
    {
        SetArrayData();
        buffer.SetData(positionsArray);
        mat.SetBuffer("_Buffer", buffer);
        mat.SetFloat("_Radius", radius);
        mat.SetInt("_Width", balls.Count);

    }

    void OnDestroy()
    {
        buffer.Release();
    }

 }
