using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController playerControllerInstance;
    public static PlayerController PlayerControllerInstance { get { return playerControllerInstance; } }

    [Header("Player Movement")]
    [SerializeField] bool clampPosition;
    [SerializeField] bool allowedToRotate = false;
    [SerializeField] Vector3 playerSpeed;
    private Vector3 maxPosition;
    private Vector3 minPosition;

    [Header("Player Health")]
    private bool isAlive;
    [SerializeField] float maximumHealth;
    [SerializeField] int playerLives;
    private float currentHealth;
    [SerializeField] float shieldHealth = 200f; // Temporary shield variable

    [Header("Player Shooting")]
    public float firingRate;    // Shoots per second
    public float missileRate;   // Missiles per second
    public float missileCountdown = 10f;
    [Range(0f,3f)] private float currentMissileTimer;
    private float nextFire, nextMissile;
    
    private bool allowedToShoot, allowFireMissile;
    public bool laserActivated;
    //public LineRenderer lineRenderer;

    [Header("Player Trails")]
    public bool trailTestMode;

    [Header("Player Upgrades")]
    public bool hasShield;
    public bool hasDoubleSpeed;
    public bool hasDoubleDamage;
    public bool hasDoubledFireRate, hasTripledFireRate;
    [SerializeField] bool hasDoubleShoot;
    [SerializeField] bool hasTripleShoot;
    private bool hasImprovedSecondaryGun;
    private bool hasPlasmaAvailable;
    private bool hasInvisibility;
    private bool isTakingDoubleDamage, isTakingTripledDamaged;

    [Header("Other Objects")]
    public ParticleSystem trailParticle;
    private Sprite playerSpriteSelected;
    public GameObject explosionPrefab;
    private LevelManager levelManager;
    private GameManager gameManager;
    private EnemyController enemyController;
    private FieldOfView fieldOfView;
    PlayerShootingController playerSController;

    // Events and Delegates
    public static event Action<float, float> PlayerHealthEvt;
    public static event Action OnPlayerDeathEvt;


    // --------->> Save/Load stats <<-------
    // Save player stats
    public void Save()
    {
        SaveAndLoad.SavePlayer(this);
    }
    // Load all player stats
    public void Load()
    {
        float[] loadedStats = SaveAndLoad.LoadPlayer();
        currentHealth = loadedStats[0];
        maximumHealth = loadedStats[1];
        playerLives = (int)loadedStats[2];

    }
    // Delete all player stats
    public void Delete()
    {
        SaveAndLoad.DeleteSavedPlayer();
    }

    void Awake()
    {
        playerControllerInstance = this;
        fieldOfView = GetComponentInChildren<FieldOfView>();
        currentHealth = maximumHealth;
        currentMissileTimer = missileCountdown;
        hasShield = false;
    }

    // Called everytime the object becomes enable
    void OnEnable()
    {
        isAlive = true;
        allowedToShoot = true;
    }

    // Called everytime the object becomes disable
    void OnDisable()
    {
        
    }

    // Use this for initialization only
    void Start()
    {
        gameManager = GameManager.GameManagerInstance;
        levelManager = LevelManager.LevelManagerInstance;
        enemyController = EnemyController.EnemyControllerInstance;
        playerSController = PlayerShootingController.PlayerSCInstance;
        if (PlayerHealthEvt != null) PlayerHealthEvt(currentHealth, maximumHealth);    // Call all subscribers to the PlayerHealth 
        LevelManager.GameStateEvt += Update;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        // Iterates over the shooting inputs and commands
        StartCoroutine("ShootingCommand");

        if (currentHealth <= 0.02f && isAlive)
            OnPlayerDeath();

        PlayerTrusterEffect();
        
    }

    // FixedUpdate is called once every fixed framerate frame
    void FixedUpdate()
    {
        // Move the player using standart inputs
        float verticalPosition = Input.GetAxis("Vertical") * playerSpeed.y * Time.deltaTime;
        float horizontalPosition = Input.GetAxis("Horizontal") * playerSpeed.x * Time.deltaTime;
        this.transform.Translate(horizontalPosition, verticalPosition, 0f);
        if (allowedToRotate)
            TurningMoviment();
        else
            transform.rotation = Quaternion.Euler(Vector3.zero);
        // Restrict the position of the player
        if (clampPosition)
        {
            ClampedPosition();
            float current_X = Mathf.Clamp(this.transform.position.x, minPosition.x, maxPosition.x);
            float current_Y = Mathf.Clamp(this.transform.position.y, minPosition.y, maxPosition.y);
            this.transform.position = new Vector3(current_X, current_Y, this.transform.position.z);
        }
    }

    /// <summary>
    /// Definition to clamp the player's position
    /// related to the 2D camera and its sprite.
    /// </summary>
    void ClampedPosition()
    {
        float zDistanceBetweenCameraAndObject = this.transform.position.z - Camera.main.transform.position.z;

        // Return a world point coordinate
        Vector3 xLeftMinPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistanceBetweenCameraAndObject));
        Vector3 xRightMaxPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zDistanceBetweenCameraAndObject));
        Vector3 yBottomMinPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistanceBetweenCameraAndObject));
        Vector3 yTopMaxPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, zDistanceBetweenCameraAndObject));

        // Gets the half of the sprite size, starting from the center
        float xHalfSpriteSize = this.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        float yHalfSpriteSize = this.GetComponentInChildren<SpriteRenderer>().bounds.extents.y;
        minPosition.x = xLeftMinPosition.x - xHalfSpriteSize;
        maxPosition.x = xRightMaxPosition.x + xHalfSpriteSize;
        minPosition.y = yBottomMinPosition.y - yHalfSpriteSize;
        maxPosition.y = yTopMaxPosition.y + yHalfSpriteSize;
    }

    void TurningMoviment ()
	{
        Vector3 playerToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float rotationAngle = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
	}

    /// <summary>
    /// Contains all the player's shooting commands
    /// and all its permissions.
    /// </summary>
    IEnumerator ShootingCommand()
    {
        // TODO Find better solution for the following condition
        if (currentMissileTimer <= 0.02f) currentMissileTimer = 0f;
        allowFireMissile = currentMissileTimer <= 0f && fieldOfView.IsClosestTargetInSight /*&& !levelManager.IsGamePaused*/ ? true : false;

        if (Input.GetButton("Fire1") && Time.time > nextFire && allowedToShoot)
        {
            nextFire = Time.time + (1 / firingRate);
            allowedToShoot = true;
            playerSController.FireBullets();
            yield break;
        }

        if (allowFireMissile && Input.GetButtonDown("Fire2"))
        {
            fieldOfView.IsTargetLockedOn = true;
            nextFire = Time.time + missileRate;
            currentMissileTimer = missileCountdown;
            allowedToShoot = false;
            playerSController.FireMissile();
            yield return new WaitForSeconds(1f);    // Restart shooting delay
            allowedToShoot = true;
            yield break;
        }
        currentMissileTimer -= Time.deltaTime;
        yield return null;
    }
    
    /// <summary>
    /// Defines the trusters' animation state and
    /// sets the force which moves the particles.
    /// </summary>
    void PlayerTrusterEffect()
    {
        // Plays the particle system when the player is moving
        if (!trailTestMode)
        {
            if (Input.GetAxis("Horizontal") <= 0f && Input.GetAxis("Vertical") == 0f)
                trailParticle.Stop();
            else
                trailParticle.Play();
        }

        // Sets the force over the particles when player is moving vertically
        var _TPForce = trailParticle.forceOverLifetime; // Force applyed to the particles during its lifetime
        if (Input.GetAxis("Vertical") > 0f)
            _TPForce.y = new ParticleSystem.MinMaxCurve(Input.GetAxis("Vertical") * -8f);
        else if (Input.GetAxis("Vertical") < 0f)
            _TPForce.y = new ParticleSystem.MinMaxCurve(Input.GetAxis("Vertical") * -8f);
        else
            _TPForce.y = new ParticleSystem.MinMaxCurve(0f);
    }

    void TakeDamage(float damage)
    {
        float damageTaken = damage;
        CurrentPlayerHealth(damageTaken);
    }

    void CurrentPlayerHealth(float _damageTaken)
    {
        if (hasShield)
            currentHealth -= _damageTaken / 2;
        else
            currentHealth -= _damageTaken;
        if (PlayerHealthEvt != null) PlayerHealthEvt(currentHealth, maximumHealth);
    }

    void OnPlayerDeath()
    {
        if (OnPlayerDeathEvt != null) OnPlayerDeathEvt();
        allowedToShoot = false;
        allowFireMissile = false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        isAlive = false;
        this.gameObject.SetActive(false);
    }


    /*
     *  ==========================================
     *  -------->> TRIGGERS/COLLISIONS <<---------
     *  ==========================================
     */
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("EnemyBullets"))
        {
            TakeDamage(_EnemyBulletScript.enemyBulletDamage);
        }
        return;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("BackBound"))
        {
            allowedToShoot = true;
        }
    }


    


    //public void FireLaser()
    //{
    //    Debug.Log("Laser");
    //    if (!lineRenderer.enabled)
    //    {
    //        lineRenderer.enabled = true;
    //        //impactEffect.Play();
    //        // impactLight.enabled = true;
    //        Debug.Log("Line");
    //    }
    //   // if (fieldOfView.ClosestTarget)
    //   // {
    //        lineRenderer.SetPosition(0, transform.position);
    //       // lineRenderer.SetPosition(1, fieldOfView.ClosestTarget.position);

    //  //  }

    //   // Vector3 dir = firePoint.position - target.position;

    //    //impactEffect.transform.position = target.position + dir.normalized;

    //    //impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    //}

    // -------->> Getters/Setters <<---------
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
    }
    public float MaximumHealth
    {
        get { return maximumHealth; }
    }
    public bool HasShield
    {
        get { return hasShield; }
        set { hasShield = value; }
    }
    public float ShieldHealth
    {
        get { return shieldHealth; }
        set { shieldHealth = value; }
    }
    public bool AllowShooting
    {
        get { return allowedToShoot; }
        set { allowedToShoot = value; }
    }
    public bool HasDoubleShoot { get { return hasDoubleShoot; } set { hasDoubleShoot = value; } }
    public bool HasTripleShoot { get { return hasTripleShoot; } set { hasTripleShoot = value; } }
    public bool AllowMissile
    {
        get { return allowFireMissile; }
        set { allowFireMissile = value; }
    }
    public float MissileTimer
    {
        get { return currentMissileTimer; }
    }

}
