using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Worm : MonoBehaviour {
	public float offset;
	
	void Start(){

	}
	void Update(){
		
	}
}
/*
	public GameObject bodyPart;
	[Range(0, 20)]
	public int bodyPartsAmount;
	public float offset;
	private GameObject child;
	private GameObject temp;

	void Start () {
		for(int i=0; i<bodyPartsAmount; i++){
			GameObject obj = Instantiate(bodyPart, new Vector3(transform.position.x + (offset*(i+1)), transform.position.y, transform.position.z), Quaternion.identity);
			if(child == null){
				child = obj;
				temp = obj;
			} 
			temp.GetComponent<WormChild>().child = obj;
			obj.name = "BodyPart[" + i + "]";
		}
	}*/