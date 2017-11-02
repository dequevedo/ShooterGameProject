using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenoidalMovement : MonoBehaviour {	 
	public float horizontalSpeed;
	public float verticalSineAmplitude;
	private Vector3 _startPosition;
	private float timeCounter = 0;

	void Start () 
	{
		_startPosition = transform.position;
	}

	void Update()
	{
		timeCounter += Time.deltaTime;
		transform.position = _startPosition + new Vector3(-horizontalSpeed * timeCounter, verticalSineAmplitude * Mathf.Sin(timeCounter), 0f);
	}
}
