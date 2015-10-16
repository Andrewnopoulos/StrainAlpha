using UnityEngine;
using System.Collections;

public class BossLaserSegment : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8 && other.tag == "Player")
        {
            other.GetComponent<PlayerScript>().TakeDamage(5.0f * Time.deltaTime);
        }
    }
}
