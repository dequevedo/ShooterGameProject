using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class ModuleManager : MonoBehaviour {
	public GameObject player;
	public int moduleAmount;
	//Contém os 3 Modulos ativos atualmente na cena. o segundo é o módulo que o player está atualmente, o terceiro e primeiro módulo são, respectivamente, o módulo anterior ao player e o próximo módulo
	private GameObject[] temp = new GameObject[3];
	//Contém os Prefabs dos Modulos (TileMapGroup)
	public GameObject[] prefabs; 
	private float actualCoordinates = 0;

	//Chama a função ciclo caso o player saia da área do módulo de cenário atual
	public void Update(){
		
		if(player.transform.position.x + 25.5f >= actualCoordinates){
			Cycle();
		}
	}

	//Faz um ciclo nos módulos ativos
	public void Cycle(){
		GameObject newModule = Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(actualCoordinates, 0, 0), Quaternion.identity);
		newModule.transform.parent = transform;
		actualCoordinates += 25.5f;
		Destroy(temp[0]);
		temp[0] = temp[1];
		temp[1] = temp[2];
		temp[2] = newModule;
	}
}
