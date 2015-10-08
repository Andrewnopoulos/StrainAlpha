using UnityEngine;
using System.Collections;

public class GravitationEffectScript : MonoBehaviour {

    private bool active = false;

    private BlackHoleScript parentScript;

    private int enemyLayer = 9;
    private int enemyBulletLayer = 10;

	// Use this for initialization
	void Start () {
        parentScript = GetComponentInParent<BlackHoleScript>();
	}
	
	// Update is called once per frame
	void Update () {
        active = parentScript.IsActive();
	}
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == enemyBulletLayer || other.gameObject.layer == enemyLayer)
        {
            
        }
    }
}
