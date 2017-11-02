using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour {
    [Range(0f, 1f)]
    public float damage;
    public GameObject sparklePrefab;
    private GameObject sparkle;

    void Start() {
        sparkle = Instantiate(sparklePrefab, transform.position, Quaternion.identity);
        sparkle.SetActive(false);
        sparkle.name = "Sparkle";
        //sparkle.transform.parent = transform;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            sparkle.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            sparkle.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.transform.tag == "Player")
        {
           // coll.transform.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            sparkle.transform.position = coll.transform.position;
        }
    }
}
