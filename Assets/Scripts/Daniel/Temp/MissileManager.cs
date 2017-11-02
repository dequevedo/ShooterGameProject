using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour {

	public GameObject missilePrefab;
	void Start () {
		GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
		missile.GetComponent<_MissileScript>().targetTag = "Enemy";
	}
}
