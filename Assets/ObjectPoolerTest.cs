using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerTest : MonoBehaviour {

    [System.Serializable]
    public class Objects
    {
        public GameObject objects;
        public int amount;
        public bool expand = false;
    }

    public List<Objects> objectsList;
    //private List<GameObject> pooledObjects;

	// Use this for initialization
	void Start () {
        //pooledObjects = new List<GameObject>();
        foreach (Objects objs in objectsList)
        {
            for (int i = 0; i < objs.amount; i++)
            {
                GameObject obj = Instantiate(objs.objects) as GameObject;
                obj.SetActive(false);
                //pooledObjects.Add(obj);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
