using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellScript : MonoBehaviour {

    public NPCManager manager;

    private float health = 5.0f;
    private float damage = 2.0f;
    private float speed = 3.5f;

    private float MaxSpeed = 5.0f;

    private bool infected = false;
    private bool playerDetected = false;

    public float detectionRange = 5.0f;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 1.0f;

    public Vector3 velocity;

    private Chromosome myGenes;

    private Transform playerLocation;

    private SkinnedMeshRenderer skinMeshRenderer;

    private float blend0 = 0;
    private float blend1 = 0;
    private float blend2 = 0;

    public float animationOffset;
    public float animationSpeed;

    private Vector3 rotationAxis;
    private float rotationSpeed;

    public float MaxAngularVelocity = 30;

    // animates to twice of maxAnimationAmplitude's value
    public float maxAnimationAmplitude = 30;

	// Use this for initialization
	void Start () {
        myGenes = new Chromosome(0.1f);
        infected = false;
        playerDetected = false;
        velocity = new Vector3(0, 0, 0);
	}

    void Awake()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        animationOffset = Random.Range(0.0f, 10.0f);
        animationSpeed = Random.Range(0.7f, 1.3f);

        rotationAxis = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        rotationAxis.Normalize();

        rotationSpeed = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);
    }

    float distanceFrom(GameObject otherObject)
    {
        return (gameObject.transform.position - otherObject.transform.position).magnitude;
    }

    public void SetVelocityDelta(Vector3 _inputVelocityChange)
    {
        velocity += _inputVelocityChange;
    }
	
	// Update is called once per frame
	void Update () {

        infected = true;

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject);
        }

        if (infected)
        {
            InfectedUpdate();

            if (velocity.magnitude >= MaxSpeed)
            {
                velocity = velocity / velocity.magnitude * MaxSpeed;
            }
        }

        transform.position += velocity * Time.deltaTime;

        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        UpdateAnimation();
	}

    void UpdateAnimation()
    {
        skinMeshRenderer.SetBlendShapeWeight(1, (Mathf.Sin(animationSpeed * Time.time + animationOffset) * maxAnimationAmplitude) + maxAnimationAmplitude);
    }

    void InfectedUpdate()
    {
        if( playerDetected || (playerLocation.position - transform.position).magnitude < detectionRange)
        {
            velocity += (playerLocation.position - transform.position) * speed * Time.deltaTime;
            playerDetected = true;
        }
        
    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;
    }

    public Chromosome GetChromosome()
    {
        return myGenes;
    }

    public void GetInfected(Chromosome _input)
    {
        myGenes = Chromosome.Crossover(myGenes, _input);
        infected = true;

        // other stuff for infecting the cell
    }
}
