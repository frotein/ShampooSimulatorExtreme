using UnityEngine;
using System.Collections;

public class ConstantPools : MonoBehaviour {

    public Transform soapBubblePool;
    public Transform wateraPool;
    public Transform shampooPool;
    // Use this for initialization
    void Start ()
    {
        Constants.pools = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
