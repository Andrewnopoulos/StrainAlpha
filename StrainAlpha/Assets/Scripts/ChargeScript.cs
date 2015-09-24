using UnityEngine;
using System.Collections;

public class ChargeScript : MonoBehaviour {

    public float damage = 2.0f;

    private Renderer renderer;
    private Collider collider;

    private bool active = false;

    private int enemyLayer = 9;

	// Use this for initialization
	void Start () {

        renderer = gameObject.GetComponent<Renderer>();
        collider = gameObject.GetComponent<Collider>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            //deal damage to the enemy
            BounceEnemy(other);
            other.GetComponent<CellScript>().TakeDamage(damage);
        }
    }
    public void SetActive(bool _active)
    {
        active = _active;
        renderer.enabled = _active;
        collider.enabled = _active;
    }

    public bool GetActive()
    {
        return active;
    }

    void BounceEnemy(Collider other)
    {
        Vector3 delta = gameObject.transform.position - other.transform.position;
        delta = new Vector3(delta.x * 50.0f, 0, delta.z * 50.0f);
        other.attachedRigidbody.AddForce(delta);
    }
}
