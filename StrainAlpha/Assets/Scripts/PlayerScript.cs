using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Weapon
{
    SHIELD = 0,
    LASER = 1,
    BOMB = 2,
    CHARGE = 3,
}

public class PlayerScript : MonoBehaviour {

    private bool isControllerConnected = false;

    public GameObject bulletPrefab;

    private ShieldScript shield;
    private LaserScript laser;
    private ChargeScript charge;
    private BombScript bomb;

    private Weapon currentWeapon = Weapon.SHIELD;

    private CharacterController characterController;

    private float mass = 1.0f;

    private Vector3 velocity;

    //personal variables
    private float baseHealth = 10.0f;
    private float baseDamage = 2.0f;
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
    private float turnDamp = 0.0f;

    private float maxEnergy = 10.0f;
    private float energy;
    private float dashCost = 5.0f;

    //time until next shot
    private float fireCooldown = 0.0f;

    //time left in current dash
    private float currentDash = 0.0f;
    private float dashTime = 0.2f;

    //direction of dash
    private Vector3 dashDir = Vector3.zero;

    //time until next dash
    private float dashCooldown = 0.3f;
    private float currentDashCooldown = 0.0f;

    private float weaponSelectCooldown = 0.0f;

    private int nucleusLayer = 14;

    private bool laserActive = false;
    private bool shieldActive = false;
    private bool chargeActive = false;
    private bool bombActive = false;

    private float laserDrainSpeed = 0.05f;
    private float shieldDrainSpeed = 0.15f;
    private float chargeDrainSpeed = 0.25f;
    private float bombDrainSpeed = 0.05f;

    Chromosome playerGenes;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        //weaponText = canvas.GetComponentInChildren<Text>();

        shield = gameObject.GetComponentInChildren<ShieldScript>();
        laser = gameObject.GetComponentInChildren<LaserScript>();
        charge = gameObject.GetComponentInChildren<ChargeScript>();
        bomb = gameObject.GetComponentInChildren<BombScript>();

        maxHealth = baseHealth;
        maxDamage = baseDamage;
        maxSpeed = baseSpeed;
        maxFireRate = baseFireRate;

        health = maxHealth;
        damage = maxDamage;
        speed = maxSpeed;
        fireRate = maxFireRate;

        energy = maxEnergy;

