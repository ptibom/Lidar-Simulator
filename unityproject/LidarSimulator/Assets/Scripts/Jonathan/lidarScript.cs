using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class lidarScript : MonoBehaviour {


	public GameObject sensorPrefab;
	public GameObject hitPointPrefab;
	public int resolution = 16;
	public int rotationAngle = 1;
	public bool doubleRows;



	private HashSet<GameObject> sensors;
	private particleUpdateScript psScript;
	private bool lapCollected = false;
	private bool pointsUpdated = true;
	private HashSet<RaycastHit> fullLapPointSet;

	//Variables used for counting datapoints over a period
	private float stopTime;
	private bool done = false;
	private int total = 0;

	void Start () 
	{
		fullLapPointSet = new HashSet<RaycastHit> ();
		sensors = new HashSet<GameObject> ();
//		psScript = GameObject.FindGameObjectWithTag("pSystem").GetComponent<particleUpdateScript>();

		createSensors ();

		stopTime = Time.time + 5;

	}

	void Update () 
	{
		if(!lapCollected && pointsUpdated)
		{
			pointsUpdated = false;
			StartCoroutine (collectData ());	
		}
		if (lapCollected && !pointsUpdated) 
		{
			lapCollected = false;
			StartCoroutine (updatePoints ());
		}


		//countPoints ();
	}

	void createSensors(){
		
		int created = 0;
		int create = resolution * 4;
		if(!doubleRows){
			create = resolution * 2;
		}
		float tilt = 10.5f/resolution;
		float tiltBase = -2;
		float tiltCount = 0;
		Vector3 pos = new Vector3 (2.54f*0.01f , 9.03f * 2.54f * 0.01f, 0);

		while (created <= create)
		{
			GameObject sensor = Instantiate (sensorPrefab, transform.position, transform.rotation);
			sensor.transform.Translate (pos);
			sensor.transform.Rotate (new Vector3 (tiltBase + tilt * tiltCount, 0, 0));
			sensor.transform.SetParent (transform);
			sensor.name = "Sensor: " + created.ToString();
			sensors.Add (sensor);

			created++;
			tiltCount++;
			if(created == resolution){
				tiltCount = 0;
				tiltBase = 8.9f;
				tilt = 16/resolution;
				pos = new Vector3 (2.54f * 0.01f, 6.18f * 2.54f * 0.01f, 0);
			} else if(created == resolution*2){
				tiltCount = 0;
				tiltBase = -2;
				tilt = 10.5f/resolution;
				pos = new Vector3 (-2.54f * 0.01f, 9.03f * 2.54f * 0.01f, 0);
			} else if(created == resolution*3){
				tiltCount = 0;
				tiltBase = 8.9f;
				tilt = 16/resolution;
				pos = new Vector3 (-2.54f * 0.01f, 6.18f * 2.54f * 0.01f, 0);
			}
		}



	}

	/*void countPoints()
	{
		if (Time.time >= stopTime && !done) 
		{
			int i = 0;

			while (i < noSensors) {

				sensorScript sensor = GameObject.Find ("Sensor: " + i.ToString ()).GetComponent<sensorScript> ();
				total = total + sensor.getListSize ();
				i++;
			}
			done = true;
		}
	}*/

	void OnGUI()
	{
		if (Time.time >= stopTime && done) 
		{
			GUI.Box (new Rect (5, 5, 500, 500), new GUIContent (total.ToString()));
		}
	}


	private IEnumerator collectData()
	{
		for (int i = 0; i < 360; i++) 
		{
			foreach (GameObject s in sensors) 
			{
				s.GetComponent<sensorScript> ().listMethod ();				
			}
			transform.Rotate (new Vector3 (0, rotationAngle, 0));
		}

		lapCollected = true;

		yield return null;


	}

	private IEnumerator updatePoints()
	{
		fullLapPointSet.Clear ();
		foreach (GameObject s in sensors) 
		{
			fullLapPointSet.UnionWith(s.GetComponent<sensorScript> ().clearPointSet ());
		}

		//psScript.updateParticlePos (fullLapPointSet);

		//Bör inte användas såhär. Återanvänd punkterna för att optimera och skapa fler vid behov eller inaktivera om de inte används


		gameObjectVisualization ();

		yield return new WaitForSecondsRealtime(1);

		pointsUpdated = true;
	}

	void gameObjectVisualization(){
		foreach(Transform child in GameObject.FindGameObjectWithTag ("Points").transform)
		{
			Destroy(child.gameObject);
		}

		foreach(RaycastHit hit in fullLapPointSet)
		{
			Instantiate (hitPointPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.SetParent (GameObject.FindGameObjectWithTag ("Points").transform);;
		}
	}
		
}

// ------------------------------------  Editor Settings -----------------------------------------

[CustomEditor (typeof(lidarScript))]
public class LidarEditor : Editor{


	bool advancedSettingsFoldout = false;
	bool helpFoldout = false;

	public override void OnInspectorGUI(){
		lidarScript lS = (lidarScript)target;

		EditorGUILayout.Space ();
		helpFoldout = EditorGUILayout.Foldout (helpFoldout, "Settings help: ");
		if (helpFoldout) {
			EditorGUILayout.HelpBox (
				"The lidar sensor originaly consists of 4 sets of lasers  " +
				"with 16 lasers per set. The sets are setup in a square layout " +
				"which makes it possible to disable one row for faster " +
				"generation but lower data point density. It is also possible " +
				"to lower the amount of lasers per set for higher efficiency " +
				"aswell as adjusting the roatation angle between the data points.", MessageType.Info, true);
		}

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("LIDAR settings:");
		EditorGUILayout.Space ();
		lS.doubleRows = EditorGUILayout.Toggle ("Two rows of sets: ", lS.doubleRows);
		EditorGUILayout.Space ();
		lS.resolution = EditorGUILayout.DelayedIntField ("Lasers per set: ", lS.resolution);
		EditorGUILayout.Space ();
		lS.rotationAngle = EditorGUILayout.IntField ("Angular rotation: ", lS.rotationAngle);
		EditorGUILayout.Space ();


		lS.hitPointPrefab = (GameObject)EditorGUILayout.ObjectField ("Visual point prefab: ", lS.hitPointPrefab, typeof(GameObject));
		lS.sensorPrefab = (GameObject)EditorGUILayout.ObjectField ("Laser prefab: ", lS.sensorPrefab, typeof(GameObject));
		EditorGUILayout.Space ();


		advancedSettingsFoldout = EditorGUILayout.Foldout (advancedSettingsFoldout, "Advanced settings: ");
		if (advancedSettingsFoldout) {
					EditorGUILayout.LabelField ("Individual set settings... TO BE IMPLEMENTED");
		}
		EditorGUILayout.Space ();
	}

}