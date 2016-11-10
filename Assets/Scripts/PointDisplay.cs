using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointDisplay : MonoBehaviour {

    float fadeTime;
    float startFade;
    float startTime;
    float upSpeed;
    Text text;
    // Use this for initialization
	void Start ()
    {
        
    }

    public void Setup(float fade, string message, float score, float startF = 2,float upSpeed = 5)
    {
        startTime = Time.time;
        fadeTime = fade;
        startFade = startF;
        this.upSpeed = upSpeed;
        text = transform.GetComponent<Text>();
        text.text = "+" + score + "\n" + message;

    }
	// Update is called once per frame
	void Update ()
    {
        float tim = Time.time - startTime;
        if (tim > startFade)
        {
            
            float t = 1 - ((tim - startFade) / fadeTime);
            Color c = text.color;
            Color newColor = new Color(c.r, c.g, c.b, t);
            text.color = newColor;
            if (tim > startFade + fadeTime) Destroy(gameObject);
        }

        

        transform.position += new Vector3(0, Time.deltaTime * upSpeed,0);
	}
}
