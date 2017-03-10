using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
	public GameObject next;
	public GameObject previous;

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


	/*Lägger in waypoiten på sista platsen i en path lista och fixar waypointens */
	public void AddToPath(int pathNumber) {
		
	
	}

	/*tar bort waypointen från path listan och fixar till next och previous referenserna*/
	public void RemoveFromPath() {
	
	
	}

	/*skapar en ny path lista i listan över paths*/
	public static void StartPath() {
		paths.Add (new List<WayPoint> ());
	
	}

	public static void removePath() {
	
	
	}

	public void ClosePath(WayPoint otherWayPoint) {
	
	}
}
