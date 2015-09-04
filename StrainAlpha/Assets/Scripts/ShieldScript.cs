using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {

    private bool active = false;

    private Renderer renderer;
    private Collider collider;

    private int enemyLayer = 9;
    private int enemyBulletLayer = 10;

	// Use this for initialization
	void Start () {

        renderer = gameObject.GetComponent<Renderer>();
        collider = gameObject.GetComponent<Collider>();

	}

    public bool GetActive()
    {
        return active;
    }
	
	// Update is called once per frame
	void Update () {

        if (active && !renderer.enabled)
        {
            renderer.enabled = true;
            collider.enabled = true;
        }

        if (!active)
        {
            renderer.enabled = false;
            collider.enabled = false;
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
