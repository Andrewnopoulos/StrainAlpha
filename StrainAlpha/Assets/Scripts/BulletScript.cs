using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float damage = 0.0f;
    public float speed = 0.0f;

    private float lifeTime = 2.0f;

    private bool alive = true;

    private int enemyLayer = 9;

	// Use this for initialization
	void Start () {

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
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            //deal damage to the enemy
            other.GetComponent<CellScript>().TakeDamage(damage);
        }
        alive = false;
    }
}
