using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

    private List<string> helpMessages;

    private float timeUntilNextMessage;
    private float messageDelay = 1.0f;

    private Text message;
    private int currentMessage = 0;

    void Awake()
    {
        helpMessages = new List<string>();

        helpMessages.Add("Welcome to Strain, press the 'A' button to continue...");
        helpMessages.Add("Use the left stick to move the nanobot...");
        helpMessages.Add("Use the right stick to aim and shoot...");
        helpMessages.Add("Shoot enemies to destroy them. When enemies die they drop their core...");
        helpMessages.Add("Collect these cores to gain points in your personal gene levels...");

        message = gameObject.GetComponentInChildren<Text>();
    }

	void Start () {

        message.text = helpMessages[currentMessage];

	}
	
	void Update () {
	
        timeUntilNextMessage -= Time.deltaTime;

        if (Input.GetButtonDown("A") && timeUntilNextMessage <= 0)
        {
            timeUntilNextMessage = messageDelay;
            currentMessage++;
            message.text = "";
        }

        if (timeUntilNextMessage <= 0 && currentMessage < helpMessages.Count)
        {
            message.text = helpMessages[currentMessage];
        }

	}
}
