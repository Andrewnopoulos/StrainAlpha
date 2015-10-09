using UnityEngine;
using System.Collections;

public enum AttackType
{
    DORMANT = 0,
    ATTACKRADIAL = 1,
    ATTACKCIRCLE = 2,
    ATTACKTARGET = 3,
    SPAWNCELLS = 4,
}

public class BossScript : MonoBehaviour {

    private AttackType attackType = AttackType.DORMANT;

    private float health = 500.0f;
    private float damage = 1.0f;
    private float speed = 3.5f;

    public int scoreWorth = 100000;

    private float MaxSpeed = 5.0f;

    private Vector3 target;
    private Transform playerLocation;

    public GameObject bulletPrefab;
    public GameObject infectedCell;

    private PlayerUI ui;

    private bool roaming = false;

    private float dormantTime = 1.0f;
    private float dormantCountdown = 1.0f;

    private float shootCooldown;
    private float shootRate;

    private float attackLength = 5.0f;
    private float attackCooldown = 5.0f;

    private bool born = false;

    public float birthTime = 30.0f;

    public float gravitationForce = 1.0f;

    private float scale = 1.0f;

    private float targetScale = 1.0f;

	// Use this for initialization
	void Start () {
        playerLocation = GameObject.Find("Player").transform;
        target = new Vector3(0, 0, 0);
        ui = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
	}
	
	// Update is called once per frame
	void Update () {

        scale += (targetScale - scale) * (Time.deltaTime);

        transform.localScale = new Vector3(scale, scale, scale);

        if (!born)
        {
            birthTime -= Time.deltaTime;

            if (birthTime <= 0)
            {
                born = true;
                gameObject.GetComponentsInChildren<Collider>()[0].enabled = true;
            }
        }
        else
        {
            if (health <= 0)
            {
                Die();
            }

            switch (attackType)
            {
                case AttackType.DORMANT:
                    {
                        Dormant();
                        break;
                    }
                case AttackType.ATTACKRADIAL:
                    {
                        AttackRadial();
                        break;
                    }
                case AttackType.ATTACKCIRCLE:
                    {
                        AttackCircle();
                        break;
                    }
                case AttackType.ATTACKTARGET:
                    {
                        AttackTarget();
                        break;
                    }
                case AttackType.SPAWNCELLS:
                    {
                        SpawnCells();
                        break;
                    }
            }
        }
	}

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }

    private void AttackRadial()
    {
        shootCooldown -= Time.deltaTime;
        transform.Rotate(new Vector3(0, 1, 0), 360 * Time.deltaTime);
        if (shootCooldown <= 0)
        {
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletScript script = newBullet.GetComponent<BulletScript>();
            script.damage = damage;
            script.speed = 15.0f;
            script.SetAsEnemyBullet();
            shootCooldown = shootRate;
            script.transform.localScale *= 2;
            script.lifeTime = 3.0f;
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;
        }
    }

    private void AttackCircle()
    {
        shootCooldown -= Time.deltaTime;
        if (shootCooldown <= 0)
        {
            for (int i = 0; i < 32; ++i)
            {
                transform.Rotate(new Vector3(0, 1, 0), (360 / 32));
                GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
                BulletScript script = newBullet.GetComponent<BulletScript>();
                script.damage = damage;
                script.speed = 15.0f;
                script.SetAsEnemyBullet();
                script.transform.localScale *= 2;
                script.lifeTime = 3.0f;
            }
            shootCooldown = shootRate;
        }
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;
        }
    }

    private void AttackTarget()
    {
        shootCooldown -= Time.deltaTime;
        transform.LookAt(playerLocation);
        if (shootCooldown <= 0)
        {
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletScript script = newBullet.GetComponent<BulletScript>();
            script.damage = damage;
            script.speed = 15.0f;
            script.SetAsEnemyBullet();
            script.transform.localScale *= 2;
            script.lifeTime = 3.0f;
            shootCooldown = shootRate;
        }
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;
        }
    }

    private void Dormant()
    {
        dormantCountdown -= Time.deltaTime;
        if (dormantCountdown <= 0)
        {
            int rand = (int)Random.Range(1.0f, 3.99f);
            if (rand == 1)
            {
                attackType = AttackType.ATTACKRADIAL;
                shootRate = 0.02f;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            else if (rand == 2)
            {
                attackType = AttackType.ATTACKCIRCLE;
                shootRate = 0.5f;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            else if (rand == 3)
            {
                attackType = AttackType.ATTACKTARGET;
                shootRate = 0.05f;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            else
            {
                attackType = AttackType.SPAWNCELLS;
                shootRate = 0.3f;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            dormantCountdown = dormantTime;
        }
    }

    private void SpawnCells()
    {
        shootCooldown -= Time.deltaTime;
        transform.LookAt(playerLocation);
        if (shootCooldown <= 0)
        {
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletScript script = newBullet.GetComponent<BulletScript>();
            script.damage = damage;
            script.speed = 15.0f;
            script.SetAsEnemyBullet();
            script.transform.localScale *= 2;
            script.lifeTime = 3.0f;
            shootCooldown = shootRate;
        }
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;
        }
    }

    public void TakeDamage(float pain)
    {
        health -= pain;
    }

    void Die()
    {
        ui.AddScore(scoreWorth);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (born)
            return;
        if (other.tag == "Enemy" && other.gameObject.layer == 9)
        {
            //absorb the enemy
            Destroy(other.gameObject);

            targetScale += 0.04f;
            health += 2.0f;
            damage += 0.01f;

        }
    }

}
