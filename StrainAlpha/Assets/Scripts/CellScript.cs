using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {

    public NPCManager manager;

    private float health = 5.0f;
    private float damage = 2.0f;
    private float speed = 8.5f;

    private bool infected = false;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 1.0f;

    private Chromosome myGenes;

	// Use this for initialization
	void Start () {
        myGenes = new Chromosome(0.1f);
        infected = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject);
        }

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
