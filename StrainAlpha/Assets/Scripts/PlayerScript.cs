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

    //time left in current dash
    private float currentDash = 0.0f;
    private float dashTime = 0.2f;

    //direction of dash
    private Vector3 dashDir;

    //time until next dash
    private float dashCooldown = 2.0f;
    private float currentDashCooldown = 0.0f;

	// Use this for initialization
	void Start () {

        characterController = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
	
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;

        if (currentDash > 0)
            currentDash -= Time.deltaTime;

        if (currentDashCooldown > 0)
            currentDashCooldown -= Time.deltaTime;

        Vector3 movementVector = Vector3.zero;
        Vector3 lookVector = Vector3.zero;

        if (currentDash > 0)
        {
            movementVector = dashDir;
        }
        else
        {

            movementVector.x = Input.GetAxis("LeftStickX");
            movementVector.z = -Input.GetAxis("LeftStickY");

            movementVector = movementVector * speed;

            if (Input.GetButton("LeftBumper"))
            {
                if (currentDashCooldown <= 0)
                {
                    currentDashCooldown = dashCooldown;

                    currentDash = dashTime;
                    dashDir = movementVector * 3.0f;
                    movementVector = dashDir;

                    //apply screen shake
                }
            }
        }
        characterController.Move(movementVector * Time.deltaTime);

        lookVector.z = -Input.GetAxis("RightStickY");
        lookVector.x = Input.GetAxis("RightStickX");
        lookVector.y = 0;

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
