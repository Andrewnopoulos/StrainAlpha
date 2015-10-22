using UnityEngine;
using System.Collections;

public class SoundDeleteAfterPlay : MonoBehaviour {
	AudioSource sound;
	float timer;

	// Use this for initialization
	void Start () 
	{
		sound = GetComponent<AudioSource> ();
		sound.Play ();
		timer = sound.clip.length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		Debug.Log (timer);
		if (timer <= -0.5f) 
		{
			Destroy(gameObject);
		}
	}
}
