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
    public GameObject tilePrefab;
    //public int arrayWidth;
    ComputeBuffer buffer;
    Material mat;
    Transform[,] tiles;
    public List<Transform>[,] tileLists;
    ComputeBuffer[,] tileBuffers;
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
        if(useTiling)
            SetUpTiles();
    }

    // puts the positions in an array to be sent
	Vector2[] SetArrayData(List<Transform> list)
    {
        Vector2[] temp = new Vector2[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            temp[i] = list[i].position.XY();
        }
        return temp;
    }
	// Update is called once per frame
	void Update ()
    {
        
        SetTilesLists();
        //SetBuffersAndMaterials();
        buffer.SetData(SetArrayData(balls));
        mat.SetBuffer("_Buffer", buffer);
        mat.SetFloat("_Radius", radius);
        mat.SetInt("_Width", balls.Count);

    }
    // assigns each ball to a tile list depending on its location
    void SetTilesLists()
    {
        for(int i = 0; i < xTiles; i++)
        {
            for (int j = 0; j < yTiles ; j++)
            {
                tileLists[i, j].Clear();  
            }
        }

        foreach (Transform t in balls)
        {
            int xPos = 0;
            int yPos = 0;
            for (int i = 0; i < xTiles; i++)
            {
                Transform tile = tiles[i, 0];
                if(t.position.x < tile.position.x + tile.lossyScale.x &&
                   t.position.x > tile.position.x - tile.lossyScale.x)
                {
                    xPos = i;
                    i = xTiles;
                }
            }

            for (int i = 0; i < yTiles; i++)
            {
                Transform tile = tiles[0, i];
                if (t.position.y < tile.position.y + tile.lossyScale.y &&
                    t.position.y > tile.position.y - tile.lossyScale.y)
                {
                    yPos = i;
                    i = yTiles;
                }
            }
            tileLists[xPos, yPos].Add(t);
        }
    }

    void SetBuffersAndMaterials()
    {
        for (int i = 0; i < xTiles; i++)
        {
            for (int j = 0; j < yTiles; j++)
            {
                tileBuffers[i, j].SetData(SetArrayData(tileLists[i,j]));
                Material mat = tiles[i,j].GetComponent<Renderer>().material;
                mat.SetBuffer("_Buffer", tileBuffers[i, j]);
                mat.SetInt("_Width", tileLists[i, j].Count);
                mat.SetFloat("_Radius", radius);
                mat.SetColor("_WaterColor", waterColor);
            }
        }
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
        Vector2 topCorner =  transform.position.XY() + (size / 2);
        Vector2 bottomCorner = transform.position.XY() - (size / 2);
        tiles = new Transform[xTiles,yTiles];
        Vector2 halfSize = ((new Vector2((topCorner.x - bottomCorner.x) * (1f / ((float)xTiles)) + bottomCorner.x,
                                        (topCorner.y - bottomCorner.y) * (1f / ((float)yTiles)) + bottomCorner.y)) - bottomCorner) / 2;
        for (int y = 0; y < yTiles; y++)
        {          
            float yI = ((float)y) / ((float)yTiles); 
          
            for (int x = 0; x < xTiles; x++)
            {
                float xI = ((float)x) / ((float)xTiles);
                GameObject temp = GameObject.Instantiate(tilePrefab);
                temp.transform.position = new Vector2((topCorner.x - bottomCorner.x) * xI + bottomCorner.x,
                                                      (topCorner.y - bottomCorner.y) * yI + bottomCorner.y) + halfSize;
                temp.transform.position = temp.transform.position.XY().XYZ(transform.position.z);
                temp.transform.parent = transform;
                temp.transform.localScale = new Vector3(1f / ((float)xTiles), 1f / (float)yTiles, 1);
                tiles[x, y] = temp.transform;
            }
        }

        tileLists = new List<Transform>[xTiles, yTiles];
        tileBuffers = new ComputeBuffer[xTiles, yTiles];
        for (int i = 0; i < xTiles; i++)
        {         
            for (int j = 0; j < yTiles; j++)
            {
                ComputeBuffer buffer = new ComputeBuffer(50, sizeof(float) * 2, ComputeBufferType.Default);
                tileBuffers[i, j] = buffer;
                tileLists[i, j] = new List<Transform>();
            }
        }

    }



 }
