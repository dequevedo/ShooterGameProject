using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class TileMapGroupEnvironment : MonoBehaviour {
	public float tileLength = 25.5f;
	public float tileHeight = 13f;

	public bool generateProps;
	public GameObject[] centerBack;
	public GameObject[] centerFront;
	public GameObject[] upBack;
	public GameObject[] upFront;
	public GameObject[] downBack;
	public GameObject[] downFront;

	//public TilemapGroup tmg;

	private bool[] sortingOrderAvailable;

	int FindAvailableSortingOrder(){
		for(int i = 0; i<sortingOrderAvailable.Length; i++){
			if(sortingOrderAvailable[i] == true){
				sortingOrderAvailable[i] = false;
				return i;
			}
		}
		return 0;
	}

	private void InitializeSortingOrders(){
		sortingOrderAvailable = new bool[10];
		for(int i = 0; i<sortingOrderAvailable.Length; i++){
			sortingOrderAvailable[i] = true;
		}
	}

	void InstantiateProp(GameObject[] gameObjPositionType, float yPosition, float zMin, float zMax, int multiplier){
		GameObject tempObj = Instantiate(
					gameObjPositionType[Random.Range(0, gameObjPositionType.Length)], 
					new Vector3(
						transform.position.x + Random.Range(0f, tileLength)
						,yPosition
						,Random.Range(zMin, zMax)), 
					Quaternion.identity);
		tempObj.GetComponent<SpriteRenderer>().sortingOrder = (10 + FindAvailableSortingOrder()) * multiplier;
		tempObj.transform.parent = transform;
	}

	void Awake(){
		InitializeSortingOrders();
		//tmg = GetComponentInChildren<TilemapGroup>(); //Utilizar posteriormente para pegar o tamanho do tile;

/*
		foreach(Tilemap x in tmg.Tilemaps){
			min = x.MapBounds.min.x;
		}
		foreach(Tilemap x in tmg.Tilemaps){
			max = x.MapBounds.max.x;
		}*/
		if(generateProps) GenerateProps();
	}

	public void GenerateProps(){
		int propAmount = Random.Range(0, 5);
		for(int i = 0; i < propAmount; i++){
			switch(Random.Range(0, 6)){
				case 0:
				InstantiateProp(centerBack, tileHeight/2, 0.5f, 2f, -1);
				break;
				case 1:
				InstantiateProp(centerFront, tileHeight/2, -0.5f, -2f, 1);
				break;
				case 2:
				InstantiateProp(upBack, tileHeight, 0.5f, 2f, -1);
				break;
				case 3:
				InstantiateProp(upFront, tileHeight, -0.5f, -2f, 1);
				break;
				case 4:
				InstantiateProp(downBack, 0, 0.5f, 2f, -1);
				break;
				case 5:
				InstantiateProp(downFront, 0, -0.5f, -2f, 1);
				break;
			}
		}
	}
}
