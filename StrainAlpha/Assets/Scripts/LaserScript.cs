using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

    public float damage = 5.0f;

    private bool active = false;

    private Renderer renderer;
    private Collider collider;

    public float laserTime = 3.0f;

    private int enemyLayer = 9;

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
        if (active && renderer.enabled)
        {
            laserTime -= Time.deltaTime;
        }

        if (laserTime < 0)
        {
            renderer.enabled = false;
            collider.enabled = false;
            active = false;
            laserTime = 3.0f;
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (!active)
            return;
        if (other.gameObject.layer == enemyLayer)
        {
            //deal damage to the enemy
            other.GetComponent<CellScript>().TakeDamage(damage * Time.deltaTime);
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
    }

    public bool GetActive()
    {
        return active;
    }
}
