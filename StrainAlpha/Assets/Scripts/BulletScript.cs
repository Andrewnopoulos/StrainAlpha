using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float damage;
    public float speed;

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
}
