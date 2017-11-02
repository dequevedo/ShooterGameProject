using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCustomization : MonoBehaviour
{
	public GameObject shipContainer;

	public GameObject[] cockpits;
	private GameObject actualCockpitObj;
	private int actualCockpitIndex = 0;

	public GameObject[] engines;
	private GameObject actualEngineObj;
	private int actualEngineIndex = 0;

	public GameObject[] weapons;
	private GameObject actualWeaponObj;
	private int actualWeaponIndex = 0;

	public GameObject[] wings;
	private GameObject actualWingObj;
	private int actualWingIndex = 0;

	void Awake ()
	{
		shipContainer = new GameObject ();
		shipContainer.name = "SpaceShip";
		shipContainer.tag = "SpaceShip";
		DontDestroyOnLoad (shipContainer);
		updateShipCustomization ();
	}

	public void switchWeapon (bool increasing)
	{
		if (increasing) {
			if (actualWeaponIndex == weapons.Length - 1) {
				actualWeaponIndex = 0;
			} else {
				actualWeaponIndex++;
			}
		} else {
			if (actualWeaponIndex == 0) {
				actualWeaponIndex = weapons.Length - 1;
			} else {
				actualWeaponIndex--;
			}
		}
		updateShipCustomization ();
	}

	public void switchEngine (bool increasing)
	{
		if (increasing) {
			if (actualEngineIndex == engines.Length - 1) {
				actualEngineIndex = 0;
			} else {
				actualEngineIndex++;
			}
		} else {
			if (actualEngineIndex == 0) {
				actualEngineIndex = engines.Length - 1;
			} else {
				actualEngineIndex--;
			}
		}
		updateShipCustomization ();
	}

	public void switchCockpit (bool increasing)
	{
		if (increasing) {
			if (actualCockpitIndex == cockpits.Length - 1) {
				actualCockpitIndex = 0;
			} else {
				actualCockpitIndex++;
			}
		} else {
			if (actualCockpitIndex == 0) {
				actualCockpitIndex = cockpits.Length - 1;
			} else {
				actualCockpitIndex--;
			}
		}
		updateShipCustomization ();
	}

	public void switchWing (bool increasing)
	{
		if (increasing) {
			if (actualWingIndex == wings.Length - 1) {
				actualWingIndex = 0;
			} else {
				actualWingIndex++;
			}
		} else {
			if (actualWingIndex == 0) {
				actualWingIndex = wings.Length - 1;
			} else {
				actualWingIndex--;
			}
		}
		updateShipCustomization ();
	}

	public void updateShipCustomization ()
	{

		Destroy (actualCockpitObj);
		actualCockpitObj = Instantiate (cockpits [actualCockpitIndex], transform.position, Quaternion.identity);
		
		Destroy (actualEngineObj);
		actualEngineObj = Instantiate (engines [actualEngineIndex], actualCockpitObj.transform.Find ("EnginePosition").gameObject.transform.position, Quaternion.identity);
		
		Destroy (actualWeaponObj);
		actualWeaponObj = Instantiate (weapons [actualWeaponIndex], actualCockpitObj.transform.Find ("WeaponPosition").gameObject.transform.position, Quaternion.identity);
		
		Destroy (actualWingObj);
		actualWingObj = new GameObject ();
		actualWingObj.name = "Wings";
		GameObject wing1 = Instantiate (wings [actualWingIndex], actualCockpitObj.transform.Find ("WingPosition").gameObject.transform.position, Quaternion.identity);
		Vector3 wing2Position = new Vector3 (-actualCockpitObj.transform.Find ("WingPosition").gameObject.transform.position.x, actualCockpitObj.transform.Find ("WingPosition").gameObject.transform.position.y, actualCockpitObj.transform.Find ("WingPosition").gameObject.transform.position.z);
		GameObject wing2 = Instantiate (wings [actualWingIndex], wing2Position, Quaternion.identity);
		wing1.GetComponent<SpriteRenderer> ().flipX = false;
		wing2.GetComponent<SpriteRenderer> ().flipX = true;
		wing1.GetComponent<SpriteRenderer> ().sortingOrder = -1;
		wing2.GetComponent<SpriteRenderer> ().sortingOrder = -1;
		wing1.transform.parent = actualWingObj.transform;
		wing2.transform.parent = actualWingObj.transform;

		actualCockpitObj.transform.parent = shipContainer.transform;
		actualEngineObj.transform.parent = shipContainer.transform;
		actualWeaponObj.transform.parent = shipContainer.transform;
		actualWingObj.transform.parent = shipContainer.transform;
	}

	public void LoadNewScene ()
	{
		SceneManager.LoadScene ("Game_D");
		Debug.Log (GameObject.FindGameObjectWithTag ("Player"));
	}
}
