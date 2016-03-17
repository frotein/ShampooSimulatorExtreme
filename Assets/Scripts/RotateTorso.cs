using UnityEngine;
using System.Collections;

public class RotateTorso : MonoBehaviour {

    // Use this for initialization
    public float leanRate;
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float leanDir = Input.GetAxisRaw("Lean");
        transform.eulerAngles += new Vector3(0, 0, leanRate * leanDir * Time.deltaTime);
     //   Debug.Log(transform.localEulerAngles);
	}
}
