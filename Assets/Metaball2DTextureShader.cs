using UnityEngine;
using System.Collections;

public class Metaball2DTextureShader : MonoBehaviour {

    public float test;
    public Vector2[] testArray;
    ComputeBuffer buffer;
    Material mat;
   

    // Use this for initialization
	void Start () 
    {
        mat = transform.GetComponent<MeshRenderer>().material;
        buffer = new ComputeBuffer(testArray.Length, sizeof(float) * 2, ComputeBufferType.Default);
    }
	
	// Update is called once per frame
	void Update ()
    {
        buffer.SetData(testArray);
        //mat.SetFloat("_Cutoff", test);
        mat.SetBuffer("_BufferX", buffer);
    }

    void OnDestroy()
    {
        buffer.Release();
    }

 }
