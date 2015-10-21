using UnityEngine;
using System.Collections;

public class ObstacleColVariationScript : MonoBehaviour 
{
	Renderer sphereColo;
	float CCR; //Color code R etc...
	float CCG;
	float CCB;
	public float A;

	// Use this for initialization
	void Start () 
	{
		sphereColo = GetComponent<Renderer>(); //between 0 and 1
		CCR = Random.Range (0.25f, 0.4f);
		CCG = Random.Range (0f, 1f);
		CCB = Random.Range (0.15f, 0.25f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		sphereColo.material.SetColor("_Color", new Vector4 (1, 1, 1, A));
	}
}
