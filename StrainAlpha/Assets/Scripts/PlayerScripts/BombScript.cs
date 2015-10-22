using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    private float damage = 8.0f;

    public GameObject bullet;

    private bool active = false;

    private int enemyLayer = 9;

    private float fireRate = 0.2f;
    private float fireCooldown = 0.2f;

    // Update is called once per frame
    void Update()
    {
        fireRate -= Time.deltaTime;
    }

    public void ShootBullet()
    {

        if (fireRate < 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            fireRate = fireCooldown;
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
    }

    public bool GetActive()
    {
        return active;
    }
}
