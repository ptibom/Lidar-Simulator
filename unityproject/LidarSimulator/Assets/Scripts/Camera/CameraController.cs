using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject followThis;
	public float behindDistance;
	public float aboveheight;
	public float roamingSpeed = 10;
	public float roamingHeight = 30;
	public float smoothingSpeed = 0.045f; // Ska vara runt 0.045 kör 2

	float distancetoObject;
	Vector3 targetPosition;
	Vector3 targetDirection;
	enum CameraState{FOLLOW, ROAM}
	CameraState state = CameraState.FOLLOW;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void LateUpdate () {
		//Om state är follow så kör follow logiken för att följa objektet. Om state är roam så kör roam logiken för att flyga runt fritt
		switch (state) {
			case CameraState.FOLLOW:
				Follow ();
				break;
			case CameraState.ROAM:
				Roam();
				break;
		}

		//Följande kod är bara för att testa övergången mellan following och roaming. Den kan tas bort sen så kan något externt kontrollerskript kalla på SetRoam() eller SetFollow()
		if(Input.GetKeyDown(KeyCode.Space)){
			if (state == CameraState.FOLLOW) {
				SetRoam ();
			} 
			else {
				SetFollow ();
			}
		}
	}

	public void SetFollow() {
		//ändra tillståndet så att kameran nu följer objektet istället.
		state = CameraState.FOLLOW;
	
	}

	public void SetRoam() {
		state = CameraState.ROAM;
		transform.rotation = Quaternion.Euler (90, 0, 0);
		transform.position = new Vector3 (transform.position.x, roamingHeight, transform.position.z);
	}

	void Roam(){
		Vector3 moveDirection = new Vector3 (0, 0, 0);

		if (Input.GetKey (KeyCode.UpArrow)) {
			moveDirection = moveDirection + new Vector3 (0, 1, 0);
		}
		if(Input.GetKey (KeyCode.LeftArrow)) {
			moveDirection = moveDirection + new Vector3 (-1, 0, 0);
		}
		if(Input.GetKey (KeyCode.DownArrow)) {
			moveDirection = moveDirection + new Vector3 (0, -1, 0);
		}
		if(Input.GetKey (KeyCode.RightArrow)) {
			moveDirection = moveDirection + new Vector3 (1, 0, 0);
		}
		if (Input.GetKey (KeyCode.RightControl)) {
			moveDirection = moveDirection + new Vector3 (0, 0, 1);
		}
		if (Input.GetKey (KeyCode.RightShift)) {
			moveDirection = moveDirection + new Vector3 (0, 0, -1);
		}
		moveDirection = moveDirection.normalized;
		transform.Translate((moveDirection * roamingSpeed*Time.deltaTime));
	

	}


	void Follow() {

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


		//Raycast för att kolla om något är mellan kameran och det den följer
		RaycastHit hit;
		bool hitEnvironment;
		hitEnvironment = false;
		Ray cameraRay = new Ray(followThis.transform.position, targetDirection);
		Debug.DrawRay (followThis.transform.position, targetDirection); // gör saker lite tydligare i sceneview om man pausar och kollar där
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