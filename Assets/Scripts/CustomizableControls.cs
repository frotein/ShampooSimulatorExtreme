using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CustomizableControls
{
    public int player;
    public Dictionary<string, CustomizableControl> controlsDictionary;
    public CustomizableControls(int playr) // set the default control for a specific player
    {
        controlsDictionary = new Dictionary<string, CustomizableControl>();
        player = playr;
    }
}

public class CustomizableControl
{
    public KeyCode key; // the key pressed to trigger the action
    public bool controller; // is a controler used for this action
    public float sensitivity; // the sensitivity of the action
    public string name; // the name of the axis if a controller input is used

    // set the key and sensitivity for a Computer Key
    public CustomizableControl(KeyCode k, float val)
    {
        controller = false;
        key = k;
        sensitivity = val;
    }
    // set the axis name and the sensitivity
    public CustomizableControl(string name, float val)
    {
        controller = true;
    }
    public bool WasClicked()
    {
        bool clicked = false;

        if (!controller)
        {
            if (Input.GetKeyDown(key))
                clicked = true;
        }
        else
        {
            if (Input.GetButtonDown(name))
                clicked = true;
        }

        return clicked;
    }
}