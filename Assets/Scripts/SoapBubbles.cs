using UnityEngine;
using System.Collections;

public class SoapBubbles : MonoBehaviour
{

    public float shrinkRate;
    float shrinkDTime;
    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        shrinkDTime = shrinkRate * Time.deltaTime;

        
        
    }

    public void Shrink()
    {
        transform.localScale -= new Vector3(shrinkDTime, shrinkDTime, 0);
        if(transform.localScale.x <= 0 && transform.localScale.y <= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.parent = Constants.pools.soapBubblePool;
            transform.gameObject.SetActive(false);
        }
    }

}
