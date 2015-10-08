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

    private bool gravitating = false;

    private Vector3 velocity;

	// Use this for initialization
	void Start () {
        
	}

    void Awake()
    {
        velocity = transform.forward;
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

        if (gravitating)
        {
            
        }

        transform.position += velocity * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            alive = false;

        if (!alive)
            Kill();
	}

    public void SetGravitating(bool _gravitating)
    {
        gravitating = _gravitating;
    }

    public void ApplyForce(Vector3 _force)
    {
        velocity += _force * Time.deltaTime;
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

            Vector3 delta = other.transform.position - transform.position;
            delta.Normalize();
            transform.forward -= 2 * delta;
            transform.forward.Normalize();
            lifeTime = 1.5f;
            alive = true;
        }

        if (!isEnemyBullet && other.gameObject.layer != enemyLayer && other.gameObject.layer != shieldLayer)
        {
            alive = false;
        }
        
        
    }
}
