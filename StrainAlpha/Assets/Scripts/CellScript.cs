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
    REGULAR, // none of the below
    SPEED, // high speed
    HEALTH, // high health
    DAMAGE, // high damage
    KAMIKAZE, // speed + damage
    REPLICATION, // health + speed
    MINE, // health + damage
    RANGED // high ranged
}

public class CellFSM : FiniteStateMachine<InfectedCellState>
{ }

public class CellScript : MonoBehaviour {

    public CellFSM cellStateMachine;

    public GameObject bulletPrefab;

    public NPCManager manager;

    private BlendColourScript blendShapeChild;

    private NucleusScript childNucleus;

    public InfectedSpecialType infectedType = InfectedSpecialType.REGULAR;

    public float ScalingHealth = 5.0f;
    public float ScalingDamage = 1.0f;
    public float ScalingRanged = 5.0f;
    public float ScalingMaxSpeed = 3.0f;
    public float ScalingAcceleration = 5.0f;

    public float health = 5.0f;
    private float damage = 0.5f;
    private float speed = 3.5f;

    private float fireCoolDown = 0.0f;

    private float hitCooldown = 0.5f;
    private float currentHitCooldown = 0.0f;

    public int scoreWorth = 100;

    private float MaxSpeed = 5.0f;

    public float turnDamp = 0.05f;

    public float geneTriggerValue = 0.4f;

    private float replicationCountdown = 10.0f;
    public float MaxReplicationTime = 10.0f;

    private float kamikazeTimer = 0.0f;

    public float kamikazeLifetime = 3.0f;

    public float infectedTimerScale = 0.0f;

    public bool infected = false;
    public bool playerDetected = false;
    public bool roaming = true;

    public bool ranged = false;

    public float detectionRange = 8.0f;

    private bool gravitating = false;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 2.0f;

    public Vector3 velocity;

    private Chromosome myGenes;

    public Transform targetLocation;
    public Transform cellPosition;

    private Transform playerLocation;
    private Transform futureLocation;

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

    public bool beAbsorbed = false;

    private bool disabled = false;
    public float disableDuration = 2.0f;
    private float disableTimer = 0.0f;

    public float healthChromosome = 0.0f;
    public float damageChromosome = 0.0f;
    public float rangedChromosome = 0.0f;
    public float speedChromosome = 0.0f;

	// Use this for initialization
	void Start () {
        playerLocation = GameObject.Find("Player").transform;
        futureLocation = GameObject.Find("Player").GetComponent<PlayerScript>().PlayerFutureTransform();
        
        targetLocation = transform;
	}

    void GoDormant()
    {
        targetLocation = cellPosition;
    }

    public void SetGravitating(bool _gravitating)
    {
        gravitating = _gravitating;
    }

    public void ApplyForce(Vector3 _force)
    {
        velocity += _force * Time.deltaTime;
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

    public void SetDisabled()
    {
        disabled = true;
        disableTimer = disableDuration;
    }

    void Chase()
    {
        targetLocation = playerLocation;
        if (ranged)
        {
            fireCoolDown = Random.Range(0, fireRate);
            targetLocation = futureLocation;
        }

        if (infectedType == InfectedSpecialType.KAMIKAZE)
        {
            SetKamikaze();
        }

        if (infectedType == InfectedSpecialType.MINE)
        {
            SetMine();
        }
    }

    void SetKamikaze()
    {
        kamikazeTimer = kamikazeLifetime;
        MaxSpeed *= 2.4f;
        speed *= 0.75f;
        health *= 0.3f;
        detectionRange *= 2.5f;
    }

    void KamikazeUpdate()
    {
        kamikazeTimer -= Time.deltaTime;

        if (kamikazeTimer < 0)
        {
            manager.AddToKillList(this);
        }
    }

    void SetMine()
    {
        MaxSpeed = 0.0f;
        speed = 0.0f;
        detectionRange = 3.0f;
    }

    void MineUpdate()
    {
        if ((playerLocation.position - cellPosition.position).magnitude < detectionRange)
        {
            manager.AddToKillList(this);
        }
    }

    public Transform GetPosition()
    {
        return cellPosition;
    }

    public Vector3 GetCurrentLocation()
    {
        return cellPosition.position;
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

        kamikazeTimer = 0.0f;

        animationOffset = Random.Range(0.0f, 10.0f);
        animationSpeed = Random.Range(0.7f, 1.3f);

        infectedTimerScale = 0.0f;

        rotationAxis = new Vector3(0, 1, 0);

        rotationSpeed = Random.Range(-MaxAngularVelocity, MaxAngularVelocity);

        cellPosition = gameObject.transform;

        gameObject.tag = "Neutral";

        infectedType = InfectedSpecialType.REGULAR;

        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        skinMeshRenderer.SetBlendShapeWeight(0, 100);

        cellStateMachine = new CellFSM();

        cellStateMachine.AddTransition(InfectedCellState.DORMANT, InfectedCellState.CHASINGPLAYER, Chase);
        cellStateMachine.AddTransition(InfectedCellState.CHASINGPLAYER, InfectedCellState.SEARCHING, StartSearching);
        cellStateMachine.AddTransition(InfectedCellState.CHASINGPLAYER, InfectedCellState.CHASINGPLAYER, Chase);
        cellStateMachine.AddTransition(InfectedCellState.SEARCHING, InfectedCellState.CHASINGPLAYER, Chase);
        cellStateMachine.AddTransition(InfectedCellState.DORMANT, InfectedCellState.DORMANT, GoDormant);
        cellStateMachine.AddTransition(InfectedCellState.SEARCHING, InfectedCellState.DORMANT, GoDormant);

        blendShapeChild = GetComponentInChildren<BlendColourScript>();

        childNucleus = GetComponentInChildren<NucleusScript>();

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

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject.GetComponent<CellScript>());
        }

