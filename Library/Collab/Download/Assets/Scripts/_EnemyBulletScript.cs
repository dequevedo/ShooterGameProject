using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyBulletScript : MonoBehaviour
{

	public Vector3 bulletSpeed;
	public int enemyBDamage;
	public static int enemyBulletDamage;
	public GameObject sparkle;

	void OnEnable ()
	{
		//Invoke ("DeactiveteBullets", 2f);
	}

	void OnDisable ()
	{
		//CancelInvoke ();
	}

	void Start()
	{
		enemyBulletDamage = enemyBDamage;
	}

	void OnTriggerEnter2D (Collider2D collider){
		if (collider.CompareTag ("Bounds") || collider.CompareTag ("BackBound") || collider.CompareTag ("Player")) {
			this.gameObject.SetActive (false);
			//Instantiate (sparkle, transform.position, Quaternion.identity);
		}
	}

	void FixedUpdate ()
	{
		//this.transform.position += bulletSpeed * Time.deltaTime;
        transform.Translate(bulletSpeed * Time.deltaTime, Space.Self);
	}

}
