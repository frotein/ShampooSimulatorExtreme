using UnityEngine;
using System.Collections;

public class GenerateWetTint : MonoBehaviour {

    // Use this for initialization
    public int xSize, ySize;
    public GameObject WetTintPrefab;
    Vector2 size;
    void Start ()
    {
        size = WetTintPrefab.GetComponent<BoxCollider2D>().size;
        GenerateTintObjects(xSize, ySize);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GenerateTintObjects(int x, int y)
    {
        Vector3 rowSpace = Vector3.zero;
        for (int iY = 0; iY < y; iY++ )
        {
            rowSpace.x = 0;
            for (int iX = 0; iX < x; iX ++)
            {                
                GameObject tint = GameObject.Instantiate(WetTintPrefab);
                tint.transform.parent = transform;
                tint.transform.localPosition = rowSpace;
                tint.transform.up = transform.up;
                rowSpace.x += size.x;
            }
            rowSpace.y -= (size.y / 2);
        }
    }
}
