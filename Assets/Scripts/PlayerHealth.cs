using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Collections;


public class PlayerHealth : MonoBehaviour
{
	
	public float startingHealth = 500f;  // The amount of health the player starts with.
	public Slider health_Slider;  // The slider to represent how much health the player has.
	public Image health_FillImage;  // The image component of the slider.
	public Color fullHealthColor = Color.green;  // The color the health bar will be when on full health.
	public Color zeroHealthColor = Color.red;  // The color the health bar will be when on no health.
	// public GameObject m_ExplosionPrefab;  // A prefab that will be instantiated in Awake, then used whenever the tank dies.

	// private AudioSource m_ExplosionAudio;  // The audio source to play when the tank explodes.
	// private ParticleSystem m_ExplosionParticles;  // The particle system the will play when the tank is destroyed.
	private float currentHealth;  // How much health the player currently has.
	private bool isPlayerDead;  // Has the player's health been reduced beyond zero?


	private void Awake ()
	{
		// Instantiate the explosion prefab and get a reference to the particle system on it.
		// m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

		// Get a reference to the audio source on the instantiated prefab.
		// m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

		// Disable the prefab so it can be activated when it's required.
		// m_ExplosionParticles.gameObject.SetActive (false);
	}


	private void OnEnable ()
	{
		// When the player is enabled, reset its health and whether or not it's dead.
		currentHealth = startingHealth;
		isPlayerDead = false;

		// Update the health slider's value and color.
		SetHealthUI ();
	}


	public void TakeDamage (float amount)
	{
		// Reduce current health by the amount of damage done.
		currentHealth -= amount;

		// Change the UI elements appropriately.
		SetHealthUI ();

		// If the current health is at or below zero and it has not yet been registered, call OnDeath().
		if (currentHealth <= 0f && !isPlayerDead) {
			OnDeath ();
		}
	}


	private void SetHealthUI ()
	{
		// Set the slider's value appropriately.
		health_Slider.value = currentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		health_FillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, currentHealth / startingHealth);

	}


	private void OnDeath ()
	{
		// Set the flag so that this function is only called once.
		isPlayerDead = true;

		// Move the instantiated explosion prefab to the player's position and turn it on.
		// m_ExplosionParticles.transform.position = transform.position;
		// m_ExplosionParticles.gameObject.SetActive (true);

		// Play the particle system of the player exploding.
		// m_ExplosionParticles.Play ();

		// Play the player's explosion sound effect.
		// m_ExplosionAudio.Play ();

		// Turn the player off.
		// gameObject.SetActive (false);
	}

	public void SetHealth(float health){
		currentHealth = health;
	}
}
