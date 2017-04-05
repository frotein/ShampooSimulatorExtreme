using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileVariables : MonoBehaviour {

    public int x, y;
    public List<Transform> balls;
    public List<Transform> allBalls;
    public Bounds bounds;
    public TileVariables[] neighbors;

    public float[] xPos, yPos;
    Material mat;
    
    // Use this for initialization
    void Start()
    {
        balls = new List<Transform>();
        mat = transform.GetComponent<SpriteRenderer>().material;
        bounds = transform.GetComponent<SpriteRenderer>().bounds;
    }

    void FixedUpdate()
    {

        var materialProperty = new MaterialPropertyBlock();


        SetArrayData(balls);
        materialProperty.SetFloat("_Width", (float)balls.Count);
        materialProperty.SetFloatArray("xPos", xPos);
        materialProperty.SetFloatArray("yPos", yPos); 
        gameObject.GetComponent<Renderer>().SetPropertyBlock(materialProperty);
        // if (x == 5 && y == 5) Debug.Log(transform.position + " offset " +  transform.lossyScale);           
    }

    public void SetRadius(float radius)
    {
        transform.GetComponent<SpriteRenderer>().material.SetFloat("_Radius", radius);
    }

    public void SetWaterColor(Color color)
    {
        transform.GetComponent<SpriteRenderer>().material.SetColor("_WaterColor", color);
    }

    // puts the positions in an array to be sent
    void SetArrayData(List<Transform> list)
    {
        xPos = new float[300];
        yPos = new float[300];
        for (int i = 0; i < 300; i++)
        {
            if (list.Count > i)
            {
                xPos[i] = list[i].position.x;
                yPos[i] = list[i].position.y;
            }
            else
            {
                xPos[i] = 0;
                yPos[i] = 0;
            }    
        }
    }

    void OnDestroy()
    {
    }
}
