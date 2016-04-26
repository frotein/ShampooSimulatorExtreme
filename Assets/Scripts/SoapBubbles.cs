using UnityEngine;
using System.Collections;

public class SoapBubbles : MonoBehaviour
{
    public PlayersStatus status;
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
        if(transform.lossyScale.x <= .35f && transform.lossyScale.y <= .35f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.parent = Constants.pools.soapBubblePool;
            status.soaps.Remove(transform);
            transform.gameObject.SetActive(false);
        }
    }

}
