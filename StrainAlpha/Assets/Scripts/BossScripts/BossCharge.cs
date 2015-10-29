using UnityEngine;
using System.Collections;

public class BossCharge : MonoBehaviour {

    private Collider parent;
    private Collider myCollider;
    private PlayerScript player;
	void Start () {

        parent = GameObject.Find("Charger").GetComponent<Collider>();
        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        myCollider = gameObject.GetComponent<Collider>();

	}
	
	void Update () {
	
        if (parent.enabled && !myCollider.enabled)
        {
            myCollider.enabled = true;
        }
        else if (!parent.enabled && myCollider.enabled)
        {
            myCollider.enabled = false;
        }

	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player.TakeDamage(5.0f * Time.deltaTime);
        }
    }
}
