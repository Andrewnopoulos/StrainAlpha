using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private bool isControllerConnected = false;

    public GameObject bulletPrefab;
    public GameObject canvas;
    private Text weaponText;

    private CharacterController characterController;

    private float mass = 1.0f;

    private Vector3 velocity;

    //personal variables
    private float baseHealth = 10.0f;
    private float baseDamage = 1.0f;
    private float baseSpeed = 8.0f;
    private float baseFireRate = 0.2f;

    private float maxHealth;
    private float maxDamage;
    private float maxSpeed;
    private float maxFireRate;

    private float health;
    private float damage;
    private float speed;
    private float fireRate;

    //time until next shot
    private float fireCooldown = 0.0f;

    //time left in current dash
    private float currentDash = 0.0f;
    private float dashTime = 0.2f;

    //direction of dash
    private Vector3 dashDir = Vector3.zero;

    //time until next dash
    private float dashCooldown = 2.0f;
    private float currentDashCooldown = 0.0f;

    Chromosome playerGenes;

	void Start () {

        characterController = GetComponent<CharacterController>();
        canvas = GameObject.Find("CanvasObject");
        weaponText = canvas.GetComponentInChildren<Text>();

        maxHealth = baseHealth;
        maxDamage = baseDamage;
        maxSpeed = baseSpeed;
        maxFireRate = baseFireRate;

        health = maxHealth;
        damage = maxDamage;
        speed = maxSpeed;
        fireRate = maxFireRate;

        playerGenes = new Chromosome(0);

        if (Input.GetJoystickNames().Length > 0)
        {
            isControllerConnected = true;
        }
	}
	
	void Update () {
	
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;

        if (currentDash > 0)
            currentDash -= Time.deltaTime;

        if (currentDashCooldown > 0)
            currentDashCooldown -= Time.deltaTime;

        Vector3 movementVector = velocity;
        Vector3 lookVector = Vector3.zero;

        velocity *= 0.9f;

        if (currentDash > 0)
        {
            movementVector += dashDir;
        }
        else
        {
            if (isControllerConnected)
            {
                movementVector.x += Input.GetAxis("LeftStickX");
                movementVector.z += -Input.GetAxis("LeftStickY");
            }
            else
            {
                Vector3 newVelocity = Vector3.zero;

                if (Input.GetKey(KeyCode.A))
                {
                    newVelocity.x -= 1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    newVelocity.x += 1;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    newVelocity.z += 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    newVelocity.z -= 1;
                }

                movementVector += Vector3.Normalize(newVelocity);
            }

            if (Input.GetAxis("LeftTrigger") > 0.1f || Input.GetKeyDown(KeyCode.Space))
            {
                if (currentDashCooldown <= 0)
                {
                    currentDashCooldown = dashCooldown;

                    currentDash = dashTime;
                    dashDir = Vector3.Normalize(movementVector) * 3.0f;
                    movementVector += dashDir;

                    //apply screen shake
                }
            }
        }
        movementVector.y = 0.0f;
        characterController.Move(movementVector * speed * Time.deltaTime);
        characterController.transform.position = new Vector3(characterController.transform.position.x, 0, characterController.transform.position.z);

        if (isControllerConnected)
        {
            lookVector.z = -Input.GetAxis("RightStickY");
            lookVector.x = Input.GetAxis("RightStickX");
            lookVector.y = 0;

            if (lookVector.sqrMagnitude > 0.2f)
            {
                transform.rotation = Quaternion.LookRotation(lookVector);

                if (fireCooldown <= 0)
                {
                    GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
                    BulletScript script = newBullet.GetComponent<BulletScript>();
                    script.damage = damage;
                    script.speed = 15.0f;
                    fireCooldown = fireRate;
                }
            }
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.LookAt(mousePos);

            if (Input.GetMouseButton(0))
            {
                if (fireCooldown <= 0)
                {
                    GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
                    BulletScript script = newBullet.GetComponent<BulletScript>();
                    script.damage = damage;
                    script.speed = 15.0f;
                    fireCooldown = fireRate;
                }
            }
        }

        //if (Input.GetAxis("RightTrigger") > 0.1f)
        //{
        //    if (fireCooldown <= 0)
        //    {
        //        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
        //        BulletScript script = newBullet.GetComponent<BulletScript>();
        //        script.damage = damage;
        //        script.speed = 15.0f;
        //        fireCooldown = fireRate;
        //    }
        //}

        if (Input.GetButton("A"))
        {
            //charge
            weaponText.text = "Charge";
        }
        if (Input.GetButton("B"))
        {
            //spirit bomb
            weaponText.text = "Bomb";
        }
        if (Input.GetButton("X"))
        {
            //shield
            weaponText.text = "Shield";
        }
        if (Input.GetButton("Y"))
        {
            //laser
            weaponText.text = "Laser";
        }

	}

    void AddForce(Vector3 force)
    {
        Vector3 accel;
		accel = force / mass;
		velocity += accel;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        if (other.tag == "Nucleus")
        {
            playerGenes.AddChromosome(other.GetComponent<NucleusScript>().GetChromosome());
            Destroy(other.gameObject);
            return;
        }

        Vector3 normal = Vector3.Normalize(other.transform.position - gameObject.transform.position);
        Vector3 collisionVector = normal * (Vector3.Dot((other.transform.position - gameObject.transform.position) / 2, normal));
        Vector3 forceVector = collisionVector * (1.0f / ((1.0f / mass) + (1.0f / other.attachedRigidbody.mass))) * 1.2f;
        AddForce(-forceVector);

        

    }
}
