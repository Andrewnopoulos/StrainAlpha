using UnityEngine;
using System.Collections;

public class BlendColourScript : MonoBehaviour {

    public bool infected;

    private Renderer myRenderer;

	// Use this for initialization
	void Start () {
        infected = false;
        myRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (infected)
        {
            myRenderer.material.color = new Color(1, 0, 0, 0.5f);
        }
	}
}
