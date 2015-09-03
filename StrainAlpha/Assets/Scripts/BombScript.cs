using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    public float damage = 100.0f;

    public float speed = 4.0f;
    public Vector3 size;

    public float explosionRadius;

    private float lifeTime = 0.8f;

    private bool active = false;
    private bool alive = true;

    private int enemyLayer = 9;

	// Use this for initialization
	void Start () 
    {
        size = transform.localScale;
        explosionRadius = gameObject.GetComponent<SphereCollider>().radius;
	}

    // Update is called once per frame
    void Update()
    {
        if (!active && lifeTime > 0.0f)
        {
            size += new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
            lifeTime = 0.8f;
        }
        else if (active && lifeTime > 0.0f)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        lifeTime -= Time.deltaTime;

        if (lifeTime <= -0.1f)
        {
            alive = false;
        }

    }

    public void Launch()
    {
        gameObject.transform.parent = null;
        active = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (!alive)
            return;
        if (!active)
            return;
        if (lifeTime > 0)
            return;
        if (other.gameObject.layer == enemyLayer)
        {
            other.GetComponent<CellScript>().TakeDamage(damage * Time.deltaTime);
        }
    }

}
