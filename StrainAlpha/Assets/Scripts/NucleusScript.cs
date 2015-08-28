using UnityEngine;
using System.Collections;

public class NucleusScript : MonoBehaviour {

    private Chromosome myGenes;

    public PlayerScript player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public NucleusScript(Chromosome initialGenes)
    {
        myGenes = initialGenes;
    }
}
