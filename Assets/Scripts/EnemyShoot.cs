using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {
	
	public float shootDelay;
	public Vector3 startPositionOffset;
	public ObjectPooler bullets;
	private bool allowedToShoot;

	private float nextFire;

    private PlayerController playerController;

	void Awake()
	{
	}

    void Start()
    {
		bullets = FindObjectOfType<ObjectPooler> ();
		allowedToShoot = false;
        playerController = PlayerController.PlayerControllerInstance;
	}

	void Update () {
		if (Time.time > nextFire && playerController.IsAlive && allowedToShoot) {
			nextFire = (Time.time + shootDelay);

			FireBullets ();
		}
	}

	public void FireBullets ()
	{
		if (bullets.CompareTag ("EnemyBullets")) {
			GameObject bulletObj = bullets.GetPooledObject ();
			if (bulletObj == null) {
				return;
			}
			bulletObj.transform.position = transform.position + startPositionOffset;
			bulletObj.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 180, transform.rotation.w);
			bulletObj.SetActive (true);
			SetShootPermission (true);
		}
	}

	public void SetShootPermission (bool shootingPermission)
	{
		allowedToShoot = shootingPermission;

	}

}
