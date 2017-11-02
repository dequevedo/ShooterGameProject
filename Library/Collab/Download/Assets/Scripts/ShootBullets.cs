using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullets : MonoBehaviour
{
	public float fireTime, misslieTime, firingRate;
	//public GameObject bullet;
	public Vector3 startPositionOffset;
	public ObjectPooler[] bulletsPoller;
	public bool hasDoubleFireRate = false;
    public bool hasTripleFireRate = false;
	private bool allowedMissile, missileFired;

	void Awake () {
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

//	public bool MissilePermission {
//		get { return allowedMissile; }
//		set { allowedMissile = value; }
//	}

	public bool GetMissilePermission () {
		return allowedMissile;
	}
	public void SetMissilePermission (bool allowMissile) {
		allowedMissile = allowMissile;
	}

	public void FireBullets ()
	{
		GameObject bulletObj = bulletsPoller[0].GetPooledObject();
		GameObject bulletObj2 = bulletsPoller[0].GetPooledObject();
        GameObject bulletObj3 = bulletsPoller[0].GetPooledObject();
		if (bulletObj == null) {
			return;
		}
		if (bulletObj2 == null) {
			return;
		}
        if (hasTripleFireRate)
        {
            bulletObj.transform.position = transform.position + startPositionOffset + new Vector3(0.2f, 0f, 0f);
            bulletObj.transform.rotation = transform.rotation;
            bulletObj2.transform.position = transform.position + startPositionOffset + new Vector3(0f, 0.4f, 0f);
            bulletObj2.transform.rotation = transform.rotation;
            bulletObj3.transform.position = transform.position + startPositionOffset + new Vector3(0f, -0.4f, 0f);
            bulletObj3.transform.rotation = transform.rotation;
            bulletObj.SetActive(true);
            bulletObj2.SetActive(true); bulletObj3.SetActive(true);
        }
		if (hasDoubleFireRate) {
			bulletObj.transform.position = transform.position + startPositionOffset + new Vector3(0f, 0.4f,0f);
			bulletObj.transform.rotation = transform.rotation;
			bulletObj2.transform.position = transform.position + startPositionOffset + new Vector3(0f, -0.4f, 0f);
			bulletObj2.transform.rotation = transform.rotation;
			bulletObj.SetActive (true);
			bulletObj2.SetActive (true);
		} else
        {
            bulletObj.transform.position = transform.position + startPositionOffset;
            bulletObj.transform.rotation = transform.rotation;
            bulletObj.SetActive(true);
        }
		
	}

	public void FireMissile ()
	{
		GameObject bulletObj = bulletsPoller[1].GetPooledObject();
		if (bulletObj == null) {
			return;
		}
		bulletObj.transform.position = transform.position + startPositionOffset;
		bulletObj.transform.rotation = transform.rotation;
		bulletObj.SetActive (true);
	}

}
