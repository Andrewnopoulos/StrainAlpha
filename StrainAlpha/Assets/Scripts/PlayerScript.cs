using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public GameObject bulletPrefab;
    private CharacterController characterController;

    private float health;
    private float damage;
    private float speed = 8;
    private float fireRate = 0.5f;
    private float range;

    private float fireCooldown = 0.0f;

	// Use this for initialization
	void Start () {

        characterController = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
	
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }

        Vector3 movementVector = Vector3.zero;
        Vector3 lookVector = Vector3.zero;

        movementVector.x = Input.GetAxis("LeftStickX");
        movementVector.z = -Input.GetAxis("LeftStickY");

        lookVector.z = -Input.GetAxis("RightStickY");
        lookVector.x = Input.GetAxis("RightStickX");
        lookVector.y = 0;

        characterController.Move(movementVector * speed * Time.deltaTime);

        if (lookVector.sqrMagnitude > 0.2)
        {
            transform.rotation = Quaternion.LookRotation(lookVector);
        }

        if (Input.GetAxis("RightTrigger") > 0.1f)
        {
            if (fireCooldown <= 0)
            {
                GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
                newBullet.GetComponent<BulletScript>().damage = damage;
                newBullet.GetComponent<BulletScript>().speed = 20;
                fireCooldown = fireRate;
            }
        }

	}
}
