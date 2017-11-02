using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawn : MonoBehaviour
{

	public ObjectPooler[] objPoller;
	public float spawnRate;
	private float nextSpawn;
	private Vector3 oldPosition;
	private EnemyController enemyController;
    private PlayerController playerController;

	// Use this for initialization
	void Start ()
	{
		enemyController = FindObjectOfType<EnemyController>();
        playerController = PlayerController.PlayerControllerInstance;


    }
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextSpawn && playerController.IsAlive) {
			nextSpawn = Time.time + spawnRate;
			CreatingEnemies ();
		}
	}

	void CreatingEnemies ()
	{
		//int i = (int)Random.Range (0f, 2f);
		GameObject enemyObject = objPoller[0].GetPooledObject ();
		if (enemyObject == null) {
			return;
		}

		enemyObject.transform.position = transform.position + new Vector3 (Random.Range (-1f, 1.8f), Random.Range (-4.3f, 4.3f), 0f);
		enemyObject.transform.rotation = transform.rotation;
		enemyObject.SetActive (true);


	}
}