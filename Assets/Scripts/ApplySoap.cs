using UnityEngine;
using System.Collections;

public class ApplySoap : MonoBehaviour {

    public PlayersStatus status;
    public Transform soapPool;
    public bool grabbed;
    Transform playerT;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if we are grabbing the soap
        if (grabbed)
        {
            // check what colliders we are overlapping, if we are on the player and not ontop of a soap bubble, apply soap bubbles
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position.XY());
            float playerZ = 999;
            bool applySoap = false;
            foreach (Collider2D col in cols)
            {
                if (col.transform.tag == "Player")
                {
                    

                    // if the players Z is less than the last one (is closer to camera) apply soap to that part of the player
                    if (col.transform.position.z < playerZ)
                    {
                        applySoap = true;
                        playerT = col.transform;
                        playerZ = playerT.position.z;                      
                    }
                }

                if (col.transform.tag == "Soap Bubbles")
                {
                    // if the soap bubbles are closer to the camera than the player, dont apply soap as their is already soap
                    if (col.transform.position.z < playerZ)
                    {
                        applySoap = false;
                        playerZ = col.transform.position.z;
                    }
               }

            }

            if (applySoap)
                ApplySoapBubbles();
        }
    }

    void ApplySoapBubbles()
    {
        if(soapPool.childCount > 0)
        {
            Transform soap = soapPool.GetChild(0);
            soap.gameObject.SetActive(true);
            soap.position = transform.position.XY().XYZ(playerT.position.z - .02f);
            soap.parent = playerT;
            Constants.pools.soapBubblePool.GetComponent<AppliedSoapBubbles>().appliedSoapColliders.Add(soap.GetComponent<Collider2D>());
            soap.GetComponent<SoapBubbles>().status = status;
            status.soaps.Add(soap);
        }
    }
}
