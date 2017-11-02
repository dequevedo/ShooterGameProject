using System.Collections.Generic;
using UnityEngine;

public class FramesPerSecond : MonoBehaviour {

	public float updateInterval = 0.5f;

	private double accumulatedFrames = 0.0;
	private float frames = 0f;
	private float timeLeft;

	// Use this for initialization
	void Start () {
		if (!GetComponent<GUIText> ()) {
			print ("FramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}
		timeLeft = updateInterval;
	}

	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		accumulatedFrames += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeLeft <= 0.0) {
			GetComponent<GUIText> ().text = "" + (accumulatedFrames / frames).ToString ("F2");
			timeLeft = updateInterval;
			accumulatedFrames = 0.0;
			frames = 0;
		}
	}
}
