using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {

    private Text countDown;

    private float baseScoreSize = 1.0f;
    private float scoreSize = 1.0f;

	// Use this for initialization
	void Start () {

        countDown = gameObject.GetComponentInChildren<Text>();

	}
	
	// Update is called once per frame
	void Update () {

        scoreSize -= (scoreSize - baseScoreSize) * Time.deltaTime * 2.0f;

        countDown.transform.localScale = new Vector3(scoreSize, scoreSize, scoreSize);

	}
    public void SetCountdown(string time)
    {
        countDown.text = time;

        //add effects
        scoreSize = 1.4f;
    }

    public string GetCountdown()
    {
        return countDown.text;
    }
}
