using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PauseTimeScale : MonoBehaviour {
    public bool paused = false;
	public GameObject pauseMenu;
	void Update () {
        if(Input.GetButtonDown("Cancel")) {
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
                pauseMenu.SetActive(true);
				Camera.main.GetComponent<BlurOptimized> ().enabled = true;
            }
            else {
                Time.timeScale = 1;
                paused = false;
                pauseMenu.SetActive(false);
				Camera.main.GetComponent<BlurOptimized>().enabled = false;
            }
        }
	}
}
