using UnityEngine;
using System.Collections;

public class GravitationEffectScript : MonoBehaviour {

    private bool active = false;

    private BlackHoleScript parentScript;

    private int enemyLayer = 9;
    private int enemyBulletLayer = 10;

    private Collider collider;

    public float gravitationForce = 1.0f;

	// Use this for initialization
	void Start () {
        parentScript = GetComponentInParent<BlackHoleScript>();

        collider = gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        active = parentScript.IsActive();

        collider.enabled = active;
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            CellScript enemy = other.GetComponent<CellScript>();

            Vector3 enemyPos = enemy.GetCurrentLocation();

            Vector3 normalizedForceVector = (transform.position - enemyPos).normalized;

            enemy.SetGravitating(true);

            enemy.ApplyForce(normalizedForceVector * gravitationForce);
        }
        else if (other.gameObject.layer == enemyBulletLayer)
        {
            BulletScript bullet = other.GetComponent<BulletScript>();

            Vector3 bulletPos = bullet.transform.position;

            Vector3 normalizedForceVector = (transform.position - bulletPos).normalized;

            bullet.SetGravitating(true);

            bullet.ApplyForce(normalizedForceVector * gravitationForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            other.GetComponent<CellScript>().SetGravitating(true);
        }
        else if (other.gameObject.layer == enemyBulletLayer)
        {
            other.GetComponent<BulletScript>().SetGravitating(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            other.GetComponent<CellScript>().SetGravitating(false);
        } 
        else if (other.gameObject.layer == enemyBulletLayer)
        {
            other.GetComponent<BulletScript>().SetGravitating(false);
        }
    }
}
