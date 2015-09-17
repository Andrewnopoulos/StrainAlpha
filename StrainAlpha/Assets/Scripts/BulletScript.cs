using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float damage = 0.0f;
    public float speed = 0.0f;

    private float lifeTime = 2.0f;

    private bool alive = true;

    public bool isEnemyBullet = false;

    private int enemyLayer = 9;

    private int playerLayer = 8;

    private int shieldLayer = 13;

    public ParticleSystem particles;

	// Use this for initialization
	void Start () {

	}

    public void SetAsEnemyBullet()
    {
        gameObject.layer = enemyLayer;
        isEnemyBullet = true;
    }

    public void SetAsFriendlyBullet()
    {
        isEnemyBullet = false;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            alive = false;

        if (!alive)
            Kill();
	}

    void Kill()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!alive)
            return;
        if (!isEnemyBullet && other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            GameObject.Instantiate(particles, transform.position, transform.localRotation);
            //deal damage to the enemy
            CellScript script = other.GetComponent<CellScript>();
            if (script.infected)
            {
                script.TakeDamage(damage);
            }
            alive = false;
        }

        if (isEnemyBullet && other.gameObject.layer == playerLayer)
        {
            GameObject.Instantiate(particles, transform.position, transform.localRotation);

            PlayerScript script = other.GetComponent<PlayerScript>();
            script.TakeDamage(damage);
            alive = false;
        }

        if (isEnemyBullet && other.gameObject.layer == shieldLayer)
        {
            GameObject.Instantiate(particles, transform.position, transform.localRotation);
            SetAsFriendlyBullet();

            Vector3 delta = transform.position - other.transform.position;
            delta.Normalize();
            transform.forward -= 2 * delta;
            transform.forward.Normalize();
            lifeTime = 1.5f;
            alive = true;
            
        }

        if (!isEnemyBullet && other.gameObject.layer != enemyLayer)
        {
            alive = false;
        }
        
        
    }
}
