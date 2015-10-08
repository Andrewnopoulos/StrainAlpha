using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

    public float damage = 5.0f;

    private Renderer renderer;
    private Collider collider;

    private bool active = false;

    private int enemyLayer = 9;

    private float baseLength = 25.0f;
    private float currentLength = 0.0f;

    private float baseWidth = 0.3f;
    private float currentWidth = 0.1f;

	// Use this for initialization
	void Start () {

        renderer = gameObject.GetComponent<Renderer>();
        collider = gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
	
        gameObject.transform.localScale = new Vector3(currentWidth, transform.localScale.y, currentLength);

        if (active)
        {
            currentLength = (baseLength - currentLength) * (Time.deltaTime * 10.0f);
            if (currentLength > baseLength - 1.0f)
            {
                currentWidth = (baseWidth - currentWidth) * (Time.deltaTime * 10.0f);
            }
        }
        else
        {
            currentLength = 0.0f;
            currentWidth = 0.1f;
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && other.tag == "Enemy")
        {
            //deal damage to the enemy
            other.GetComponent<CellScript>().TakeDamage(damage * Time.deltaTime);
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
        renderer.enabled = _active;
        collider.enabled = _active;
    }

    public bool GetActive()
    {
        return active;
    }
}
