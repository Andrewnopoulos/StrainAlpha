using UnityEngine;
using System.Collections;

public enum AttackType
{
    DORMANT = 0,
    ATTACKRADIAL = 1,
    ATTACKCIRCLE = 2,
    ATTACKTARGET = 3,
    LASER = 4,
    SHIELD = 5,
    CHARGE = 6,
    EXPLOSIVE = 7,
    SPAWNCELLS = 8,
}

public enum BossType
{
    HEALTH,
    SPEED,
    RANGE,
    DAMAGE,
    NULL,
}

public class BossScript : MonoBehaviour {

    private AttackType attackType = AttackType.DORMANT;

    private float baseHealth = 500.0f;
    private float basedamage = 1.0f;
    private float baseAtkSpeed = 1.0f;
    private float basespeed = 3.5f;

    private float health = 500.0f;
    private float damage = 1.0f;
    private float atkSpeed = 1.0f;
    private float speed = 3.5f;

    public int scoreWorth = 100000;

    private float MaxSpeed = 5.0f;

    private BossType bossType;

    public NPCManager manager;
    private Vector3 target;
    private Transform playerLocation;

    public GameObject bulletPrefab;
    public GameObject infectedCell;

    private SkinnedMeshRenderer skinMeshRenderer;

    private Collider cocoonCollider;
    private MeshRenderer cocoonRenderer;

    private GameObject shooter;

    private PlayerUI ui;

    private BossLaser laser;
    private Renderer chargerRenderer;
    private Collider chargerCollider;

    private bool roaming = false;

    private float dormantTime = 1.0f;
    private float dormantCountdown = 1.0f;

    private float shootCooldown;
    private float shootRate;

    private float attackLength = 5.0f;
    private float attackCooldown = 5.0f;

    private bool born = false;

    public float birthTime = 30.0f;

    private float currentAttackIndicator = 0.0f;

    private float scale = 0.0f;

    private float targetScale = 1.0f;

    private bool followingPlayer;

    private float chargeTime;
    private Vector3 chargeDir;

