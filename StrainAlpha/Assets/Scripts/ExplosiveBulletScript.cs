using UnityEngine;
using System.Collections;

public class ExplosiveBulletScript : MonoBehaviour {

    private float initialDamage = 3.0f;
    private float explosionDamage = 4.0f;
    private float speed = 15.0f;

    private float explosionTime = 0.5f;

    private float lifeTime = 2.0f;

    private bool alive = true;
    private bool collided = false;

    private int enemyLayer = 9;
    private int playerLayer = 8;
    private int shieldLayer = 13;

    private Collider[] colliders;
    private Renderer[] explosionMesh;
    public GameObject explosionParticles;

	// Use this for initialization
	void Start () {

        colliders = gameObject.GetComponents<Collider>();
        explosionMesh = gameObject.GetComponentsInChildren<Renderer>();

	}
    void Kill()
    {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {

        if (!collided)
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            lifeTime -= Time.deltaTime;
        }

        if (lifeTime < 0)
            alive = false;

        if (collided)
            Explode();

        if (!alive)
            Kill();
	}

    void Explode()
    {
        explosionTime -= Time.deltaTime;

        if (explosionTime < 0)
            alive = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (collided)
            return;
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            //deal damage to the enemy
            CellScript script = other.GetComponent<CellScript>();
            if (script.infected)
            {
                script.TakeDamage(initialDamage);
            }
            collided = true;

            colliders[0].enabled = !colliders[0].enabled;
            colliders[1].enabled = !colliders[1].enabled;

            explosionMesh[0].enabled = false;
            Instantiate(explosionParticles, transform.position, Quaternion.Euler(90, 0, 0));

        }
        if (other.gameObject.layer != enemyLayer && other.gameObject.layer != shieldLayer)
        {
            collided = true;

            colliders[0].enabled = !colliders[0].enabled;
            colliders[1].enabled = !colliders[1].enabled;

            explosionMesh[0].enabled = false;
            Instantiate(explosionParticles, transform.position, Quaternion.Euler(90, 0, 0));
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (!collided)
            return;
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            //deal damage to the enemy
            CellScript script = other.GetComponent<CellScript>();
            if (script.infected)
            {
                script.TakeDamage(explosionDamage * Time.deltaTime);
            }
        }
    }

}
