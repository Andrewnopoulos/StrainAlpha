using UnityEngine;
using System.Collections;

public class BossShooter : MonoBehaviour {

    private Transform parent;

	// Use this for initialization
	void Start () {

        parent = transform.GetComponentInParent<Transform>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        //transform.rotation 
    }
}
