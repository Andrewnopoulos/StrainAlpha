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
        helpMessages.Add("Your gene levels are represented as the bars on the inner ring around the nanobot...");
        helpMessages.Add("These gene levels passively grant bonuses to the player, but can be activated to provide short term powerful effects...");
        helpMessages.Add("Switch which gene is selected with the right and left bumpers...");
        helpMessages.Add("Your nanobot's health is represnted by the outer blue ring...");
        helpMessages.Add("If it becomes empty you will die, so be careful...");
        helpMessages.Add("Survive until the time runs out to fight the boss...");
        helpMessages.Add("Each level has a unique boss. Kill the boss to move on to the next level...");

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
