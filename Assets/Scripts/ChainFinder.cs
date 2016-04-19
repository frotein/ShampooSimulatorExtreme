using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChainFinder : MonoBehaviour {

    bool end;
    public int index;
    public List<Transform> above, below; // the above and below chains
    public List<float> lengthsAbove, lengthsBelow; // the max lengths to each part of the chain, gotten at start where everything is the max distance from eachother
    // Use this for initialization
	void Start ()
    {
        GetChains();
        GetLengths();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GetChains()
    {
        List<Transform> tempAbove = new List<Transform>();
        List<Transform> tempBelow = new List<Transform>();
        tempAbove.Add(transform);
        tempBelow.Add(transform);
        int aboveIndex = index;
        int belowIndex = index;
        HingeJoint2D[] hinges = transform.GetComponents<HingeJoint2D>();
        if (hinges.Length < 2) end = true;
        
        bool foundEnds = false;

        while (!foundEnds)
        {
            
            List<HingeJoint2D> tempHingeList = new List<HingeJoint2D>();

            foreach (HingeJoint2D hinge in hinges)
            {
                ChainFinder chain = hinge.connectedBody.GetComponent<ChainFinder>();
                
                if (chain.index > aboveIndex) // if this chain is above in index, add it to the above array and iterate on it
                {
                    tempAbove.Add(chain.transform);
                    aboveIndex = chain.index;
                    tempHingeList.AddRange(chain.GetComponents<HingeJoint2D>());
                }

                if (chain.index < belowIndex) // if this chain is below in index, add it to the below array and iterate on it
                {
                    tempBelow.Add(chain.transform);
                    belowIndex = chain.index;
                    tempHingeList.AddRange(chain.GetComponents<HingeJoint2D>());
                }
                               
            }

            if (tempHingeList.Count == 0) // if there are no more arrays end
                foundEnds = true;
            else
            {
                hinges = tempHingeList.ToArray();
            }
        }

        above = tempAbove;
        below = tempBelow;
    }

    
    void GetLengths()
    {
        lengthsAbove = new List<float>();
        lengthsBelow = new List<float>();
        foreach(Transform link in above)
        {
            float lnth = Vector2.Distance(transform.position.XY(), link.position.XY());
            lengthsAbove.Add(lnth);
        }

        foreach (Transform link in below)
        {
            float lnth = Vector2.Distance(transform.position.XY(), link.position.XY());
            lengthsBelow.Add(lnth);
        }
    }
    // gets the chain ffrom this index to given, inclusive
    public Transform[] GetChainToIndex(int ind)
    {
        Transform[] final  = null;

        // if given index is greater get from aboveChain
        if (ind > index)
        {
            final = above.GetRange(0, ind + 1 - index).ToArray();
        }

        // if given index is lower, get from velowChain, will be done later because, if done correctly is not needed
        if(ind < index)
        {

        }


       return final;
    }

    public float GetMaxLengthToIndex(int ind)
    {
        float final = 0;

        // if given index is greater get from lengthsAbove
        if (ind > index)
        {
            final = lengthsAbove[ind - index];
        }

        // if given index is lower, get from lengthsBelow, will be done later because, if done correctly is not needed
        if (ind < index)
        {

        }


        return final;
    }
}