        playerGenes = new Chromosome();
    }

	void Start () 
    {
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

        if (weaponSelectCooldown > 0)
            weaponSelectCooldown -= Time.deltaTime;

        if (energy < maxEnergy)
            energy += 3.0f * Time.deltaTime;

        if (energy > maxEnergy)
            energy = maxEnergy;

        if (health <= 0)
        {
            Die();
        }

        if (laserActive && playerGenes[2] > 0)
            playerGenes[2] -= Time.deltaTime * laserDrainSpeed; 
        else if (laserActive && playerGenes[2] <= 0)
        {
            laserActive = false;
            playerGenes[2] = 0.0f;

            if (laser.GetActive())
            {
                laser.SetActive(false);
                fireCooldown = 0.05f;
                moveDamp += 0.6f;
                turnDamp -= 0.15f;
                laserDrainSpeed -= 0.15f;
            }
        }

        if (chargeActive && playerGenes[3] > 0)
            playerGenes[3] -= Time.deltaTime * chargeDrainSpeed;
        else if (chargeActive && playerGenes[3] <= 0)
        {
            chargeActive = false;
            playerGenes[3] = 0.0f;

            if (charge.GetActive())
            {
                charge.SetActive(false);
                fireCooldown = 0.05f;
                moveDamp -= 0.5f;
                turnDamp -= 0.08f;
            }
        }

        if (shieldActive && playerGenes[0] > 0)
        {
            playerGenes[0] -= Time.deltaTime * shieldDrainSpeed;
        }
        else if (shieldActive && playerGenes[0] <= 0)
        {
            shieldActive = false;
            playerGenes[0] = 0.0f;
            shield.SetActive(false);
        }

        if (bombActive && playerGenes[1] > 0)
            playerGenes[1] -= Time.deltaTime * bombDrainSpeed;
        else if (bombActive && playerGenes[1] <= 0)
        {
            bombActive = false;
            playerGenes[1] = 0.0f;

            if (bomb.GetActive())
            {
                bomb.SetActive(false);
                fireCooldown = 0.05f;
            }
        }

        Vector3 movementVector = velocity;
        Vector3 lookVector = Vector3.zero;

        velocity *= 0.9f;

        if (chargeActive)
        {
            movementVector += transform.forward;
        }
        else if (currentDash > 0)
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
                if (currentDashCooldown <= 0 && energy > dashCost)
                {
                    currentDashCooldown = dashCooldown;
                    energy -= dashCost;

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
                Vector3 buttstuff = Vector3.zero;
                transform.rotation = Quaternion.LookRotation(Vector3.SmoothDamp(transform.forward, lookVector, ref buttstuff, turnDamp));

                if (!bombActive)
                {
                    if (fireCooldown <= 0)
                    {
                        ShootBullet();
                    }
                }
                else
                {
                    bombDrainSpeed = 0.2f;
                    bomb.ShootBullet();
                }
            }
            else if (bombActive)
            {
                bombDrainSpeed = 0.05f;
            }
        }
        else
        {
            Vector3 buttstuff = Vector3.zero;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.LookRotation(Vector3.SmoothDamp(transform.forward, Vector3.Normalize(mousePos - transform.position), ref buttstuff, turnDamp));
            //transform.LookAt(mousePos);

            if (Input.GetMouseButton(0))
            {
                if (!bombActive)
                {
                    if (fireCooldown <= 0)
                    {
                        bombDrainSpeed = 0.2f;
                        ShootBullet();
                    }
                }
            }
            else if (bombActive)
            {
                bombDrainSpeed = 0.05f;
            }
        }

        if (isControllerConnected)
        {
            if (Input.GetButton("RightBumper") && weaponSelectCooldown <= 0.0f)
            {
                ForwardCyclePower();
            }
            else if (Input.GetButton("LeftBumper") && weaponSelectCooldown <= 0.0f)
            {
                BackCyclePower();
            }

            if (Input.GetAxis("RightTrigger") > 0.1f)
            {
                //activate special power
                switch (currentWeapon)
                {
                    case Weapon.CHARGE:
                        if (!chargeActive)
                        {
                            if (playerGenes[3] < 0.8f)
                                break;
                        }
                        if (charge.GetActive())
                            break;
                        charge.SetActive(true);
                        fireCooldown = 1000.0f;
                        moveDamp += 0.5f;
                        turnDamp += 0.08f;
                        chargeActive = true;
                        break;
                
                    case Weapon.BOMB:
                        if (!bombActive)
                        {
                            if (playerGenes[1] < 0.8f)
                                break;
                        }
                        if (bomb.GetActive())
                            break;
                        bomb.SetActive(true);
                        fireCooldown = 1000.0f;
                        bombActive = true;
                        break;
                
                    case Weapon.SHIELD:
                        if (!shieldActive)
                        {
                            if (playerGenes[0] < 0.8f)
                                break;
                        }
                        if (shield.GetActive())
                            break;
                        shield.SetActive(true);
                        shieldActive = true;
                        break;
                
                    case Weapon.LASER:
                        if (!laserActive)
                        {
                            if (playerGenes[2] < 0.8f)
                                break;
                        }
                        if (laser.GetActive())
                            break;
                        laser.SetActive(true);
                        fireCooldown = 1000.0f;
                        moveDamp -= 0.6f;
                        turnDamp += 0.15f;
                        laserActive = true;
                        laserDrainSpeed += 0.15f;
                        break;
                
                    default:
                        break;
                }
            }
            else
            {
                if (laser.GetActive())
                {
                    laser.SetActive(false);
                    fireCooldown = 0.05f;
                    moveDamp += 0.6f;
                    turnDamp -= 0.15f;
                    laserDrainSpeed -= 0.15f;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && weaponSelectCooldown <= 0.0f)
            {
                ForwardCyclePower();
            }
            else if (Input.GetKeyDown(KeyCode.Q) && weaponSelectCooldown <= 0.0f)
            {
                BackCyclePower();
            }

            if (Input.GetMouseButton(1))
            {
                //activate special power
                switch (currentWeapon)
                {
                    case Weapon.CHARGE:
                        if (!chargeActive)
                        {
                            if (playerGenes[3] < 0.8f)
                                break;
                        }
                        if (charge.GetActive())
                            break;
                        charge.SetActive(true);
                        fireCooldown = 100.0f;
                        moveDamp += 0.5f;
                        turnDamp += 0.08f;
                        chargeActive = true;
                        break;
                
                    case Weapon.BOMB:
                        if (!bombActive)
                        {
                            if (playerGenes[1] < 0.8f)
                                break;
                        }
                        if (bomb.GetActive())
                            break;
                        bomb.SetActive(true);
                        fireCooldown = 1000.0f;
                        bombActive = true;
                        break;
                
                    case Weapon.SHIELD:
                        if (!shieldActive)
                        {
                            if (playerGenes[0] < 0.8f)
                                break;
                        }
                        if (shield.GetActive())
                            break;
                        shield.SetActive(true);
                        shieldActive = true;
                        break;
                
                    case Weapon.LASER:
                        if (!laserActive)
                        {
                            if (playerGenes[2] < 0.8f)
                                break;
                        }
                        if (laser.GetActive())
                            break;
                        laser.SetActive(true);
                        fireCooldown = 100.0f;
                        moveDamp -= 0.6f;
                        turnDamp += 0.15f;
                        laserActive = true;
                        laserDrainSpeed += 0.15f;
                        break;
                
                    default:
                        break;
                }
            }
            else
            {
                if (laser.GetActive())
                {
                    laser.SetActive(false);
                    fireCooldown = 0.05f;
                    moveDamp += 0.6f;
                    turnDamp -= 0.15f;
                    laserDrainSpeed -= 0.15f;
                }
            }
        }

        UpdateStats();
	}

    void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    /// <summary>
    /// returns health, maxHealth and baseHealth
    /// </summary>
    /// <returns></returns>
    public Vector3 GetHealth()
    {
        return new Vector3(health, maxHealth, baseHealth);
    }

    public Vector2 GetEnergy()
    {
        return new Vector2(energy, maxEnergy);
    }

    public void TakeDamage(float _damage)
    {
        if (!shieldActive)
            health -= _damage;
    }

    private void ShootBullet()
    {
        GameObject newBullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
        BulletScript script = newBullet.GetComponent<BulletScript>();
        script.damage = damage;
        script.speed = 15.0f;
        script.isEnemyBullet = false;
        fireCooldown = fireRate;
    }

    private void ForwardCyclePower()
    {
        switch (currentWeapon)
        {
            case Weapon.CHARGE:
                currentWeapon = Weapon.SHIELD;
                break;
        
            case Weapon.BOMB:
                currentWeapon = Weapon.CHARGE;
                break;
        
            case Weapon.SHIELD:
                currentWeapon = Weapon.LASER;
                break;
        
            case Weapon.LASER:
                currentWeapon = Weapon.BOMB;
                break;
        
            default:
                break;
        }
        weaponSelectCooldown = 0.3f;
    }

    private void BackCyclePower()
    {
        switch (currentWeapon)
        {
            case Weapon.CHARGE:
                currentWeapon = Weapon.BOMB;
                break;
        
            case Weapon.BOMB:
                currentWeapon = Weapon.LASER;
                break;
        
            case Weapon.SHIELD:
                currentWeapon = Weapon.CHARGE;
                break;
        
            case Weapon.LASER:
                currentWeapon = Weapon.SHIELD;
                break;
        
            default:
                break;
        }
        weaponSelectCooldown = 0.3f;
    }

    public float GetGene(int i)
    {
        return playerGenes.GetGenes()[i];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == nucleusLayer)
        {
            playerGenes.AddChromosome(other.GetComponent<NucleusScript>().GetChromosome());
            Destroy(other.gameObject);
            UpdateStats();
            return;
        }
    }

    void UpdateStats()
    {
        // health changing with playerGenes[0] goes here
        damage = baseDamage + playerGenes[1] * 3.0f;
        fireRate = baseFireRate - playerGenes[2] * 0.1f;
        speed = baseSpeed + playerGenes[3] * 3.0f;
    }
}
