using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private static EnemyController enemyControllerInstance;
    public static EnemyController EnemyControllerInstance { get { return enemyControllerInstance; } }

    [Header("Enemy Stats")]
    public int points;


    [Header("Enemy Health")]
    private bool isAlive;
    public float maximumHealth;
    private float currentHealth;

    [Header("Enemy Shooting")]
    public float fireRate;
    private float nextShoot;
    public Vector3 startPositionOffset;
    private GameObject enemyBullets;
    private ObjectPooler enemyBulletsPooler;
    private bool allowedToShoot = true;
    public bool rotateAround = false;
    public float rotateSpeed;


    [Header("Other Objects")]
    public GameObject explosionPrefab;
    private GameManager gameManager;
    private LevelManager levelManager;


    void Awake()
    {
        enemyControllerInstance = this;
    }

    void Start()
    {
        gameManager = GameManager.GameManagerInstance;
        levelManager = LevelManager.LevelManagerInstance;
        enemyBullets = GameObject.FindGameObjectWithTag("EnemyBullets");
        enemyBulletsPooler = enemyBullets.GetComponent<ObjectPooler>();
    }

    private void OnEnable()
    {
        currentHealth = maximumHealth;
        allowedToShoot = true;
    }

    private void OnDisable()
    {
    }

    void Update()
    {
        if (levelManager.IsGamePaused || levelManager.HasLevelEnded) return;

        if (Time.time > nextShoot && allowedToShoot)
        {
            nextShoot = (Time.time + fireRate);
            FireBullets();
        }


        if (currentHealth <= 0)
        {
            OnDeath();
          //  currentHealth = maximumHealth;
        }
    }

    private void FixedUpdate()
    {
        if (rotateAround)
        {
            transform.RotateAround(transform.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }

    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
    }

    public static event Action<int> OnEnemyDeathEvt;
    public void OnDeath()
    {
        if (OnEnemyDeathEvt != null) OnEnemyDeathEvt(points);
        allowedToShoot = false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        gameObject.transform.parent = null;     // Avoid parent not being removed
        this.gameObject.SetActive(false);
    }

    void DisableEnemy()
    {
        allowedToShoot = false;
        this.gameObject.SetActive(false);

        //gameObject.transform.parent = null;     // Avoid parent not being removed
    }

    public void FireBullets()
    {
        GameObject enemyBulletObject = enemyBulletsPooler.GetPooledObject();
        if (enemyBulletObject == null) return;
        enemyBulletObject.transform.position = transform.position + startPositionOffset;
        enemyBulletObject.transform.rotation = transform.rotation;
        enemyBulletObject.SetActive(true);
    }

    public bool AllowShooting
    {
        get { return allowedToShoot; }
        set { allowedToShoot = value; }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("BackBound"))
        {
            allowedToShoot = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bounds"))
        {
            allowedToShoot = true;
        }
        if (col.CompareTag("EnemySpawn"))
        {
            allowedToShoot = true;
        }
    }
}
