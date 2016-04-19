using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WetTintCircleController : MonoBehaviour
{
    public Transform pointHolder;
    public Transform[] materialHolders;
    public List<Transform> positionTransforms;
    public Vector2[] positions;
    ComputeBuffer buffer;
    Material mat;
    public int maxWidth;
    // Use this for initialization
    void Start()
    {
        positionTransforms = new List<Transform>();
        
        mat =  transform.GetComponent<SpriteRenderer>().material;
        foreach(Transform t in materialHolders)
        {
            t.GetComponent<SpriteRenderer>().material = mat;
        }
        
        positions = new Vector2[maxWidth];
        buffer = new ComputeBuffer(maxWidth, sizeof(float) * 2, ComputeBufferType.Default);
    }

    void SetArrayData()
    {
        int i = 0;
        foreach (Transform t in positionTransforms)
        {
            positions[i] = t.position;
            i++;
            if (i > maxWidth - 1) break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        SetArrayData();
        buffer.SetData(positions);
        
        mat.SetBuffer("_Positions", buffer);
        mat.SetInt("_Width", Mathf.Min(maxWidth, positionTransforms.Count));
        
    }

    void OnDestroy()
    {
        buffer.Release();
    }

    public bool AtMax()
    {
        return positionTransforms.Count >= maxWidth;
    }

    public void AddToPoints(Transform t)
    {
        positionTransforms.Add(t);
    }
}