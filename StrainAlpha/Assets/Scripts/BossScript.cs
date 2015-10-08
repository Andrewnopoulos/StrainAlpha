﻿using UnityEngine;
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
    private float damage = 2.0f;
    private float speed = 3.5f;

    public int scoreWorth = 100000;

    private float MaxSpeed = 5.0f;

    private Vector3 target;
    private Transform playerLocation;

    public GameObject bulletPrefab;

    private bool roaming = false;

    private float dormantTime = 1.0f;
    private float dormantCountdown = 1.0f;

    private float shootCooldown;
    private float shootRate;

    private float attackLength = 5.0f;
    private float attackCooldown = 5.0f;

	// Use this for initialization
	void Start () {
        playerLocation = GameObject.Find("Player").transform;
        target = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
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

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
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

    }

    public void TakeDamage(float pain)
    {
        health -= pain;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
