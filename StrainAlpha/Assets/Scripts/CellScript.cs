﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InfectedCellState
{
    DORMANT,
    CHASINGPLAYER,
    SEARCHING
}

public enum InfectedSpecialType
{
    SPEED, // high speed
    HEALTH, // high health
    DAMAGE, // high damage
    KAMIKAZE, // speed + damage
    REPLICATION, // health + speed
    MINE // health + damage
}

public class CellFSM : FiniteStateMachine<InfectedCellState>
{ }

public class CellScript : MonoBehaviour {

    public float Chromosome1;
    public float Chromosome2;
    public float Chromosome3;
    public float Chromosome4;

    public CellFSM cellStateMachine;

    public GameObject bulletPrefab;

    public NPCManager manager;

    private float health = 5.0f;
    private float damage = 0.5f;
    private float speed = 3.5f;

    private float fireCoolDown = 0.0f;

    private float hitCooldown = 0.5f;
    private float currentHitCooldown = 0.0f;

    private float MaxSpeed = 5.0f;

    public float turnDamp = 0.05f;

    public bool infected = false;
    public bool playerDetected = false;
    public bool roaming = true;

    public bool ranged = false;

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
        playerLocation = GameObject.Find("Player").transform;
        targetLocation = transform;
        Chromosome1 = 0;
        Chromosome2 = 0;
        Chromosome3 = 0;
        Chromosome4 = 0;
	}

    void GoDormant()
    {
        targetLocation = cellPosition;
    }

    void StartSearching()
    {
        if (FollowNearestNeutral())
        {

        }
        else
        {
            cellStateMachine.Advance(InfectedCellState.DORMANT);
        }
    }

    void Chase()
    {
        targetLocation = playerLocation;
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
        myGenes = new Chromosome(4);
        playerDetected = false;
        infected = false;
        roaming = true;
        ranged = false;
        velocity = new Vector3(0, 0, 0);

        animationOffset = Random.Range(0.0f, 10.0f);
        animationSpeed = Random.Range(0.7f, 1.3f);

        rotationAxis = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        rotationAxis.Normalize();

        rotationSpeed = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);

        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        cellPosition = gameObject.transform;

        gameObject.tag = "Neutral";

        cellStateMachine = new CellFSM();

        cellStateMachine.AddTransition(InfectedCellState.DORMANT, InfectedCellState.CHASINGPLAYER, Chase);
        cellStateMachine.AddTransition(InfectedCellState.CHASINGPLAYER, InfectedCellState.SEARCHING, StartSearching);
        cellStateMachine.AddTransition(InfectedCellState.CHASINGPLAYER, InfectedCellState.CHASINGPLAYER, Chase);
        cellStateMachine.AddTransition(InfectedCellState.SEARCHING, InfectedCellState.CHASINGPLAYER, Chase);
        cellStateMachine.AddTransition(InfectedCellState.DORMANT, InfectedCellState.DORMANT, GoDormant);
        cellStateMachine.AddTransition(InfectedCellState.SEARCHING, InfectedCellState.DORMANT, GoDormant);
    }

    float distanceFrom(GameObject otherObject)
    {
        return (gameObject.transform.position - otherObject.transform.position).magnitude;
    }

    public void SetVelocityDelta(Vector3 _inputVelocityChange)
    {
        velocity += _inputVelocityChange;
    }

    void FixedUpdate()
    {

    }
	
	// Update is called once per frame
	void Update () {

        Chromosome1 = myGenes[0];
        Chromosome2 = myGenes[1];
        Chromosome3 = myGenes[2];
        Chromosome4 = myGenes[3];

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject.GetComponent<CellScript>());
        }

        if (fireCoolDown > 0)
        {
            fireCoolDown -= Time.deltaTime;
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

        // deccelerate moving thingy
        if (cellStateMachine.GetState() == InfectedCellState.DORMANT)
        {
            velocity -= velocity * Time.deltaTime;
        }

        transform.position += velocity * Time.deltaTime;

        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        currentHitCooldown -= Time.deltaTime;

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

        if (neutralCells.Count < 1)
        {
            return false;
        }

        Transform closest = neutralCells[0].GetPosition();
        Vector3 myPos = cellPosition.position;

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

        if ( (closest.position - myPos).magnitude < detectionRange * 2.3)
        {
            targetLocation = closest;
            return true;
        }
        return false;
    }

    void DormantUpdate()
    {
        if (FollowPlayer())
        {
            cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);
        }
    }

    void ChasingPlayerUpdate()
    {
        if (fireCoolDown <= 0 && ranged)
        {
            ShootBullet();
        }

        Vector3 buttstuff = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(Vector3.SmoothDamp(transform.forward, Vector3.Normalize(playerLocation.position - transform.position), ref buttstuff, turnDamp));

        Vector3 myPos = cellPosition.position;
        if ((playerLocation.position - myPos).magnitude > detectionRange * 2)
        {
            cellStateMachine.Advance(InfectedCellState.SEARCHING);
        }
    }

    void SearchingUpdate()
    {
        if (FollowPlayer())
        {
            cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);
        }
        if (!FollowNearestNeutral())
        {
            cellStateMachine.Advance(InfectedCellState.DORMANT);
        }
    }

    void InfectedUpdate()
    {
        switch(cellStateMachine.GetState())
        {
            case InfectedCellState.DORMANT:
                DormantUpdate();
                roaming = false;
                playerDetected = false;
                break;
            case InfectedCellState.CHASINGPLAYER:
                ChasingPlayerUpdate();
                playerDetected = true;
                roaming = false;
                break;
            case InfectedCellState.SEARCHING:
                SearchingUpdate();
                roaming = true;
                playerDetected = false;
                break;
        }
        
        if (targetLocation != null)
        {
            velocity += (targetLocation.position - transform.position) * speed * Time.deltaTime;
        }
        
    }

    private void ShootBullet()
    {
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
        BulletScript script = newBullet.GetComponent<BulletScript>();
        script.damage = damage;
        script.speed = 15.0f;
        script.SetAsEnemyBullet();
        fireCoolDown = fireRate;
    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;
        // particle effect
        cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);
    }

    public Chromosome GetChromosome()
    {
        return myGenes;
    }

    public void SetChromosome(Chromosome _input)
    {
        myGenes = _input;
        SetStats();
    }

    private void SetStats()
    {
        health += myGenes[0] * 50.0f;
        if (myGenes[0] >= 0.15f)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) * (myGenes[0]) * 5.0f;
        }
        damage += myGenes[1] * 1.0f;
        detectionRange += myGenes[2] * 7.0f;
        MaxSpeed += myGenes[3] * 10.0f;
        speed += myGenes[3] * 30.0f;

        if (myGenes[2] > 0.3f)
        {
            ranged = true;
        }

        Chromosome1 = myGenes[0];
        Chromosome2 = myGenes[1];
        Chromosome3 = myGenes[2];
        Chromosome4 = myGenes[3];

    }

    public void CreateInfected(Chromosome _input)
    {
        myGenes = _input;
        infected = true;

        gameObject.tag = "Enemy";

        SetStats();

        cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);

        SetBlendShapes();
    }

    public void BecomeInfected(Chromosome _input)
    {
        myGenes = Chromosome.Crossover(myGenes, _input);
        infected = true;

        gameObject.tag = "Enemy";

        SetStats();

        cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);

        SetBlendShapes();
        // other stuff for infecting the cell
    }

    private void SetBlendShapes()
    {
        skinMeshRenderer.SetBlendShapeWeight(0, myGenes[0] * 300);
        skinMeshRenderer.SetBlendShapeWeight(1, myGenes[1] * 300);
        skinMeshRenderer.SetBlendShapeWeight(2, myGenes[2] * 300);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && tag == "Neutral")
        {
            BecomeInfected(other.GetComponent<CellScript>().GetChromosome());
            manager.InfectNeutralCell(this);
        }

        if (infected && other.name == "Player" && currentHitCooldown < 0)
        {
            other.GetComponent<PlayerScript>().TakeDamage(damage);
            currentHitCooldown = hitCooldown;
        }

    }
}
