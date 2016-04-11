using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Metaball2DTextureShader : MonoBehaviour {

    public float radius;
    public Color waterColor;
    public Vector2[] positionsArray;
    public List<Transform> balls;
    public int poolSize;
    public bool useTiling;
    public int xTiles, yTiles;
    //public int arrayWidth;
    ComputeBuffer buffer;
    Material mat;
    Transform[] tiles;

    ComputeBuffer[] tileBuffers;
    // test variables
    public Transform test1, test2;

    // Use this for initialization
    void Start () 
    {
        positionsArray = new Vector2[poolSize];
        mat = transform.GetComponent<Renderer>().material;
        mat.SetColor("_WaterColor", waterColor);
        buffer = new ComputeBuffer(positionsArray.Length, sizeof(float) * 2, ComputeBufferType.Default);
        if(balls == null)
        balls = new List<Transform>();     


    }

    // puts the positions in an array to be sent
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
    
    // releases all buffers when done
    void OnDestroy()
    {
        buffer.Release();
        if (tileBuffers != null)
        {
            foreach (ComputeBuffer b in tileBuffers)
                b.Release();
        }
    }

    // non used code to be used when i make the shader tiling for optimizations
    void SetUpTiles()
    {
        Vector2 size = transform.GetComponent<Renderer>().bounds.size;
        for (int y = 0; y < yTiles; y++)
        {
            GameObject temp = new GameObject("Tile");
            //temp.transform.position = 
            for (int x = 0; x < xTiles; x++)
            {

            }
        }
        
    }

 }
