using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    public float lifetime = 0.5f;

    public float damage = 1.0f;

    private int playerLayer = 8;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        lifetime -= Time.deltaTime;

        if(lifetime < 0)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer && other.tag == "Player")
        {
            PlayerScript script = other.GetComponent<PlayerScript>();
            script.TakeDamage(damage);
        }
    }
}
