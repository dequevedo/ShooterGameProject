using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MissileScript : MonoBehaviour {
	
	public float maxMissileSpeed = 2f;
	
	[Range(0,10)]
	public float rotateSpeed;
	public float missileDamage;
	public string targetTag;
	public GameObject explosionPrefab;
    private FieldOfView fieldOfView;
    private PlayerController playerController;

	private float actualMissileSpeed;
	private float timeToAchieveInitialSpeed = 2; 
	private float counter = 0;
	private GameObject enemy;

    private void Start()
    {
        fieldOfView = FieldOfView.FieldOfViewInstance;
        playerController = PlayerController.PlayerControllerInstance;
    }

    void Update(){
		//EnemyDefine();

		//Responsável pelo delay inicial na velocidade do míssil
		counter += Time.deltaTime * 2f;
		if(counter <= maxMissileSpeed){
			actualMissileSpeed = counter;
		}else{
			actualMissileSpeed = maxMissileSpeed;
		}
        Transform missileTarget = fieldOfView.ClosestTarget;
        //Gira em direção ao inimigo, caso ele nao seja nulo
        if (missileTarget)
        {
            var dir = missileTarget.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.time * ((actualMissileSpeed / maxMissileSpeed) * (rotateSpeed / 100)));
        }
        //Move para frente
        transform.Translate(Vector3.right * actualMissileSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, playerController.missileEnemyLockDown.position, actualMissileSpeed*Time.fixedDeltaTime);
    }

    //Procura o inimigo mais próximo que tenha a targetTag
    public void EnemyDefine(){ 
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag(targetTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos){
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = go;
				distance = curDistance;
			}
		}
		enemy = closest;
	}

	//Desenha linhas na Scene para debugar
	void OnDrawGizmos() {
        if (enemy != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, enemy.transform.position);
        }
    }

	//Define o que acontece quando o missil colide com algo
	void OnTriggerEnter2D (Collider2D collider){
		if (collider.CompareTag (targetTag)) {
            //if(targetTag == "Player") collider.GetComponent<PlayerController>().TakeDamage(missileDamage);
            if (targetTag == "Enemy") { collider.GetComponent<EnemyController>().ReceiveDamage(missileDamage); fieldOfView.IsTargetLockedOn = false; }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			counter = 0;
			this.gameObject.SetActive (false);

		} else if (collider.CompareTag ("Scenery")){
			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			counter = 0;
			this.gameObject.SetActive (false);
            
		}
		
	}
}
