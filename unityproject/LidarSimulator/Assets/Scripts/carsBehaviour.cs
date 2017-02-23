using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carsBehaviour : MonoBehaviour {
	private float speed;

	// Use this for initialization
	void Start () {
		speed = Random.Range (5, 20);
		float step = speed * Time.deltaTime;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * speed;
	}
}
