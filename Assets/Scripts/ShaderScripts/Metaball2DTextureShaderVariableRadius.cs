using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Metaball2DTextureShaderVariableRadius : MonoBehaviour {

    public float startingRadius;
    public Transform sleepingPool; // the pool the objects start in, used to be set the starting radius
    public Color waterColor;
    public Vector3[] positionsAndRadiusArray;
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
    void Start()
    {
        positionsAndRadiusArray = new Vector3[poolSize];
        mat = transform.GetComponent<Renderer>().material;
        mat.SetColor("_WaterColor", waterColor);
        buffer = new ComputeBuffer(positionsAndRadiusArray.Length, sizeof(float) * 3, ComputeBufferType.Default);
        if (balls == null)
            balls = new List<Transform>();
        SetStartingRadius();
    }

    void SetStartingRadius()
    {
        for(int i = 0; i < sleepingPool.childCount; i++)
        {
            Transform temp = sleepingPool.GetChild(i);
            temp.GetComponent<ShampooDrop>().drawingRadius = startingRadius;
        }
    }
    // puts the positions in an array to be sent, stores the radius in the z position
    void SetArrayData()
    {
        for (int i = 0; i < balls.Count; i++)
        {           
            positionsAndRadiusArray[i] = new Vector3(balls[i].position.x, 
                                                     balls[i].position.y, 
                                                     balls[i].GetComponent<ShampooDrop>().drawingRadius);
        }
    }
    // Update is called once per frame
    void Update()
    {
        SetArrayData();
        buffer.SetData(positionsAndRadiusArray);
        mat.SetBuffer("_Buffer", buffer);
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
