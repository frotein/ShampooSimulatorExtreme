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
    Vector2 tileLength;
    Vector2 bottomCorner;
    //public int arrayWidth;
   // ComputeBuffer buffer;
    Material mat;
    public TileVariables[,] tileVariables;
    // test variables
    public Transform test1, test2;
    public Vector2 overlap;
    float radiusSqr;
    // Use this for initialization
    void Start () 
    {
        positionsArray = new Vector2[poolSize];
        mat = transform.GetComponent<Renderer>().material;
        mat.SetColor("_WaterColor", waterColor);
        //buffer = new ComputeBuffer(positionsArray.Length, sizeof(float) * 2, ComputeBufferType.Default);
        if(balls == null)
        balls = new List<Transform>();
        if(useTiling)
            SetUpTiles();

        radiusSqr = Mathf.Sqrt(radius) * 5;
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
    }
    // assigns each ball to a tile list depending on its location
    void SetTilesLists()
    {
        foreach (TileVariables tv in tileVariables)
            tv.balls.Clear();
       // Transform t = test1;
       
        
        foreach (Transform t in balls)
        {
            List<int> xPos = new List<int>();
            List<int> yPos = new List<int>();

            // calculate what tile the center is in
            Vector2 pos = (t.position.XY() - bottomCorner);
            pos.x /= tileLength.x;
            pos.y /= tileLength.y;

            // check position with some leeway for overlappping
            xPos.Add(Mathf.FloorToInt(pos.x + overlap.x));
            int pos2X = Mathf.FloorToInt(pos.x - overlap.x);
            if(!xPos.Contains(pos2X))
            {
                xPos.Add(pos2X);
            }

            yPos.Add(Mathf.FloorToInt(pos.y + overlap.y));
            int pos2Y = Mathf.FloorToInt(pos.y - overlap.y);
            if (!yPos.Contains(pos2Y))
            {
                yPos.Add(pos2Y);
            }

            foreach (int x in xPos)
            {
                if (x < xTiles && x >= 0)
                {
                    foreach (int y in yPos)
                    { if (y < yTiles && y >= 0) tileVariables[x, y].balls.Add(t); }
                }
            }

        }
        
    }

    // releases all buffers when done
    void OnDestroy()
    {
     //   buffer.Release();       
    }

    // tiles shader to be more optimized, passes nieghbors into each tile variable
    void SetUpTiles()
    {
        Vector2 size = transform.GetComponent<Renderer>().bounds.size;
        
        Vector2 topCorner =  transform.position.XY() + (size / 2);
        bottomCorner = transform.position.XY() - (size / 2);
        tileVariables = new TileVariables[xTiles,yTiles];
        Vector2 halfSize = ((new Vector2((topCorner.x - bottomCorner.x) * (2f / ((float)(xTiles))) + bottomCorner.x,
                                        (topCorner.y - bottomCorner.y) * (2f / ((float)(yTiles))) + bottomCorner.y)) - bottomCorner) / 2;

        // places all tiles
        for (int y = 0; y < yTiles; y++)
        {          
            float yI = ((float)y) / (((float)yTiles)); 
          
            for (int x = 0; x < xTiles; x++)
            {
                float xI = ((float)x) / ((float)(xTiles));
                GameObject temp = GameObject.Instantiate(tilePrefab);
                temp.name = "Tile " + x + "" + y;
                temp.transform.position = new Vector2((topCorner.x - bottomCorner.x) * xI + bottomCorner.x,
                                                      (topCorner.y - bottomCorner.y) * yI + bottomCorner.y) + halfSize / 2f;
                temp.transform.position = temp.transform.position.XY().XYZ(transform.position.z);
                temp.transform.parent = transform;
                temp.transform.localScale = new Vector3(1f / ((float)(xTiles)), 1f / ((float)yTiles), 1);
                tileLength = temp.GetComponent<Renderer>().bounds.size;
                //temp.transform.parent = null;
                //temp.transform.localScale += new Vector3(2 * radius, 2 * radius);
                temp.transform.parent = transform;

                TileVariables tv = temp.GetComponent<TileVariables>();
                tv.SetRadius(radius);
                tv.SetWaterColor(waterColor);
                tileVariables[x, y] = tv;
                tv.x = x;
                tv.y = y;
            }
        }

        // assignes all neighbors for tiles
        for(int x = 0; x < xTiles; x++)
        {
            for(int y = 0; y < yTiles; y++)
            {
                TileVariables tv = tileVariables[x, y];
                int neighborCount = 4;
                if (x == 0 || x == xTiles - 1) neighborCount--;
                if (y == 0 || y == yTiles - 1) neighborCount--;
                tv.neighbors = new TileVariables[neighborCount];
                int added = 0;
                if(x != 0) { tv.neighbors[added] = tileVariables[x - 1, y]; added++; }
                if(x != xTiles - 1) { tv.neighbors[added] = tileVariables[x + 1, y]; added++; }
                if (y != 0) { tv.neighbors[added] = tileVariables[x, y - 1]; added++; }
                if (y != yTiles - 1) { tv.neighbors[added] = tileVariables[x, y + 1]; }
            }
        }
    }



 }
