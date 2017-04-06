using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
	public GameObject next;
	public GameObject previous;
	//med denna tredje referensen slipper jag listorna.... tror jag.
	public GameObject previousBranch;
	LineRenderer nextLine;
	public bool isStartNode = false;

	public int path;
	public int pathIndex;

	/*Ifall man behöver iterara genom alla waypoints som finns, typ om man har någon funktion som ska hitta närmsta waypoint eller något*/
	public static List<WayPoint> waypoints = new List<WayPoint>();

	/*Tanken är att en path är en lista av waypoints. Som i sin tur lagras i en lista av paths*/
	//public static List<List<WayPoint>> paths = new List<List<WayPoint>> ();


	//stäng av eller sätt på waypointens triggercollider.
	public void SetColliderState(bool b){
		SphereCollider s = GetComponent<SphereCollider> ();
		s.enabled = b;
	}

	public void SetPathColliderState(bool b, GameObject wayPointInPath){
		Debug.Log ("RemovePath Called");
		//int i = 0;
		//int j = 0;
		//skapar en lista med alla waypoints i pathen och använder den för att förstöra hela pathen
		WayPoint currentWayPoint = wayPointInPath.GetComponent<WayPoint>();
		GameObject currentWayPointGameObject = wayPointInPath;
		while (true) {
			//Första loopen är problemet!!!!!!!!!!
			/*i = i + 1;
			if (i >= 10) {
				Debug.Log ("Looping too much, breaking");
			}*/
			if (currentWayPoint.isStartNode == true/*previous == null*/) {
				Debug.Log ("Breaking1");
				break;
			}
			if (currentWayPoint.previous != null) {

				currentWayPointGameObject = currentWayPoint.previous;
				currentWayPoint = currentWayPointGameObject.GetComponent<WayPoint> ();
			}
			Debug.Log ("Loop1");
			//break;
		}
		List<GameObject> WayPointsToDestroy = new List<GameObject> ();
		while (true) {
			if(WayPointsToDestroy.Contains(currentWayPointGameObject) == true){
				//Debug.Log ("Breaking2");
				break;
			}
			WayPointsToDestroy.Add (currentWayPointGameObject);
			currentWayPoint = currentWayPoint.GetComponent<WayPoint>().next.GetComponent<WayPoint>();
			currentWayPointGameObject = currentWayPoint.gameObject;
			//Debug.Log ("Loop2");

		}
		foreach (GameObject g in WayPointsToDestroy) {
			g.GetComponent<WayPoint> ().SetColliderState (b);
		}
	}

	public void SetPathVisibility(bool b, GameObject wayPointInPath){
		Debug.Log ("RemovePath Called");
		//int i = 0;
		//int j = 0;
		//skapar en lista med alla waypoints i pathen och använder den för att förstöra hela pathen
		WayPoint currentWayPoint = wayPointInPath.GetComponent<WayPoint>();
		GameObject currentWayPointGameObject = wayPointInPath;
		while (true) {
			//Första loopen är problemet!!!!!!!!!!
			/*i = i + 1;
			if (i >= 10) {
				Debug.Log ("Looping too much, breaking");
			}*/
			if (currentWayPoint.isStartNode == true/*previous == null*/) {
				Debug.Log ("Breaking1");
				break;
			}
			if (currentWayPoint.previous != null) {

				currentWayPointGameObject = currentWayPoint.previous;
				currentWayPoint = currentWayPointGameObject.GetComponent<WayPoint> ();
			}
			Debug.Log ("Loop1");
			//break;
		}
		List<GameObject> WayPointsToDestroy = new List<GameObject> ();
		while (true) {
			if(WayPointsToDestroy.Contains(currentWayPointGameObject) == true){
				//Debug.Log ("Breaking2");
				break;
			}
			WayPointsToDestroy.Add (currentWayPointGameObject);
			currentWayPoint = currentWayPoint.GetComponent<WayPoint>().next.GetComponent<WayPoint>();
			currentWayPointGameObject = currentWayPoint.gameObject;
			//Debug.Log ("Loop2");

		}
		foreach (GameObject g in WayPointsToDestroy) {
			//Debug.Log ("Destroying");
			g.GetComponent<WayPoint>().SetVisibility(b);
		}
	}

	public static void SetGlobalVisibility (bool b){
		foreach (WayPoint w in waypoints) {
			w.SetVisibility (b);
		}
	}


	public static void SetGlobalColliderState (bool b){
		//Debug.Log ("SetGlobalVisibility() called");
		foreach (WayPoint w in waypoints) {
			w.SetColliderState(b);
			//Debug.Log ("Iterated through a WayPoint");
		}
	}

	//sätt på eller stäng av waypointens renderer
	public void SetVisibility(bool b){
		if (b == true) {
			nextLine.enabled = true;
			Renderer r = GetComponent<Renderer> ();
			r.enabled = true;
		} 
		else {
			nextLine.enabled = false;
			Renderer r = GetComponent<Renderer> ();
			r.enabled = false;
		}
	
	}

	// Use this for initialization
	void awake() {


	}

	void Start () {
		waypoints.Add (this);
		nextLine = this.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (next != null) {
			Vector3[] g = new Vector3[2];
			g [0] = transform.position;
			g [1] = next.transform.position;
			nextLine.SetPositions(g);
		}

	}

	//väldigt viktigt för att få looparna i remove path att funka. Det måste finnas en startnod.
	public void SetStart(bool s) {
		isStartNode = s;
	}

	public bool IsStart(){
		return isStartNode;
	}


	/*Lägger in waypoiten på sista platsen i en path lista och fixar waypointens [Nope, obsolete, tror inte det blir bra med listor] */


	public void AddToPath(GameObject previousWayPoint) {
		WayPoint previousWayPointScript = previousWayPoint.GetComponent<WayPoint>();
		if (previousWayPointScript != null) {
			previousWayPointScript.next = this.gameObject;
			previous = previousWayPointScript.GetGameObject();
		}
	
	}

	///*tar bort waypointen från path listan och fixar till next och previous referenserna och förstör gameobjectet*/
	/*Om du har någon funktion som ska ta bort en waypoint så är det bäst att kalla på den här funktionen för den fixar så att vägen i övrigt är intakt*/
	public void RemoveFromPath() {
		/* för att det här ska funka får det aldrig finnas ett scenario när en next eller previous referens pekar på ett GameObject
		 * utan WayPoint skriptet för då blir det pannkaka av allt*/


		//WayPoint previousWayPointScript = previousWayPoint.GetComponent<WayPoint>();
		//WayPoint nextWayPointScript = previousWayPoint.GetComponent<WayPoint>();
		/*if(next != null && previous != null && next.GetComponent<WayPoint>().previousBranch != null){
			
		}*/
		if (next != null && previous != null && previousBranch != null) {

			next.GetComponent<WayPoint> ().previous = previous;
			previous.GetComponent<WayPoint> ().next = next;
			previousBranch.GetComponent<WayPoint> ().next = previous;
			previous.GetComponent<WayPoint> ().previousBranch = previousBranch;

		}
		else if(next == null && previous != null && previousBranch != null){
			previousBranch.GetComponent<WayPoint> ().next = previous;
			previous.GetComponent<WayPoint> ().previousBranch = previousBranch;
			previous.GetComponent<WayPoint> ().next = null;


		}
		else if (next != null && previous != null) {
			next.GetComponent<WayPoint> ().previous = previous;
			previous.GetComponent<WayPoint> ().next = next;
		} 
		else if (next != null && previous == null) {
			next.GetComponent<WayPoint> ().previous = null;
		} 
		else if (next == null && previous != null) {
			
		} 
		else {
		}

		Destroy (gameObject);
		/*int i = 0;
		while (true) {
			
		
		}
		Destroy (gameObject);*/

		//Skippa listorna och bara använda next och previus referenserna???? Kan funka!!!!! Var försiktig med specialfallet med en sluten väg med svans dock!
	}

	/*skapar en ny path lista i listan över paths*/
	/*public static void StartPath() {
		//paths.Add (new List<WayPoint> ());
	
	}*/



	/*Det är super-hyper-mega-ultra-viktigt att det finns en nod i vägen som är satt till startnod annars kommer du frysa programmet i en oändlig while-loop om du 
	kallar på den här*/
	public static void removePath(GameObject wayPointInPath) {
		Debug.Log ("RemovePath Called");
		//int i = 0;
		//int j = 0;
		//skapar en lista med alla waypoints i pathen och använder den för att förstöra hela pathen
		WayPoint currentWayPoint = wayPointInPath.GetComponent<WayPoint>();
		GameObject currentWayPointGameObject = wayPointInPath;
		while (true) {
			//Första loopen är problemet!!!!!!!!!!
			/*i = i + 1;
			if (i >= 10) {
				Debug.Log ("Looping too much, breaking");
			}*/
			if (currentWayPoint.isStartNode == true/*previous == null*/) {
				Debug.Log ("Breaking1");
				break;
			}
			if (currentWayPoint.previous != null) {
			
				currentWayPointGameObject = currentWayPoint.previous;
				currentWayPoint = currentWayPointGameObject.GetComponent<WayPoint> ();
			}
			Debug.Log ("Loop1");
			//break;
		}
		List<GameObject> WayPointsToDestroy = new List<GameObject> ();
		while (true) {
			/*j = j + 1;
			if (j >= 10) {
				Debug.Log ("Looping too much, breaking");
			}*/
			if(WayPointsToDestroy.Contains(currentWayPointGameObject) == true){
				Debug.Log ("Breaking2");
				break;
			}
			WayPointsToDestroy.Add (currentWayPointGameObject);
			currentWayPoint = currentWayPoint.GetComponent<WayPoint>().next.GetComponent<WayPoint>();
			currentWayPointGameObject = currentWayPoint.gameObject;
			Debug.Log ("Loop2");
			
		}
		foreach (GameObject g in WayPointsToDestroy) {
			Debug.Log ("Destroying");
			waypoints.Remove (g.GetComponent<WayPoint> ());
			Destroy (g);
		}
	}
	
	/**/
	public void ClosePath(GameObject otherWayPoint, GameObject previousWayPoint) {
		previousWayPoint.GetComponent<WayPoint>().next = otherWayPoint;
		otherWayPoint.GetComponent<WayPoint> ().previousBranch = previousWayPoint;
	}

	public GameObject GetGameObject() {
		return this.gameObject;
	}

	public static void TestMethod (GameObject g){
		WayPoint currentWayPoint = g.GetComponent<WayPoint>();
		GameObject currentWayPointGameObject = g;
		Debug.Log ("Method call worked");

	}
}
