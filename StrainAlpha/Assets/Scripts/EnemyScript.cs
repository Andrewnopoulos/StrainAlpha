using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    private float health = 5.0f;
    private float damage = 2.0f;
    private float speed = 8.5f;

    //fire rate is synonymous with range; values over 0.5 make the enemy melee
    private float fireRate = 1.0f;

    private Chromosome myGenes;

	// Use this for initialization
	void Start () {
	
	}
	
    public EnemyScript(Chromosome inputChromosome)
    {
        myGenes = inputChromosome;
        // set blend shape values based on genes in chromosome
    }

	// Update is called once per frame
	void Update () {
	
        if (health <= 0)
        {
            //add this object to the destroy list
            //spawn a nucleus of the appropriate chromosome at this location
            Destroy(gameObject);
        }

	}


    public void TakeDamage(float _damage)
    {
        health -= _damage;
    }
}
