﻿using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float damage = 0.0f;
    public float speed = 0.0f;
    public float size = 0.2f;

    private float lifeTime = 2.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(gameObject);

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;
        if (other.tag == "Enemy")
        {
            //deal damage to the enemy
            other.GetComponent<EnemyScript>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
