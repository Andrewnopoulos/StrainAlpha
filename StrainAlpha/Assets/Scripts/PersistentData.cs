using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {

    //public variables to exist through scenes
    public LevelSelection level;
    public DifficultySelection difficulty;

	// Use this for initialization
	void Awake () {

        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
