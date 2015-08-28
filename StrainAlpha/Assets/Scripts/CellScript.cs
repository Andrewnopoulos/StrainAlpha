using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {

    public NPCManager manager;

    private float health = 5.0f;
    private float damage = 2.0f;
    private float speed = 8.5f;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 1.0f;

    private Chromosome myGenes;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (health <= 0)
        {
            //add this object to the destroy list
            manager.AddToKillList(gameObject);
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
}
