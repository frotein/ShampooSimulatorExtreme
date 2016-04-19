using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WetTintController : MonoBehaviour {

    public Transform chest;
    ComputeBuffer leftLines, rightLines;
    public Vector4[] leftPoints, rightPoints;
    Material mat;
    public List<Transform> leftTransforms, rightTransforms;
    int width;
    // Use this for initialization
    void Start ()
    {
       leftPoints = new Vector4[1];
       rightPoints = new Vector4[1];
       mat = transform.GetComponent<SpriteRenderer>().material;
       leftLines = new ComputeBuffer(leftPoints.Length, sizeof(float) * 4, ComputeBufferType.Default);
       rightLines = new ComputeBuffer(rightPoints.Length, sizeof(float) * 4, ComputeBufferType.Default);
       
    }

    // puts the positions in an array to be sent, stores the radius in the z position
    void SetArrayData()
    {
        int j = 0;
        for (int i = 0; i < leftTransforms.Count; i+=2)
        {
            leftPoints[j] = new Vector4(leftTransforms[i].position.x,
                                        leftTransforms[i].position.y,
                                        leftTransforms[i + 1].position.x,
                                        leftTransforms[i + 1].position.y);
            j++;
        }
        j = 0;
        for (int i = 0; i < rightTransforms.Count; i += 2)
        {
            rightPoints[j] = new Vector4(rightTransforms[i].position.x,
                                        rightTransforms[i].position.y,
                                        rightTransforms[i + 1].position.x,
                                        rightTransforms[i + 1].position.y);
            j++;
        }
        width = j;
    }
    // Update is called once per frame
    void Update ()
    {
        SetArrayData();
        leftLines.SetData(leftPoints);
        rightLines.SetData(rightPoints);
        mat.SetBuffer("_LeftLines", leftLines);
        mat.SetBuffer("_RightLines", rightLines);
        mat.SetInt("_Width", width);
    }

    void OnDestroy()
    {
       // leftLines.Release();
       // rightLines.Release();
    }    
}
