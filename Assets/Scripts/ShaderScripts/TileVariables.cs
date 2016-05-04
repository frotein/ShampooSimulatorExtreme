using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileVariables : MonoBehaviour {

    public int x, y;
    public List<Transform> balls;
    ComputeBuffer buffer;
    Material mat;
    // Use this for initialization
    void Start()
    {
        buffer = new ComputeBuffer(50, sizeof(float) * 2, ComputeBufferType.Default);
        balls = new List<Transform>();
        mat = transform.GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        buffer.SetData(SetArrayData(balls));
        mat.SetBuffer("_Buffer", buffer);
        mat.SetInt("_Width", balls.Count);
        if (x == 5 && y == 5) Debug.Log(transform.position + " offset " +  transform.lossyScale);           
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
    Vector2[] SetArrayData(List<Transform> list)
    {
        Vector2[] temp = new Vector2[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            temp[i] = list[i].position.XY();
        }
        return temp;
    }

    void OnDestroy()
    {
        buffer.Release();
    }
}
