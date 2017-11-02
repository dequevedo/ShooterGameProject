using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormChild : Worm {
	public int index;
	public Transform head;
	public Transform parent;
	public Transform child;

	public float smooth;

	void Start () {
		try{
			parent = head.GetComponent<WormHead>().children[index-1];
		}catch(Exception e){
		}
		try{
			child = head.GetComponent<WormHead>().children[index+1];
		}catch(Exception e){
		}
					

	}
	
	void Update () {
		Vector3 specificVector = new Vector3(parent.transform.position.x, parent.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, specificVector, smooth * Time.deltaTime);
	}
}
