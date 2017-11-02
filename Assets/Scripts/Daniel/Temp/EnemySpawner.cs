using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemy;
    private float initialX = 25.5f;
    private float initialY = 6.5f;
    void Start()
    {
        transform.position = new Vector2(initialX, initialY);
        StartCoroutine(InstantiateCoroutine());
    }

    IEnumerator InstantiateCoroutine()
    {
        yield return new WaitForSeconds(1f);
		Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
		StartCoroutine(InstantiateCoroutine());
    }
}
