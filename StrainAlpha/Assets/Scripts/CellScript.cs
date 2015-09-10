using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InfectedCellState
{
    DORMANT,
    CHASINGPLAYER,
    SEARCHING
}

public class CellScript : MonoBehaviour {

    public NPCManager manager;

    private float health = 5.0f;
    private float damage = 2.0f;
    private float speed = 3.5f;

    private float MaxSpeed = 5.0f;

    public bool infected = false;
    public bool playerDetected = false;
    private bool roaming = true;

    public float detectionRange = 5.0f;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 1.0f;

    public Vector3 velocity;

    private Chromosome myGenes;

    public Transform targetLocation;
    public Transform cellPosition;

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
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        targetLocation = transform;
	}

    public Transform GetPosition()
    {
        return cellPosition;
    }

    public Transform GetTargetLocation()
    {
        return targetLocation;
    }

    public void SetTargetLocation(Transform _inputLocation)
    {
        targetLocation = _inputLocation;
    }

    void Awake()
    {
        myGenes = new Chromosome(0.04f);
        playerDetected = false;
        infected = false;
        roaming = true;
        velocity = new Vector3(0, 0, 0);

        animationOffset = Random.Range(0.0f, 10.0f);
        animationSpeed = Random.Range(0.7f, 1.3f);

        rotationAxis = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        rotationAxis.Normalize();

        rotationSpeed = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);

        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        cellPosition = gameObject.transform;

        gameObject.tag = "Neutral";
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

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject.GetComponent<CellScript>());
        }

        if (infected)
        {
            InfectedUpdate();

            if (velocity.magnitude >= MaxSpeed)
            {
                velocity = velocity / velocity.magnitude * MaxSpeed;
            }
            UpdateAnimation();
        }

        transform.position += velocity * Time.deltaTime;

        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

	}

    void UpdateAnimation()
    {
   //     skinMeshRenderer.SetBlendShapeWeight(1, (Mathf.Sin(animationSpeed * Time.time + animationOffset) * maxAnimationAmplitude) + maxAnimationAmplitude);
    }

    bool FollowPlayer()
    {
        Vector3 myPos = cellPosition.position;
        if ((playerLocation.position - myPos).magnitude < detectionRange)
        {
            targetLocation = playerLocation;
            return true;
        }
        
        return false;
    }

    bool FollowNearestNeutral()
    {
        List<CellScript> neutralCells = manager.GetNeutralCells();

        Transform closest = playerLocation;
        Vector3 myPos = cellPosition.position;

        if (neutralCells.Count < 1)
        {
            return false;
        }

        foreach (CellScript neutral in neutralCells)
        {
            float newDistance = (neutral.GetPosition().position - myPos).magnitude;
            float oldDistance = (closest.position - myPos).magnitude;

            // if the distance you're looking at is closer than the previously looked at position
            if (newDistance < oldDistance)
            {
                closest = neutral.GetPosition();
            }
        }

        if ( (closest.position - myPos).magnitude < detectionRange)
        {
            targetLocation = closest;
            return true;
        }
        return false;
    }

    void Roam()
    {

    }

    void InfectedUpdate()
    {
        if (FollowPlayer())
        {
            playerDetected = true;
            roaming = false;
        }
        else if (FollowNearestNeutral())
        {
            playerDetected = false;
            roaming = false;
        }
        else
        {
            playerDetected = false;
        }
        
        velocity += (targetLocation.position - transform.position) * speed * Time.deltaTime;

        //if( playerDetected || (targetLocation.position - transform.position).magnitude < detectionRange)
        //{
        //    velocity += (targetLocation.position - transform.position) * speed * Time.deltaTime;
        //    playerDetected = true;
        //}
    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;
    }

    public Chromosome GetChromosome()
    {
        return myGenes;
    }

    public void SetChromosome(Chromosome _input)
    {
        myGenes = _input;
    }

    public void CreateInfected(Chromosome _input)
    {
        myGenes = _input;
        infected = true;

        gameObject.tag = "Enemy";

        SetBlendShapes();
    }

    public void BecomeInfected(Chromosome _input)
    {
        myGenes = Chromosome.Crossover(myGenes, _input);
        infected = true;

        gameObject.tag = "Enemy";

        SetBlendShapes();
        // other stuff for infecting the cell
    }

    private void SetBlendShapes()
    {
        skinMeshRenderer.SetBlendShapeWeight(0, myGenes[0] * 100);
        skinMeshRenderer.SetBlendShapeWeight(1, myGenes[1] * 100);
        skinMeshRenderer.SetBlendShapeWeight(2, myGenes[2] * 100);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && tag == "Neutral")
        {
            BecomeInfected(other.GetComponent<CellScript>().GetChromosome());
            manager.InfectNeutralCell(this);
        }
    }
}
