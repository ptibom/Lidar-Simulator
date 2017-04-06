using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject followThis;

	public int scrollPixelMargin = 10;

	public float behindDistance = 10f;
	public float aboveheight = 5f;
	public float roamingSpeed = 70f;
	public float roamingHeight = 30f;
	public float smoothingSpeed = 0.045f; // Ska vara runt 0.045 kör 2

	float distancetoObject;
	Vector3 targetPosition;
	Vector3 targetDirection;
	enum CameraState{FOLLOW, ROAM}
	CameraState state = CameraState.FOLLOW;
	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		SetRoam ();
	}
	
	// Update is called once per frame
	void Update () {
		/*så att jag enkelt kan avsluta när jag testar kameran med ett build*/
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
		}
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
		//Cursor.visible = false;
	
	}

	public void SetRoam() {
		state = CameraState.ROAM;
		transform.rotation = Quaternion.Euler (90, 0, 0);
		transform.position = new Vector3 (transform.position.x, roamingHeight, transform.position.z);
		Cursor.visible = true;
	}

	void Roam(){
		Vector3 moveDirection = new Vector3 (0, 0, 0);
		//Scrolla om muspekaren är i kanten av skärmen eller om piltangenterna tryckks ner.
		//skapa en summavektor som bestämmer vart kameran ska gå och normalisera den
		if (/*Input.GetKey (KeyCode.UpArrow) ||*/ Input.mousePosition.y >= Screen.height - scrollPixelMargin) {
			moveDirection = moveDirection + new Vector3 (0, 1, 0);
		}
		if(/*Input.GetKey (KeyCode.LeftArrow) ||*/ Input.mousePosition.x < scrollPixelMargin) {
			moveDirection = moveDirection + new Vector3 (-1, 0, 0);
		}
		if(/*Input.GetKey (KeyCode.DownArrow) ||*/ Input.mousePosition.y < scrollPixelMargin) {
			moveDirection = moveDirection + new Vector3 (0, -1, 0);
		}
		if(/*Input.GetKey (KeyCode.RightArrow) ||*/ Input.mousePosition.x >= Screen.width - scrollPixelMargin) {
			moveDirection = moveDirection + new Vector3 (1, 0, 0);
		}
		if (Input.GetKey (KeyCode.RightControl)) {
			moveDirection = moveDirection + new Vector3 (0, 0, 1);
		}
		if (Input.GetKey (KeyCode.RightShift)) {
			moveDirection = moveDirection + new Vector3 (0, 0, -1);
		}
		moveDirection = moveDirection.normalized;
		//använd den normaliserade summavektorn för att translata kameran, deltaTime gör att det blir samma fart oavsett framerate, förhoppningsvis
		transform.Translate((moveDirection * roamingSpeed*Time.deltaTime));
	

	}


	void Follow() {

		targetPosition = new Vector3(0,0,0);

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