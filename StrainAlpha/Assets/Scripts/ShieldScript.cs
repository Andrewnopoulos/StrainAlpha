using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {

    private bool active = false;

    private Renderer renderer;
    private Collider collider;

    private float shieldTime = 3.0f;

    private int enemyLayer = 9;
    private int enemyBulletLayer = 10;

	// Use this for initialization
	void Start () {

        renderer = gameObject.GetComponent<Renderer>();
        collider = gameObject.GetComponent<Collider>();

	}
	
	// Update is called once per frame
	void Update () {

        if (active && !renderer.enabled)
        {
            renderer.enabled = true;
            collider.enabled = true;
        }
        if (active && renderer.enabled)
        {
            shieldTime -= Time.deltaTime;
        }

        if (shieldTime < 0)
        {
            renderer.enabled = false;
            collider.enabled = false;
            active = false;
            shieldTime = 3.0f;
        }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyBulletLayer)
        {
            Destroy(other);
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
    }
}