        if (fireCoolDown > 0)
        {
            fireCoolDown -= Time.deltaTime;
        }

        if (disableTimer > 0)
        {
            disableTimer -= Time.deltaTime;
            if (disableTimer < 0)
            {
                disabled = false;
            }
        }

        if (infected)
        {
            if (!disabled)
            {
                InfectedUpdate();
                UpdateAnimation();
            }

            if (velocity.magnitude >= MaxSpeed && velocity.magnitude != 0.0f && !gravitating)
            {
                velocity = velocity / velocity.magnitude * MaxSpeed;
            }
        }
        else
        {

        }

        // deccelerate moving thingy
        if (cellStateMachine.GetState() == InfectedCellState.DORMANT)
        {
            velocity -= velocity * Time.deltaTime;
        }

        transform.position += velocity * Time.deltaTime;

        //transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        currentHitCooldown -= Time.deltaTime;

	}

    void UpdateAnimation()
    {
        skinMeshRenderer.SetBlendShapeWeight(3, infectedTimerScale * ((Mathf.Sin(animationSpeed * Time.time + animationOffset) * maxAnimationAmplitude) + maxAnimationAmplitude));
    }

    bool FollowPlayer()
    {
        Vector3 myPos = cellPosition.position;
        if ((playerLocation.position - myPos).magnitude < detectionRange * 2)
        {
            targetLocation = playerLocation;
            if (ranged)
            {
                targetLocation = futureLocation;
            }
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

        if ( (closest.position - myPos).magnitude < detectionRange * 2.5f)
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

        if (infectedType == InfectedSpecialType.REPLICATION)
        {
            ReplicationUpdate();
        }

    }

    void ReplicationUpdate()
    {
        replicationCountdown -= Time.deltaTime;
        if (replicationCountdown < 0)
        {
            SpawnCopy();
        }
    }

    // will mutate both the child and itself when it spawns a child
    void SpawnCopy()
    {
        Chromosome ChildChromosome = myGenes;
        ChildChromosome[0] *= 0.1f;
        ChildChromosome.Mutate();
        manager.CreateInfectedCell(ChildChromosome, transform.position);
        replicationCountdown = Random.Range(MaxReplicationTime - 1.0f, MaxReplicationTime + 1.0f);
        myGenes.Mutate();
        SetStats();
    }

    void LookToPlayer()
    {
        Vector3 buttstuff = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(Vector3.SmoothDamp(transform.forward, 
            Vector3.Normalize(playerLocation.position - transform.position), 
            ref buttstuff, turnDamp));
    }

    void SpeedUpdate()
    {
        Vector3 buttstuff = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(Vector3.SmoothDamp(transform.forward,
            -velocity,
            ref buttstuff, turnDamp));
    }

    void ChasingPlayerUpdate()
    {
        if (infectedType == InfectedSpecialType.KAMIKAZE)
        {
            KamikazeUpdate();
            return;
        }

        if (infectedType == InfectedSpecialType.SPEED)
        {
            SpeedUpdate();
        }

        if (infectedType == InfectedSpecialType.MINE)
        {
            MineUpdate();
            return;
        }

        if (ranged)
        {
            if (!gravitating)
            {
                LookToPlayer();
            }
            
            if (fireCoolDown <= 0)
            {
                ShootBullet();
            }
        }

        Vector3 myPos = cellPosition.position;
        if ((playerLocation.position - myPos).magnitude > (infected ? detectionRange * 3 : detectionRange * 0.5f) )
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
        if (!beAbsorbed)
        {
            switch (cellStateMachine.GetState())
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
        }

        if (infectedTimerScale < 1.0f)
        {
            infectedTimerScale += Time.deltaTime;
            SetBlendShapes();
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
        if (infected)
        {
            cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);
        }
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

    // clamp position
    void LateUpdate()
    {
        if (infectedType == InfectedSpecialType.MINE)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -1 * (detectionRange - 1), (detectionRange - 1)), transform.position.z);
        }
    }

    private void ModifyScore()
    {
        scoreWorth += (int)(myGenes[0] * 100.0f);
        scoreWorth += (int)(myGenes[1] * 100.0f);
        scoreWorth += (int)(myGenes[2] * 100.0f);
        scoreWorth += (int)(myGenes[3] * 100.0f);
    }

    private void SetStats()
    {
        health += myGenes[0] * ScalingHealth;
        damage += myGenes[1] * ScalingDamage;
        detectionRange += myGenes[2] * ScalingRanged;
        MaxSpeed += myGenes[3] * ScalingMaxSpeed;
        speed += myGenes[3] * ScalingAcceleration;

        ModifyScore();

        if (myGenes[0] > geneTriggerValue) // health
        {
            infectedType = InfectedSpecialType.HEALTH;
            if (myGenes[1] > geneTriggerValue) // damage
            {
                infectedType = InfectedSpecialType.MINE;
                return;
            }
            if (myGenes[3] > geneTriggerValue) // speed
            {
                infectedType = InfectedSpecialType.REPLICATION;
                return;
            }
        }
        if (myGenes[1] > geneTriggerValue) // damage
        {
            infectedType = InfectedSpecialType.DAMAGE;
            if(myGenes[3] > geneTriggerValue) // speed
            {
                infectedType = InfectedSpecialType.KAMIKAZE;
                return;
            }
        }
        if (myGenes[3] > geneTriggerValue) // speed
        {
            infectedType = InfectedSpecialType.SPEED;
        }

        if (myGenes[2] > geneTriggerValue)
        {
            ranged = true;
            infectedType = InfectedSpecialType.RANGED;
        }

        healthChromosome = myGenes[0];
        damageChromosome = myGenes[1];
        rangedChromosome = myGenes[2];
        speedChromosome = myGenes[3];

        childNucleus.SetChromosome(myGenes);

        SetBlendShapes();
    }

    public void CreateInfected(Chromosome _input)
    {
        myGenes = _input;
        infected = true;

        gameObject.tag = "Enemy";

        SetStats();

        if (infectedType != InfectedSpecialType.KAMIKAZE)
        {
            cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);
        }
        else
        {
            cellStateMachine.Advance(InfectedCellState.DORMANT);
        }

    }

    public void BecomeInfected(Chromosome _input)
    {
        myGenes = Chromosome.Crossover(myGenes, _input);
        infected = true;

        gameObject.tag = "Enemy";

        SetStats();

        if (infectedType != InfectedSpecialType.KAMIKAZE)
        {
            cellStateMachine.Advance(InfectedCellState.CHASINGPLAYER);
        }
        else
        {
            cellStateMachine.Advance(InfectedCellState.DORMANT);
        }
        
        // other stuff for infecting the cell
    }

    private void SetBlendShapes()
    {

        switch(infectedType)
        {
            case InfectedSpecialType.SPEED:
                skinMeshRenderer.SetBlendShapeWeight(2, 110 * infectedTimerScale);
                transform.localScale = transform.localScale * (1 - infectedTimerScale * 0.015f);
                break;
            default:
                skinMeshRenderer.SetBlendShapeWeight(1, myGenes[1] * 300 * infectedTimerScale);
                break;
        }

        // change colour of cell

        GetComponentInChildren<Renderer>().material.SetColor("_Color", new Vector4(1, 0, 0, 1));

        blendShapeChild.infected = true;

        //skinMeshRenderer.SetBlendShapeWeight(0, myGenes[0] * 300);
        //skinMeshRenderer.SetBlendShapeWeight(1, myGenes[1] * 300 * infectedTimerScale);
        //skinMeshRenderer.SetBlendShapeWeight(2, myGenes[2] * 300);
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
            if (infectedType == InfectedSpecialType.KAMIKAZE)
            {
                kamikazeTimer = 0.01f;
            }
            else
            {
                PlayerScript script = other.GetComponent<PlayerScript>();
                script.TakeDamage(damage);
                currentHitCooldown = hitCooldown;
                // bounce off

                Vector3 toPlayer = transform.position - script.transform.position;

                ApplyForce(toPlayer.normalized * 30000.0f);
            }
        }

    }
}
