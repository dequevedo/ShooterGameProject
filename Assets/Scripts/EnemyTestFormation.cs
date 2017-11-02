using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestFormation : MonoBehaviour {

	private float nextSpawn, spawnRate = 1f;
	public ObjectPooler objPoller;
	Vector3 offset;

	void Awake () {
		//offset = new Vector3 (0f, Random.Range (0.1f, 2f), 0f);
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("CreatingEnemies", 0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
//		if (Time.time > nextSpawn) 
//		{
//			nextSpawn = Time.time + spawnRate;
//			offset -= new Vector3 (0f, 10f, 0f) * Time.deltaTime;
//		}

	}

	void CreatingEnemies ()
	{
		GameObject bulletObj = objPoller.GetPooledObject ();
		if (bulletObj == null) {
			return;
		}
		bulletObj.transform.position = transform.position + new Vector3 (0f, Random.Range (-3f, 5f), 0f);
		bulletObj.transform.rotation = transform.rotation;
		bulletObj.SetActive (true);
	}
}
