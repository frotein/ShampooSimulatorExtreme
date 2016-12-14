using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressDisplayer : MonoBehaviour {

    Slider slider;
    public Detector tracking;
    public float completeValue;
    public bool completed;
    // Use this for initialization
	void Start ()
    {
        slider = transform.GetComponent<Slider>();
        completed = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!completed)
        {
            slider.value = tracking.value / completeValue;
            if (slider.value >= 1)
            { completed = true; tracking.completed = true; }
        }

    }


}
