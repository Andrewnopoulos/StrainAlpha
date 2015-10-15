using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public MenuScript manager;

	// Use this for initialization
	void Start () {

        manager = GameObject.Find("CanvasManager").GetComponent<MenuScript>();

	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetButton("Start"))
        {
            manager.Load("MainMenu");
            gameObject.SetActive(false);
        }

	}
}
