using UnityEngine;
using System.Collections;

public class ChargeScript : MonoBehaviour {

    public float damage = 0.01f;

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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            //deal damage to the enemy
            //BounceEnemy(other);
            CellScript script = other.GetComponent<CellScript>();

            Vector3 toPlayer = script.transform.position - transform.position;

            script.SetVelocityDelta((toPlayer.normalized) * 300000.0f);
            script.TakeDamage(damage);

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

}