	// Use this for initialization
	void Start () {
        playerLocation = GameObject.Find("Player").transform;
        target = new Vector3(0, 0, 0);
        ui = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();

        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        shooter = GameObject.Find("shooter");

        laser = gameObject.GetComponentInChildren<BossLaser>();

        chargerRenderer = GameObject.Find("Charger").GetComponent<Renderer>();
        chargerCollider = GameObject.Find("Charger").GetComponent<Collider>();

        cocoonCollider = gameObject.GetComponentsInChildren<Collider>()[1];
        cocoonRenderer = gameObject.GetComponentInChildren<MeshRenderer>();

        bossType = BossType.NULL;

        followingPlayer = false;

        chargerRenderer.enabled = false;
        chargerCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (followingPlayer)
        {
            FollowPlayer();
        }

        scale += (targetScale - scale) * (Time.deltaTime);

        transform.localScale = new Vector3(scale, scale, scale);

        if (!born)
        {
            CocoonUpdate();
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
                case AttackType.LASER:
                    {
                        AttackLaser();
                        break;
                    }
                case AttackType.SHIELD:
                    {
                        Shield();
                        break;
                    }
                case AttackType.CHARGE:
                    {
                        Charge();
                        break;
                    }
                case AttackType.EXPLOSIVE:
                    {
                        Explosive();
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

    void SetBlendShape()
    {
        float _health = health / baseHealth / 3.0f;
        float _speed = speed / basespeed * 100.0f;
        float _atkspeed = atkSpeed / baseAtkSpeed * 100.0f;
        float _damage = damage / basedamage * 100.0f;
        //if (_health > _speed && _health > _damage && _health > _atkspeed)
        //{
        //    skinMeshRenderer.SetBlendShapeWeight(3, 100);
        //    bossType = BossType.HEALTH;
        //}
        //else if (_speed > _atkspeed && _speed > _damage && _speed > _health)
        //{
        //    skinMeshRenderer.SetBlendShapeWeight(2, 100);
        //    bossType = BossType.SPEED;
        //}
        //else if (_atkspeed > _damage && _atkspeed > _speed && _atkspeed > _health)
        //{
        //    skinMeshRenderer.SetBlendShapeWeight(1, 100);
        //    bossType = BossType.RANGE;
        //}
        //else
        //{
        //    skinMeshRenderer.SetBlendShapeWeight(0, 100);
        //    bossType = BossType.DAMAGE;
        //}

        //cos damage looks best
        skinMeshRenderer.SetBlendShapeWeight(0, 100);
        bossType = BossType.DAMAGE;
    }

    void FollowPlayer()
    {
        Vector3 delta = Vector3.Normalize(playerLocation.position - transform.position);
        transform.position += (delta * speed) * Time.deltaTime;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
    }

    void CocoonUpdate()
    {
        birthTime -= Time.deltaTime;

        if (birthTime <= 0)
        {
            born = true;
            gameObject.GetComponentsInChildren<Collider>()[0].enabled = true;
            cocoonCollider.enabled = false;
            cocoonRenderer.enabled = false;

            SetBlendShape();
        }
    }

    private void AttackRadial()
    {
        shootCooldown -= Time.deltaTime;
        shooter.transform.Rotate(new Vector3(0, 1, 0), 360 * Time.deltaTime);
        if (shootCooldown <= 0)
        {
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, shooter.transform.rotation);
            BulletScript script = newBullet.GetComponent<BulletScript>();
            script.damage = damage;
            script.speed = 12.0f;
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
                shooter.transform.Rotate(new Vector3(0, 1, 0), (360 / 32));
                GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, shooter.transform.rotation);
                BulletScript script = newBullet.GetComponent<BulletScript>();
                script.damage = damage;
                script.speed = 12.0f;
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
        shooter.transform.LookAt(playerLocation);
        if (shootCooldown <= 0)
        {
            GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, shooter.transform.rotation);
            BulletScript script = newBullet.GetComponent<BulletScript>();
            script.damage = damage;
            script.speed = 12.0f;
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

    private void AttackLaser()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            laser.SetActive(false);
            attackType = AttackType.DORMANT;
        }
    }

    private void Shield()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;
        }
    }

    private void Charge()
    {
        followingPlayer = false;

        chargeTime -= Time.deltaTime;
        if (chargeTime > 0)
        {
            //choose direction to charge in
            //chargeDir = Vector3.Normalize(playerLocation.position - transform.position);
            shooter.transform.LookAt(playerLocation.position);
        }
        else
        {
            //charge in direction
            transform.position += (shooter.transform.forward * speed * 4.0f) * Time.deltaTime;
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;

            chargerCollider.enabled = false;
            chargerRenderer.enabled = false;
        }
    }

    private void Explosive()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackType = AttackType.DORMANT;
        }
    }

    private void Dormant()
    {
        followingPlayer = false;

        dormantCountdown -= Time.deltaTime;
        if (dormantCountdown <= 0)
        {
            AttackType specialAttack = AttackType.DORMANT;
            switch (bossType)
            {
                case BossType.HEALTH:
                    specialAttack = AttackType.CHARGE;
                    break;

                case BossType.SPEED:
                    specialAttack = AttackType.CHARGE;
                    break;

                case BossType.DAMAGE:
                    specialAttack = AttackType.LASER;
                    break;

                case BossType.RANGE:
                    specialAttack = AttackType.LASER;
                    break;
            }

            int rand = 0;
            if (currentAttackIndicator < 2)
                rand = (int)Random.Range(1.0f, 3.99f);
            else if (currentAttackIndicator == 2)
                rand = 4;

            if (currentAttackIndicator == 3)
            {
                rand = 5;
                currentAttackIndicator = -1;
            }

            if (rand == 1)
            {
                attackType = AttackType.ATTACKRADIAL;
                shootRate = 0.02f / atkSpeed;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            else if (rand == 2)
            {
                attackType = AttackType.ATTACKCIRCLE;
                shootRate = 0.5f / atkSpeed;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            else if (rand == 3)
            {
                attackType = AttackType.ATTACKTARGET;
                shootRate = 0.05f / atkSpeed;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            else if (rand == 4)
            {
                attackType = specialAttack;
                attackCooldown = attackLength;
            }
            else
            {
                attackType = AttackType.SPAWNCELLS;
                shootRate = 0.3f / atkSpeed;
                shootCooldown = 0.0f;
                attackCooldown = attackLength;
            }
            dormantCountdown = dormantTime;
            currentAttackIndicator++;

            if (attackType == AttackType.LASER)
            {
                laser.SetActive(true);
            }
            else if (attackType == AttackType.SHIELD)
            {

            }
            else if (attackType == AttackType.EXPLOSIVE)
            {

            }
            else if (attackType == AttackType.CHARGE)
            {
                chargeTime = 1.0f;

                chargerCollider.enabled = true;
                chargerRenderer.enabled = true;
            }

            followingPlayer = true;
        }
    }

    private void SpawnCells()
    {
        shootCooldown -= Time.deltaTime;
        shooter.transform.LookAt(playerLocation);
        if (shootCooldown <= 0)
        {
            int rand = (int)Random.Range(0.0f, 3.99f);
            manager.CreateInfectedCell(new Chromosome(rand), transform.position + (shooter.transform.forward * transform.localScale.x / 2));
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
            CellScript cell = other.gameObject.GetComponent<CellScript>();

            targetScale += 0.04f;

            //these values will need balancing
            health += cell.GetChromosome()[0] * 3.0f;
            damage += cell.GetChromosome()[1] / 100.0f;
            atkSpeed += cell.GetChromosome()[2] / 100.0f;
            speed += cell.GetChromosome()[3] / 100.0f;

            Destroy(other.gameObject);
        }
    }

    public float GetHealth()
    {
        return health;
    }

}
