using UnityEngine;
using System.Collections;

public class SpawnColliderScript : MonoBehaviour {

    public CampScript parent;

	// Use this for initialization
	void Start () {
	    parent = gameObject.GetComponentInParent<CampScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        parent.canSpawn = false;
    }

    void OnTriggerExit(Collider other)
    {
        parent.canSpawn = true;
    }

}
