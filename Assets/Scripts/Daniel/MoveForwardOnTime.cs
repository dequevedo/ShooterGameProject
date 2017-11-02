using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardOnTime : MonoBehaviour {
	public float speed;
	void Update () {
		transform.Translate(new Vector3(Time.deltaTime * speed, 0 ,0));
	}
}
