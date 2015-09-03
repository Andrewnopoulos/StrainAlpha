using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellScript : MonoBehaviour {

    public NPCManager manager;

    private float health = 5.0f;
    private float damage = 2.0f;
    private float speed = 8.5f;

    private bool infected = false;

    public float detectionRange = 100.0f;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 1.0f;

    public Vector3 velocity;

    private Chromosome myGenes;

    private Transform playerLocation;

    private SkinnedMeshRenderer skinMeshRenderer;

    private float blend0 = 0;
    private float blend1 = 0;
    private float blend2 = 0;

	// Use this for initialization
	void Start () {
        myGenes = new Chromosome(0.1f);
        infected = false;
        velocity = new Vector3(0, 0, 0);
	}

    void Awake()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    float distanceFrom(GameObject otherObject)
    {
        return (gameObject.transform.position - otherObject.transform.position).magnitude;
    }
	
	// Update is called once per frame
	void Update () {

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject);
        }

        int neighbourCount = 0;

        //foreach(GameObject cell in neutralCells)
        //{
        //    if (distanceFrom(cell) < detectionRange)
        //    {

        //        neighbourCount++;
        //    }
        //}

        if (infected)
        {
            InfectedUpdate();
        }

        transform.position += velocity;

        UpdateAnimation();
	}

    void UpdateAnimation()
    {
        skinMeshRenderer.SetBlendShapeWeight(1, 100 * Mathf.Sin(Time.time));
    }

    void InfectedUpdate()
    {

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
