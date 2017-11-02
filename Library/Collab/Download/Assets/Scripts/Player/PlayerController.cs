using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	// Public Variables:
	// public GameObject projectile;
	public float xSpeed, ySpeed, /*projectileSpeed,*/ firingRate, missileRate;
	public float maxHealth;
	public float actualHealth = 500;
	public float missileTime = 11.0f;
	public bool clampPosition;

	// Private Variables:
	private float xMinPosition, xMaxPosition, yMinPosition, yMaxPosition;
	private float nextFire, nextMissile;
	private bool allowedToShoot, allowFireMissile;
	public static bool isDead;

	// Public Objects:
	// public AudioClip fireSound;
	public EnvironmentTriggers scenario;
	public GameObject explosionPrefab;
	public Text missileTimerText;

	// Private Objects:
	private ShootBullets bulletsFired;
	// private GameManager gameManager;
	private SpriteRenderer playerSprite;
	private Color playerDefaultColor;
	private PlayerHealth my_Health;

	public void Save()
	{
		SaveAndLoad.SavePlayer (this);
	}

	public void Load()
	{
		int[] loadedStats = SaveAndLoad.LoadPlayer ();
		actualHealth =(float) loadedStats [0];
	}

	void Awake ()
	{
		
		//Load ();

	}

	// Use this for initialization
	void Start ()
	{
		my_Health = gameObject.GetComponent<PlayerHealth> ();
		missileTimerText.text = missileTime.ToString ("0");
		//actualHealth = maxHealth;
		isDead = false;
		GetDefaultSpriteAndColor ();
//		gameManager = GameObject.FindObjectOfType<GameManager> ();
		float zDistanceBetweenCameraAndObject = this.transform.position.z - Camera.main.transform.position.z;

		// Return a world point coordinate
		Vector3 xLeftMinPosition = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, zDistanceBetweenCameraAndObject));
		Vector3 xRightMaxPosition = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, zDistanceBetweenCameraAndObject));
		Vector3 yBottomMinPosition = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, zDistanceBetweenCameraAndObject));
		Vector3 yTopMaxPosition = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, zDistanceBetweenCameraAndObject));

		// Gets the half of the sprite size, starting from the center
		float xHalfSpriteSize = this.GetComponentInChildren<SpriteRenderer> ().bounds.extents.x;
		float yHalfSpriteSize = this.GetComponentInChildren<SpriteRenderer> ().bounds.extents.y;
		xMinPosition = xLeftMinPosition.x - xHalfSpriteSize;
		xMaxPosition = xRightMaxPosition.x - xHalfSpriteSize;
		yMinPosition = yBottomMinPosition.y + yHalfSpriteSize;
		yMaxPosition = yTopMaxPosition.y - yHalfSpriteSize;

		// Access the script of the bullets
		bulletsFired = gameObject.GetComponent<ShootBullets> ();

		allowedToShoot = true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isDead) {
			missileTimerText.text = missileTime.ToString ("0");
			missileTime -= Time.deltaTime;
			if (missileTime <= 0.5f) {
				allowFireMissile = true;
				missileTimerText.gameObject.SetActive (false);
			} else {
				allowFireMissile = false;
				missileTimerText.gameObject.SetActive (true);
			}


			if (Input.GetButton ("Fire1") && Time.time > nextFire && allowedToShoot) {
				nextFire = Time.time + firingRate;
				allowedToShoot = true;
				bulletsFired.FireBullets ();
			}

			if (allowFireMissile) {
				if (Input.GetButtonDown ("Fire2")) {
					nextFire = Time.time + missileRate;
					missileTime = 10.5f;
					allowedToShoot = false;
					bulletsFired.FireMissile ();
				}
				if (Input.GetButtonUp ("Fire2")) {
					nextFire = Time.time + missileRate;
					allowedToShoot = true;
				}
			} else {
				allowedToShoot = true;
			}
		}

		if (actualHealth <= 0) {
			allowedToShoot = false;
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			this.gameObject.SetActive (false);
			isDead = true;
		}

	}

	void FixedUpdate ()
	{
		// Move the player using standart inputs
		float verticalPosition = Input.GetAxis ("Vertical") * xSpeed * Time.deltaTime;
		float horizontalPosition = Input.GetAxis ("Horizontal") * ySpeed * Time.deltaTime;
		this.transform.Translate (horizontalPosition, verticalPosition, 0f);

		// Restrict the position of the player
		if (clampPosition) {
			float current_X = Mathf.Clamp (this.transform.position.x, xMinPosition, xMaxPosition);
			float current_Y = Mathf.Clamp (this.transform.position.y, yMinPosition, yMaxPosition);
			this.transform.position = new Vector3 (current_X, current_Y, this.transform.position.z);
		}
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.CompareTag ("BackBound")) {
			ReceiveDamage (scenario.damageGiven);
			my_Health.TakeDamage (15);
			StartCoroutine (SetShipColor ());
			allowedToShoot = false;
		
		} else if (collider.CompareTag ("EnemyBullets")) {
			ReceiveDamage (_EnemyBulletScript.enemyBulletDamage);
			my_Health.TakeDamage (_EnemyBulletScript.enemyBulletDamage);
			//StartCoroutine (SetShipColor ());
		} else {
			return;
		}
 	}

	void OnTriggerExit2D (Collider2D collider)
	{
		if (collider.CompareTag ("BackBound")) {
			allowedToShoot = true;
		}
	}

//	void OnCollisionEnter2D (Collision2D coll)
//	{
//		if (/*coll.gameObject.tag == "Scenery" ||*/ coll.gameObject.tag == "EnemyBullets") {
//			ReceiveDamage ((float)_EnemyBulletScript.enemyBulletDamage);
//			StartCoroutine (SetShipColor ());
//		}
//	}

	public void ReceiveDamage (float damage)
	{
		actualHealth -= damage;
	}

	private void GetDefaultSpriteAndColor ()
	{
		playerSprite = GetComponentInChildren<SpriteRenderer> ();
		playerDefaultColor = playerSprite.color;
	}

	private IEnumerator SetShipColor () //Sets the ship color to red during 0.1 seconds, useful when taking damage.
	{
		playerSprite.color = Color.red;
		yield return new WaitForSeconds (0.1f);
		playerSprite.color = playerDefaultColor;
	}

	public bool GetShootingPermission()
	{
		return allowedToShoot;
	}
	public void SetShootPermission(bool shootingPermission)
	{
		allowedToShoot = shootingPermission;
	}
	public bool GetMissilePermission()
	{
		return allowFireMissile;
	}
	public void SetMillsePermission(bool shootingPermission)
	{
		allowFireMissile = shootingPermission;
	}


}
