using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

	public static ObjectPooler current;	// Keep a reference to itself
	public GameObject pooledObject;
	public int pooledObjectsAmount;
	public bool expandPooledObjects;

	List<GameObject> pooledObjectsList;

	void Awake ()
	{
		current = this;
	}

	// Use this for initialization
	void Start ()
	{
		pooledObjectsList = new List<GameObject> ();
		for (int i = 0; i < pooledObjectsAmount; i++) {
			GameObject obj = Instantiate (pooledObject) as GameObject;
			obj.SetActive (false);
			pooledObjectsList.Add (obj);
		}
	}

	public GameObject GetPooledObject ()
	{
		// Returns every deactiveted object when needed.
		for (int i = 0; i < pooledObjectsList.Count; i++) {
			if (!pooledObjectsList [i].activeInHierarchy) {
				pooledObjectsList [i].SetActive (true);
				return pooledObjectsList [i];
			}
		}

		// If it's allowed to expand the amount of objects in the pool.
		if (expandPooledObjects) {
			GameObject obj = Instantiate (pooledObject) as GameObject;
			pooledObjectsList.Add (obj);
			return obj;
		} 

		// If both conditions above aren't reached, nothing is returned.
		return null;
	}
}
