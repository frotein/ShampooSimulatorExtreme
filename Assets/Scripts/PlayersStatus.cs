using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// This script checks the various conditions the player is in to dtermine if they are dirty, wet, soapy, or clean
// if clean puts up win screen
enum Status { dirty, clean, soapy, wet };
public class PlayersStatus : MonoBehaviour {

    public Text bodyStatusText, hairStatusText, winText;
    public Color soapyColor, wetColor, cleanColor;
    public float hairDirtyness = 100;
    public List<Transform> dirts;
    public List<Transform> soaps;
    public List<Transform> shampoos;
    Status hairStatus, bodyStatus;
    bool once;
    public float bodyWetness = 0;
    public float hairWetness = 0;
    // Use this for initialization
	void Start ()
    {
        soaps = new List<Transform>();
        shampoos = new List<Transform>();
        hairStatus = Status.dirty;
        bodyStatus = Status.dirty;
        once = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetBodyStatus();
        SetHairStatus();
        if (bodyStatus == Status.clean && hairStatus == Status.clean)
            winText.gameObject.SetActive(true);
    }

    void SetBodyStatus()
    {
        if (bodyStatus == Status.dirty)
        {
            if (!Dirty())
                SetToSoapy();
        }

        if (soaps.Count >= 0 && bodyStatus != Status.dirty && bodyStatus != Status.soapy)
            SetToSoapy();

        if (soaps.Count == 0 && bodyStatus == Status.soapy)
            SetToWet();

        if (bodyStatus == Status.wet && bodyWetness <= 0)
            SetToDry();


        if (bodyStatus == Status.clean && bodyWetness > 0)
            SetToWet();
    }

    void SetHairStatus()
    {
        if(hairStatus == Status.dirty && hairDirtyness <= 0)
            SetToSoapyHair();
     

        if (hairStatus == Status.soapy && shampoos.Count == 0)
            SetToWetHair();

        if (hairStatus != Status.dirty && shampoos.Count > 0)
            SetToSoapyHair();

        if (hairStatus == Status.wet && hairWetness <= 0)
            SetToDryHair();
    }
    bool Dirty()
    {
        bool dirty = false;
        foreach (Transform dirt in dirts)
        {
            if(dirt.gameObject.activeSelf)
            {
                dirty = true;
                break;
            }
        }

        return dirty;
    }


    void SetToSoapy()
    {
        bodyStatusText.text = "Soapy";
        bodyStatusText.color = soapyColor;
        bodyStatus = Status.soapy;
    }

    void SetToSoapyHair()
    {
        hairStatusText.text = "Soapy";
        hairStatusText.color = soapyColor;
        hairStatus = Status.soapy;
    }

    void SetToWet()
    {
        bodyStatusText.text = "Wet";
        bodyStatusText.color = wetColor;
        bodyStatus = Status.wet;
    }

    void SetToWetHair()
    {
        hairStatusText.text = "Wet";
        hairStatusText.color = wetColor;
        hairStatus = Status.wet;
    }
    void SetToDry()
    {
        bodyStatusText.text = "Clean";
        bodyStatusText.color = cleanColor;
        bodyStatus = Status.clean;
    }

    void SetToDryHair()
    {
        hairStatusText.text = "Clean";
        hairStatusText.color = cleanColor;
        hairStatus = Status.clean;
    }
    public void AddToBodyWetness()
    {
        if(bodyWetness < 100)
        {
            bodyWetness += .5f * Time.deltaTime;
        }
    }

    public void AddToHairWetness()
    {
        if (hairWetness < 100)
        {
            hairWetness += 1f * Time.deltaTime;
        }
    }

    public void DryBody()
    {
        if (bodyWetness > 0)
        {
            bodyWetness -= .5f;
        }
    }

    public void DryHair()
    {
        if (hairWetness > 0)
        {
            hairWetness -= 1f;
        }
    }

    public void CleanHair()
    {
        if(hairDirtyness > 0)
        {
            hairDirtyness -= 5f * Time.deltaTime;
        }
    }
}
