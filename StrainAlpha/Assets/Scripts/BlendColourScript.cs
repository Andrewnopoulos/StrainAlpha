using UnityEngine;
using System.Collections;

public class BlendColourScript : MonoBehaviour {

    public bool infected;

    private Renderer myRenderer;

    private float colourBlend = 0.0f;

	// Use this for initialization
	void Start () {
        infected = false;
        myRenderer = GetComponent<Renderer>();
        colourBlend = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	    if (infected)
        {
            if (colourBlend < 1.0f)
            {
                colourBlend += Time.deltaTime;
            }
            myRenderer.material.color = new Color(colourBlend, 0, 0, 0.5f);
        }
	}
}
