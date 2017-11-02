using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerShootingController : MonoBehaviour {

    static PlayerShootingController playerSCInstance;
    public static PlayerShootingController PlayerSCInstance { get { return playerSCInstance; } }

    //public event Action FireBulletsEvent;   
    /* Delegate with void return type, no parameters.
       Same as: public delegate void FireBulletsDelegate();
       public event FireBulletsDelegate fireBulletsEvent;
    */
    

    [Header("Bullets Pooler Atts.")]
    [SerializeField] ObjectPooler[] bulletsObjectsPoller;

    [Header("Shoot Properties")]
    [SerializeField] Vector3[] singleBulletOffset = new Vector3[1];
    [SerializeField] Vector3[] doubleBulletOffset = new Vector3[2]; 
    [SerializeField] Vector3[] tripleBulletOffset = new Vector3[3];
    Vector3 missilePositionOffset;
    
    

    private int shootsFired;

    [Header("Classes Instances")]
    LevelManager levelManager;
    PlayerController playerController;

    void Awake()
    {
        playerSCInstance = this;
    }

    // Use this for initialization
    void Start () {
        // Set all other object instances
        levelManager = LevelManager.LevelManagerInstance;
        playerController = PlayerController.PlayerControllerInstance;

        shootsFired = 0;
	}

    void OnDisable()
    {
        levelManager.LevelShootsFired = shootsFired;
    }

    public void FireBullets()
    {
        //if (FireBulletsEvent != null) { FireBulletsEvent(); }

        GameObject bulletObj = bulletsObjectsPoller[0].GetPooledObject();
        if (bulletObj == null) return;

        if (!playerController.HasDoubleShoot && !playerController.HasTripleShoot)
        {
            bulletObj.transform.position = transform.position + singleBulletOffset[0];
            bulletObj.transform.rotation = transform.rotation;
            bulletObj.SetActive(true);
            shootsFired++;
        }
        else if (playerController.HasDoubleShoot && !playerController.HasTripleShoot)
        {
            GameObject bulletObj2 = bulletsObjectsPoller[0].GetPooledObject();
            if (bulletObj2 == null) return;
            bulletObj.transform.position = transform.position + doubleBulletOffset[0];
            bulletObj.transform.rotation = transform.rotation;
            bulletObj2.transform.position = transform.position + doubleBulletOffset[1];
            bulletObj2.transform.rotation = transform.rotation;
            bulletObj.SetActive(true);
            bulletObj2.SetActive(true);
            shootsFired += 2;
        }
        else if (playerController.HasTripleShoot)
        {
            playerController.HasDoubleShoot = false;
            GameObject bulletObj2 = bulletsObjectsPoller[0].GetPooledObject();
            if (bulletObj2 == null) return;
            GameObject bulletObj3 = bulletsObjectsPoller[0].GetPooledObject();
            if (bulletObj3 == null) return;
            bulletObj.transform.position = transform.position + tripleBulletOffset[0];
            bulletObj.transform.rotation = transform.rotation;
            bulletObj2.transform.position = transform.position + tripleBulletOffset[1];
            bulletObj2.transform.rotation = transform.rotation;
            bulletObj3.transform.position = transform.position + tripleBulletOffset[2];
            bulletObj3.transform.rotation = transform.rotation;
            bulletObj.SetActive(true);
            bulletObj2.SetActive(true);
            bulletObj3.SetActive(true);
            shootsFired += 3;
        }
    }

    public void FireMissile()
    {
        GameObject bulletObj = bulletsObjectsPoller[1].GetPooledObject();
        if (bulletObj == null)
        {
            return;
        }
        bulletObj.transform.position = transform.position + missilePositionOffset;
        bulletObj.transform.rotation = transform.rotation;
        bulletObj.SetActive(true);
    }

}

