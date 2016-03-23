﻿using UnityEngine;
using System.Collections;

public class HandCloser : MonoBehaviour {

    // the sprites for a open, grabbing and closed hand
    public Sprite open, holding, closed;

    // the gameobjects for the finger sprites that will go over objects
    public GameObject grabFingers, closedFingers; 
    // the new local positions for grabbing and closing hand so it looks natural
    public Vector3 grabbingPosition, closedPosition;
    Vector3 openPosition;
    // is this the left hand?
    public bool left;

    // is true if we switched to grabbing this frame 
    bool grabbingThisFrame;

    // is true if we switched to grabbing this frame 
    bool closingThisFrame;

    // is true if we switched to grabbing this frame 
    bool openingThisFrame;

    // the current and last sprites, used to transition and so sprites arent set every frame
    int currentSprite;
    int lastSprite;

    // the sprite Renderer
    SpriteRenderer rend;

    // Use this for initialization
	void Start ()
    {
        rend = transform.GetComponent<SpriteRenderer>();
        openPosition = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float grabAmt;
        // switches input based on hand side hand it is
        if (left)
            grabAmt = Input.GetAxis("LeftHand");
        else
            grabAmt = Input.GetAxis("RightHand");

        // if we dont have enoguh force to grab, set it to open hand sprite
        if (grabAmt < Constants.player.grabAmount)
            currentSprite = 0;
        else
        {
            // if we have enough to grab but not to close, set it to grab hand sprite
            if (grabAmt < Constants.player.closeAmount)
                currentSprite = 1;
            else
            {
                // if we have enough force to close, set it to closed hand sprite;
                currentSprite = 2;
            }
        }

        // if this sprite is different than last frames, switch the sprite image
        if (currentSprite != lastSprite)
            switchSprites(currentSprite);
        lastSprite = currentSprite;      
    }

    // returns true if the hand is 
    public bool isGrabbing()
    {
        return currentSprite == 1;
    }

    // returns if we grabbed this frame, is function so it cant be edited
    public bool grabbedThisFrame()
    {
        return grabbingThisFrame;
    }

    // returns if we closed the hand this frame, is function so it cant be edited
    public bool closedThisFrame()
    {
        return closingThisFrame;
    }

    // returns if we opened the hand this frame, is function so it cant be edited
    public bool openedThisFrame()
    {
        return openingThisFrame;
    }

    // switches the sprites and moves the tranform slightly, 0 is closed, 1 is grabbing, 2 is closed
    void switchSprites(int newSprite)
    {
        openingThisFrame = false;
        grabbingThisFrame = false;
        closingThisFrame = false;

        if (newSprite == 0)
        {
            rend.sprite = open;
            openingThisFrame = true;
            grabFingers.SetActive(false);
            closedFingers.SetActive(false);
            transform.localPosition = openPosition;
        }
        
        else
        {
            if (newSprite == 1)
            {
                rend.sprite = holding;
                transform.localPosition = grabbingPosition;
                grabFingers.SetActive(true);
                closedFingers.SetActive(false);
                grabbingThisFrame = true;
            }
            else
            {
                if(newSprite == 2)
                {
                    rend.sprite = closed;
                    transform.localPosition = closedPosition;
                    closingThisFrame = true;
                    grabFingers.SetActive(false);
                    closedFingers.SetActive(true);
                }
            }
                
        }
    }
}