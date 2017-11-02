using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
	public GameObject explosion;
	public GameObject objectToDestroy;
	public void ExplodeObject() {
		Instantiate(explosion, transform.position, Quaternion.identity);
		Destroy(objectToDestroy);
	}
}
