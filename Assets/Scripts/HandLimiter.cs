using UnityEngine;
using System.Collections;

// this script takes the grabbed objects and applies the grabbed limits on them
public class HandLimiter : MonoBehaviour
{

    public MovementLimiter handsLimiter; // dynamic limiter to limit when objects certain objects are grabbed
    public int grabbingCount; // how many hands are grabbing something with a limit
    public Transform leftHand, rightHand;
    public Transform leftGrabbed, rightGrabbed;
    Transform lastFrameLeftGrabbed, lastFrameRightGrabbed;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(handsLimiter != null)
        {
            handsLimiter.ApplyLimits();
        }

        CheckIfLimiterIsNeeded();
    }

    void LateUpdate()
    {
        
        lastFrameLeftGrabbed = leftGrabbed;
        lastFrameRightGrabbed = rightGrabbed;
    }
    void CheckIfLimiterIsNeeded()
    {
        if (leftGrabbed != null && rightGrabbed != null)
        {
            if (leftGrabbed != lastFrameLeftGrabbed || rightGrabbed != lastFrameRightGrabbed)
            {
                // if both hands are grabbing the towel
                if (leftGrabbed.gameObject.layer == LayerMask.NameToLayer("Towel") && rightGrabbed.gameObject.layer == LayerMask.NameToLayer("Towel"))
                {
                    //enact the limit on it
                    CreateTowelLimit();
                }
            }
        }
        else
        {
            if (handsLimiter != null)
            {
                foreach (Transform link in handsLimiter.chain)
                {
                    link.BroadcastMessage("SetGrabbed", false);
                }
                handsLimiter = null;
            }
        }
    }
    void CreateTowelLimit()
    {
        ChainFinder leftChain = leftGrabbed.GetComponent<ChainFinder>();
        ChainFinder rightChain = rightGrabbed.GetComponent<ChainFinder>();
        Transform[] chain;
        float lengthLimit = 0;
        if (leftChain.index > rightChain.index)
        {
            chain = rightChain.GetChainToIndex(leftChain.index);
            lengthLimit = rightChain.GetMaxLengthToIndex(leftChain.index);
        }
        else
        {
            chain = leftChain.GetChainToIndex(rightChain.index);
            lengthLimit = leftChain.GetMaxLengthToIndex(rightChain.index);
        }
        foreach(Transform link in chain)
        {
            link.BroadcastMessage("SetGrabbed", true);
        }
        MovementLimiter limiter = new MovementLimiter(leftHand, rightHand, lengthLimit, true, true, chain);
        handsLimiter = limiter;
       // Debug.Log("created towel limit");
    }
}
