using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BulletScript : MonoBehaviour
{

	public Vector3 bulletSpeed;
	public float bulletDamage;
	public GameObject sparklePrefab;

	void OnEnable ()
	{
		//Invoke ("DeactiveteBullets", 2f);
	}

	void OnDisable ()
	{
		//CancelInvoke ();
	}

	void OnTriggerEnter2D (Collider2D collider){
		if (collider.CompareTag ("Enemy")) {
			collider.GetComponent<EnemyController>().ReceiveDamage(bulletDamage);
			Instantiate(sparklePrefab, transform.position, Quaternion.identity);
			this.gameObject.SetActive (false);
		}else if (collider.CompareTag ("Bounds")) {
			this.gameObject.SetActive (false);
		}else if (collider.CompareTag ("Scenery")){
			Instantiate(sparklePrefab, transform.position, Quaternion.identity);
			this.gameObject.SetActive (false);
		}
	}

	void FixedUpdate ()
	{
		this.transform.position += bulletSpeed * Time.deltaTime;
	}

}
