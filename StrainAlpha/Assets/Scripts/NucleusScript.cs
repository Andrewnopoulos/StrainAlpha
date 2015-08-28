using UnityEngine;
using System.Collections;

public class NucleusScript : MonoBehaviour {

    public float magnetismRadius = 100.0f;
    public float acceleration = 10.0f;
    public float maxVelocity = 10.0f;

    private Vector3 velocity;
    
    private Chromosome myGenes;

    public PlayerScript player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	    if (Vector3.Distance(player.transform.position, transform.position) < magnetismRadius)
        {
            Vector3 vectorToPlayer = player.transform.position - transform.position;

            velocity += (vectorToPlayer / vectorToPlayer.magnitude) * Time.deltaTime * acceleration;
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
