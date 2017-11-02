using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public RaycastHit2D[] hits;
    public List<Transform> list = new List<Transform>();
    private GameObject player;
    public LayerMask layers;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        list.Clear();
        hits = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position, 500, layers);
        foreach(RaycastHit2D x in hits) {
            list.Add(x.transform);
        }
        if(hits[0].transform.tag == "Player") {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            LookAt(hits[0].transform);
			GetComponent<EnemyShoot> ().SetShootPermission (true);
        }
        else {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
			GetComponent<EnemyShoot> ().SetShootPermission (false);
        }
        
    }

    void LookAt(Transform targetTransform) {
        Vector3 dir = targetTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
