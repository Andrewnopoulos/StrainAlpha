using UnityEngine;
using System.Collections;

public class BossLaser : MonoBehaviour {

    public float damage;

    private Renderer[] renderer;
    private Collider[] collider;
    private GameObject[] children;

    private bool active = false;

    private int playerLayer = 8;

    private float baseLength = 35.0f;
    private float currentLength = 0.0f;

    private float baseWidth = 0.15f;
    private float currentWidth = 0.05f;

    private float basePos = 17.5f;
    private float currentPos = 0.0f;

    // Use this for initialization
    void Start()
    {
        renderer = new Renderer[4];
        collider = new Collider[4];
        children = new GameObject[5];

        renderer = gameObject.GetComponentsInChildren<Renderer>();
        collider = gameObject.GetComponentsInChildren<Collider>();
        children = gameObject.GetComponentsInChildren<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject obj in children)
        {
            obj.transform.localScale = new Vector3(currentWidth, transform.localScale.y, currentLength);
        }

        if (active)
        {
            currentLength += (baseLength - currentLength) * (Time.deltaTime * 8.0f);
            if (currentLength > baseLength - 10.0f)
            {
                currentWidth += (baseWidth - currentWidth) * (Time.deltaTime * 4.0f);
            }
        }
        else
        {
            currentLength = 0.0f;
            currentWidth = 0.05f;
            currentPos = 0.0f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == playerLayer && other.tag == "Player")
        {
            //deal damage to the enemy
            other.GetComponent<PlayerScript>().TakeDamage(damage * Time.deltaTime);
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
        for (int i = 0; i < 4; ++i)
        {
            renderer[i].enabled = _active;
            collider[i].enabled = _active;
        }
    }

    public bool GetActive()
    {
        return active;
    }
}
