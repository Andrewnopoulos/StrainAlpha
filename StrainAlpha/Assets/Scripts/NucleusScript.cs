using UnityEngine;
using System.Collections;

public class NucleusScript : MonoBehaviour {

    public float magnetismRadius = 100.0f;
    public float acceleration = 10.0f;
    public float maxVelocity = 10.0f;

    private bool attracted;

    private Vector3 velocity;
    
    private Chromosome myGenes;

    private Transform playerLocation;

	// Use this for initialization
	void Start () {
        playerLocation = GameObject.Find("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

	    if (Vector3.Distance(playerLocation.position, transform.position) < magnetismRadius || attracted)
        {
            Vector3 vectorToPlayer = playerLocation.position - transform.position;

            velocity += (vectorToPlayer / vectorToPlayer.magnitude) * Time.deltaTime * acceleration;

            attracted = true;
        }

        if (velocity.magnitude > maxVelocity)
        {
            velocity /= velocity.magnitude;
            velocity *= maxVelocity;
        }

        transform.position += velocity * Time.deltaTime;

	}

    public void SetChromosome(Chromosome input)
    {
        myGenes = input;
    }

    public Chromosome GetChromosome()
    {
        return myGenes;
    }
}
