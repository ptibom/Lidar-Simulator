using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
	public GameObject next;
	public GameObject previous;
	//med denna tredje referensen slipper jag listorna.... tror jag.
	public GameObject previousBranch;

	public int path;
	public int pathIndex;

	/*Ifall man behöver iterara genom alla waypoints som finns, typ om man har någon funktion som ska hitta närmsta waypoint eller något*/
	public static List<WayPoint> waypoints = new List<WayPoint>();

	/*Tanken är att en path är en lista av waypoints. Som i sin tur lagras i en lista av paths*/
	public static List<List<WayPoint>> paths = new List<List<WayPoint>> ();



	// Use this for initialization
	void awake() {
		waypoints.Add (this);
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/*Lägger in waypoiten på sista platsen i en path lista och fixar waypointens [Nope, obsolete, tror inte det blir bra med listor] */


	public void AddToPath(GameObject previousWayPoint) {
		WayPoint previousWayPointScript = previousWayPoint.GetComponent<WayPoint>();
		if (previousWayPointScript != null) {
			previousWayPointScript.next = this.gameObject;
			previous = previousWayPointScript.GetGameObject();
		}
	
	}

	/*tar bort waypointen från path listan och fixar till next och previous referenserna och förstör gameobjectet*/
	public void RemoveFromPath() {
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

	public static void removePath() {
	
	
	}

	public void ClosePath(GameObject otherWayPoint, GameObject previousWayPoint) {
	
	}

	public GameObject GetGameObject() {
		return this.gameObject;
	}
}
