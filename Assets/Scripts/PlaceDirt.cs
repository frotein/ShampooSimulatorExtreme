using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceDirt : MonoBehaviour
{

    public int dirtOnChest = 15;
    public int averageDirtPerLimb = 4;
    public List<GameObject> dirts;
    public Transform chest;
    public List<Transform> limbs;
    List<Transform> dirtPiles;

    float radius = 0.1f;
    // Use this for initialization
    void Start ()
    {
        dirtPiles = new List<Transform>();
        PlaceDirtPilesOnChest(dirtOnChest);

        foreach(Transform limb in limbs)
            PlaceDirtOnLimb(4, limb);

        transform.GetComponent<PlayersStatus>().dirts = dirtPiles;   
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void PlaceDirtPilesOnChest(int piles)
    {
        int breakLimit = 99;
        SpriteRenderer rend = chest.GetComponent<SpriteRenderer>();
        Vector2 min = rend.bounds.min.XY();
        Vector2 max = rend.bounds.max.XY();
        int i = 0;
        
        
        GameObject newPile = null;
        while (i < piles)
        {
            GameObject dirt = dirts[Random.Range(0, dirts.Count)];
            float x = Random.Range(0.1f, 0.9f);
            float y = Random.Range(0.075f, 0.925f);
            Vector2 pos = new Vector2((max.x - min.x) * x + min.x, (max.y - min.y) * y + min.y);
            if(newPile == null)
                newPile = GameObject.Instantiate(dirt);

            bool inCorner = (x < .25f || x > .75f) && (y > .8f); 
            bool touching = false;
            Collider2D[] cols = Physics2D.OverlapCircleAll(pos, radius);
            foreach(Collider2D col in cols)
            {
                if (col.tag == "dirt")
                {
                    touching = true;
                }
            }

            if (!touching && !inCorner)
            {
                i++;
                newPile.transform.position = pos.XYZ(-.01f);
                newPile.transform.parent = chest;
                newPile.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
                dirtPiles.Add(newPile.transform);
                newPile.GetComponent<DirtPile>().localPos = new Vector2(x, y);
                breakLimit = 99;
                newPile = null;
            }
            else
            {
                breakLimit--;
            }
            if (breakLimit <= 0) { Destroy(newPile); break; }
        }
    }

    void PlaceDirtOnLimb(int piles, Transform limb)
    {
        int i = 0;
        int breakLimit = 99;
        GameObject newPile = null;
        List<Transform> storedPiles = new List<Transform>();
        Vector3 tempPos = new Vector3(999, 999,0);

        while (i < piles)
        {
            GameObject dirt = dirts[Random.Range(0, dirts.Count)];
            float y = Random.Range(-0.3f, 0.3f);
            float x = 0;//Random.Range(-0.05f, 0.05f);
            Vector2 localPos = new Vector2(x, y);
            if(newPile == null)
                newPile = GameObject.Instantiate(dirt);

            newPile.transform.parent = limb;
            newPile.transform.localPosition = localPos.XYZ(-1f);
            Vector2 pos = newPile.transform.position.XY();

            newPile.transform.position = tempPos;
            Collider2D[] cols = Physics2D.OverlapCircleAll(pos, radius);
            bool touching = false;
            foreach (Collider2D col in cols)
            {
                if (col.tag == "dirt")
                {
                    touching = true;
                }
            }

            if (!touching)
            {
                newPile.transform.position = pos.XYZ(-.01f);
                newPile.transform.localPosition = newPile.transform.localPosition.XY().XYZ(-.01f);
                newPile.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
                dirtPiles.Add(newPile.transform);
                newPile = null;                
                breakLimit = 99;
                i++;
            } 
            else
            {
                breakLimit--;
            }
            if (breakLimit <= 0) { Destroy(newPile); break; }         
        }
    }    
}
