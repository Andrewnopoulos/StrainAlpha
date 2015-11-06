using UnityEngine;
using System.Collections;

public class creditsscroll : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position += new Vector3(0, 30.0f * Time.deltaTime, 0);

	}
}
