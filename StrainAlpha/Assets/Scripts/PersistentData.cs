using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

    //public variables to exist through scenes
    public float levelTime;
    public float totalTime;
    public float levelKills;
    public float totalKills;

	// Use this for initialization
	void Awake () {

        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
