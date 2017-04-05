using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WetTintCircleController : MonoBehaviour
{
    public Transform pointHolder;
    public bool hair;
    public Transform[] materialHolders;
    public List<Transform> positionTransforms;
    public Vector2[] positions;
    Material mat;
    public int maxWidth;
    public float[] xPos, yPos;
    public Texture tex;
    Sprite spr;
    public bool setBlock;

    List<Texture> heldTextures; // used to fix glitch with material property
    // Use this for initialization
    void Start()
    {
        positionTransforms = new List<Transform>();
        xPos = new float[400];
        yPos = new float[400];
        mat =  transform.GetComponent<SpriteRenderer>().material;
        heldTextures = new List<Texture>();
        foreach(Transform t in materialHolders)
        {
            heldTextures.Add(t.GetComponent<SpriteRenderer>().sprite.texture);
        }
        spr = transform.GetComponent<SpriteRenderer>().sprite;
        tex = spr.texture;
        positions = new Vector2[maxWidth];
        Constants.player.wetTintControllers.Add(this);
    }

    void SetArrayData()
    {
        
        for(int i = 0; i < 400; i++)
        {
            if(i < positionTransforms.Count && i < maxWidth)
            {
                xPos[i] = positionTransforms[i].position.x;
                yPos[i] = positionTransforms[i].position.y;
            }
            else
            {
                xPos[i] = 0;
                yPos[i] = 0;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
     
        SetArrayData();
        //buffer.SetData(positions);
        var materialProperty = new MaterialPropertyBlock();

        
        materialProperty.SetFloat("_Width", (float)Mathf.Min(maxWidth, positionTransforms.Count));
        materialProperty.SetFloatArray("xPos", xPos);
        materialProperty.SetFloatArray("yPos", yPos);
        gameObject.GetComponent<SpriteRenderer>().SetPropertyBlock(materialProperty);
        int i = 0;
        foreach (Transform t in materialHolders)
        {
            t.GetComponent<SpriteRenderer>().SetPropertyBlock(materialProperty);
            t.GetComponent<SpriteRenderer>().material.mainTexture = heldTextures[i];
            i++;
        }
        mat.mainTexture = tex;
        //mat.SetBuffer("_Positions", buffer);
        //mat.SetInt("_Width", Mathf.Min(maxWidth, positionTransforms.Count));

    }

    void OnDestroy()
    {
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