using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointDisplayManager : MonoBehaviour {


    public float totalScore;
    public GameObject textPrefab;
    public Text scoreText;
    public static PointDisplayManager instance;
    public float fadeTime;
    public float pointsPerBounce;
    public float distancePointsScaler;
    public float spinningPointsScaler;
    // Use this for initialization
	void Start ()
    {
        instance = this;
 	}
	
	// Update is called once per frame
	void Update ()
    {
        scoreText.text = "Score: " + totalScore;

        if (Input.GetMouseButtonDown(0))
            SpawnPoints("Mouse Test", 200, Camera.main.ScreenToWorldPoint(Input.mousePosition));

	}


    public void SpawnPoints(string message, float points, Vector3 position)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        GameObject textP = GameObject.Instantiate(textPrefab);
        textP.GetComponent<PointDisplay>().Setup(fadeTime, message, points);
        textP.transform.parent = transform;
        textP.transform.position = screenPos;
        totalScore += points;
    }

    string CatchMessages(int bounces, float degreesSpun, float lengthTraveled)
    {
        string final = "";
        if (bounces > 0)
        {
            if (bounces == 2)
                final += "double ";

            if (bounces == 3)
                final += "triple ";

            if (bounces == 4)
                final += "quadruple ";

            if (bounces >= 5 && bounces < 10)
                final += "mega ";

            if (bounces > 10)
                final += "ultra ";

            final += "bounce ";
        }

        if (lengthTraveled > 25f)
            final += "long ";
        else
        {
            if (lengthTraveled < 3) final += "short ";
        }
        final += "Catch";
        return final;
    }

    float CatchPoints(int bounces, float degressSpun, float lengthTraveled)
    {
        float total = 0;
        total += bounces * pointsPerBounce;
        // total += degressSpun 
        total += lengthTraveled * distancePointsScaler;
        total = Mathf.Round(total);
        return total;
    }

    public void CatchResponse(int bounces, float degreesSpun, float lengthTraveled,Vector3 position)
    {
        string message = CatchMessages(bounces, degreesSpun, lengthTraveled);
        float points = CatchPoints(bounces, degreesSpun, lengthTraveled);

        if (points > 0)
            SpawnPoints(message, points, position);
    }
}
