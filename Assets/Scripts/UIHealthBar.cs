using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour {

	
	public PlayerController playercontroller;
	
	private Text hpText;
	private Vector2 dimensions;
	private RectTransform rectTransform;
	private float baseWidth;
	private float baseHeight;

	void Awake(){
		rectTransform = GetComponent<RectTransform>();
		dimensions = rectTransform.sizeDelta;
		hpText = GetComponentInChildren<Text>();
	}

	void Update () {
		if(playercontroller.CurrentHealth <= 0){
			hpText.text = "Dead";
			rectTransform.sizeDelta = new Vector2 (0, dimensions[1]);
		} else {
			hpText.text = playercontroller.CurrentHealth.ToString() + "%";
			rectTransform.sizeDelta = new Vector2 ((playercontroller.CurrentHealth * 250)/playercontroller.MaximumHealth, dimensions[1]);
		}
	}
}
