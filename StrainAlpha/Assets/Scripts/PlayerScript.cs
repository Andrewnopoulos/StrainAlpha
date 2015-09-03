using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private bool isControllerConnected = false;

    public GameObject bulletPrefab;
    public GameObject canvas;
    private Text weaponText;

    private ShieldScript shield;
    private LaserScript laser;

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

    private float moveDamp = 1.0f;
    private float turnDamp = 1.0f;

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

    private int nucleusLayer = 14;

    Chromosome playerGenes;

	void Start () {

        characterController = GetComponent<CharacterController>();
        canvas = GameObject.Find("CanvasObject");
        weaponText = canvas.GetComponentInChildren<Text>();

        shield = gameObject.GetComponentInChildren<ShieldScript>();
        laser = gameObject.GetComponentInChildren<LaserScript>();

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
        characterController.Move(movementVector * speed * moveDamp * Time.deltaTime);
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

        if (isControllerConnected)
        {
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

            if (Input.GetAxis("RightTrigger") > 0.1f)
            {
                //activate special power
                switch (weaponText.text)
                {
                    case "Charge":
                        break;

                    case "Bomb":
                        break;

                    case "Shield":
                        shield.SetActive(true);
                        break;

                    case "Laser":
                        laser.SetActive(true);
                        fireCooldown = laser.laserTime;
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                //charge
                weaponText.text = "Charge";
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                //spirit bomb
                weaponText.text = "Bomb";
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                //shield
                weaponText.text = "Shield";
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                //laser
                weaponText.text = "Laser";
            }

            if (Input.GetMouseButton(1))
            {
                //activate special power
            }
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
        if (other.gameObject.layer == nucleusLayer)
        {
            playerGenes.AddChromosome(other.GetComponent<NucleusScript>().GetChromosome());
            Destroy(other.gameObject);
            return;
        }

        //obsolete bounce code
        //Vector3 normal = Vector3.Normalize(other.transform.position - gameObject.transform.position);
        //Vector3 collisionVector = normal * (Vector3.Dot((other.transform.position - gameObject.transform.position) / 2, normal));
        //Vector3 forceVector = collisionVector * (1.0f / ((1.0f / mass) + (1.0f / other.attachedRigidbody.mass))) * 0.5f;
        //AddForce(-forceVector);

    }
}
