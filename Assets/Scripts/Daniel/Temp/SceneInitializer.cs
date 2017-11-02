using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour {
	private GameObject spaceship;
	private GameObject player;

	//Esse script depende da cena de Customização, pois é esperado receber um objeto com a tag "SpaceShip"
	void Awake(){
		spaceship = GameObject.FindGameObjectWithTag("SpaceShip");
		player = GameObject.FindGameObjectWithTag("Player");
		spaceship.transform.parent = player.transform;
		spaceship.transform.localPosition = new Vector3(0,0,0);
		spaceship.transform.Rotate(Vector3.forward, 90);
	}
}
