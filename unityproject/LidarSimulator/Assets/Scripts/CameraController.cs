using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject followThis;
	public float behindDistance;
	public float aboveheight;
	public float smoothingSpeed = 0.045f; // Ska vara runt 0.045 kör 2

	float distancetoObject;
	Vector3 targetPosition;
	Vector3 targetDirection;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void LateUpdate () {
		
		targetPosition = new Vector3(0,0,0);

		Vector3 DeltaPos;

		Vector3 smoothVelocity;
		if (followThis.GetComponent<Rigidbody> () != null) {
			smoothVelocity = followThis.GetComponent<Rigidbody> ().velocity;
		}
		else{
		smoothVelocity = new Vector3 (0, 0, 0);
		}

		//funktion som placerar kamerans första spökplats (followerPosition) där den ska vara, kanske blir lite buggigt så kan ändras om det behövs
		//Om du vill att kameran ska följa objektet på ett annat sätt så är det denna raden, som bestämmer followerposition som du ska ändra. resten kan du lämna som det är
		//som det är nu beror kamerans riktning på "bilens" fram riktning.
		Vector3 followerPosition = followThis.transform.position - followThis.transform.forward*behindDistance + new Vector3(0, aboveheight,0);

		//fixar riktningen för raycasten
		targetDirection = followerPosition - followThis.transform.position;

		RaycastHit hit;
		bool hitEnvironment;
		hitEnvironment = false;
		Ray cameraRay = new Ray(followThis.transform.position, targetDirection);
		Debug.DrawRay (followThis.transform.position, targetDirection);
		if (Physics.Raycast (cameraRay, out hit, Vector3.Distance(followThis.transform.position,followerPosition))) {
			if (hit.collider.tag == "Environment") {
				targetPosition = hit.point + hit.normal.normalized*0.5f;/* Flytta kameran till en bit innan det som är mellan kameran och bilen enl normalen på ytan*/;
				hitEnvironment = true;
				//Debug.Log ("Hit1");
			}
		}

		if (hitEnvironment == false) {
			targetPosition = followerPosition;  // ingenting var ivägen och andra spökpositionen tar samma värde som första
		}
		if(Vector3.Magnitude (targetPosition - transform.position) > smoothingSpeed*Time.deltaTime*10){
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition,ref smoothVelocity, smoothingSpeed*Time.deltaTime);
			//Om kameran hoppar för långt på en frame, använd SmoothDamp för att göra övergången snyggare
		}
		else{
			transform.position = targetPosition;
		}

		transform.forward = -targetDirection; //titta på bilen





	}
}