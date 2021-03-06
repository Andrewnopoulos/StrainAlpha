﻿using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour {

    public bool active = false;

    private Renderer renderer;
    private Collider collider;

    private int enemyLayer = 9;
    private int enemyBulletLayer = 10;

	// Use this for initialization
	void Start () {
        renderer = gameObject.GetComponent<Renderer>();
        collider = gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active && !renderer.enabled)
        {
            renderer.enabled = true;
            collider.enabled = true;
        }

        if (!active)
        {
            renderer.enabled = false;
            collider.enabled = false;
        }
	}

    public bool IsActive()
    {
        return active;
    }

    public void SetActive(bool _active)
    {
        active = _active;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyBulletLayer)
        {
            Destroy(other);
        }
        else if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            // TODO change this to something like bouncing enemies off or some shit
            other.GetComponent<CellScript>().TakeDamage(10.0f);
        }
    }

}
