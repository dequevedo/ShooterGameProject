using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour {

    private bool hasSpawned;

	// Use this for initialization
	void Start () {
        hasSpawned = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (hasSpawned)
        {
            Debug.Log("Waiting to spawn");
            StartCoroutine(SpeedBoostSpawn());
            return;
        }
        else
        {
            Instantiate(this, new Vector3(Random.Range(9f, 11f), Random.Range(-5f, 5f), 0f), transform.rotation);
            hasSpawned = true;
        }
        Debug.Log(hasSpawned);
	}

    IEnumerator SpeedBoostSpawn()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Waiting spawn");
        hasSpawned = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        } return;
    }
}
