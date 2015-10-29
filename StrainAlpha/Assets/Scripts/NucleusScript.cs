using UnityEngine;
using System.Collections;

public class NucleusScript : MonoBehaviour {

    public float magnetismRadius = 100.0f;
    public float acceleration = 7.0f;
    public float maxVelocity = 10.0f;

    private bool attracted;

    private Vector3 velocity;
    
    private Chromosome myGenes;

    private Transform playerLocation;

    public bool attachedToCell = true;

    private Renderer[] renderers;

    public Color healthColour;
    public Color damageColour;
    public Color rangedColour;
    public Color speedyColour;

	// Use this for initialization
	void Start () {
        playerLocation = GameObject.Find("Player").GetComponent<Transform>();
	}

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
	
    public void SetVelocity(Vector3 _inputVel)
    {
        velocity = _inputVel;
    }

	// Update is called once per frame
	void Update () {

        if (!attachedToCell)
        {
            if (Vector3.Distance(playerLocation.position, transform.position) < magnetismRadius || attracted)
            {
                //Vector3 vectorToPlayer = playerLocation.position - transform.position;

                //velocity += (vectorToPlayer / vectorToPlayer.magnitude) * Time.deltaTime * acceleration;

                Vector3 toPlayer = playerLocation.position - transform.position;
                transform.position = transform.position + (Time.deltaTime * acceleration * (toPlayer) / toPlayer.sqrMagnitude);

                attracted = true;
            }
            else
            {
                // deccelerate
                velocity -= Time.deltaTime * velocity;
            }

            // normalize velocity
            if (velocity.magnitude > maxVelocity)
            {
                velocity /= velocity.magnitude;
                velocity *= maxVelocity;
            }

            // continue moving
            if (!attracted)
            {
                transform.position += velocity * Time.deltaTime;
            }
        }
	}

    public void SetChromosome(Chromosome input)
    {
        myGenes = input;

        int[] best = {-1, -1};
        float[] vals = { 0.0f, 0.0f };

        // do for each gene
        for (int i = 0; i < myGenes.Length(); i++)
        {
            // if there's a new best
            if (myGenes[i] > vals[0])
            {
                // change best to second best
                best[1] = best[0];
                vals[1] = vals[0];

                // update best
                vals[0] = myGenes[i];
                best[0] = i;
            } else if (myGenes[i] > vals[1])
            {
                // update second best
                vals[1] = myGenes[i];
                best[1] = i;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            switch(best[i])
            {
                case 0:
                    renderers[i].material.color = healthColour;
                    break;

                case 1:
                    renderers[i].material.color = damageColour;
                    break;

                case 2:
                    renderers[i].material.color = rangedColour;
                    break;

                case 3:
                    renderers[i].material.color = speedyColour;
                    break;
            }
        }
    }

    public Chromosome GetChromosome()
    {
        return myGenes;
    }
}
