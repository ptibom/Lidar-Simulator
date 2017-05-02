using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class SaveLoadTester : MonoBehaviour {

	/*public Dictionary<float, LinkedList<SphericalCoordinate>> randomData;
	public Dictionary<float, LinkedList<SphericalCoordinate>> loadedData;
	// Use this for initialization
	void Start () {
		randomData = new Dictionary<float, LinkedList<SphericalCoordinate>> ();
		for (int i = 0; i < 3; i++) {
			LinkedList<SphericalCoordinate> LL = new LinkedList<SphericalCoordinate> ();
			for (int j = 0; j < 180; j++) {
				LL.AddLast(new SphericalCoordinate(Mathf.Sin(((float)j*Mathf.PI)/180f), (float)j, (float)j, Vector3.zero, (int)0));
			}
			randomData.Add((float)i, LL);
		}

		SaveManager.SaveToCsv(randomData, "savedRandomData");
		loadedData = LoadManager.LoadCsv ("savedRandomData");

		foreach (KeyValuePair<float, LinkedList<SphericalCoordinate>>  kv in loadedData) {
			foreach(SphericalCoordinate sc in kv.Value) {
				Debug.Log (kv.Key.ToString () + " : " + sc.ToString ());
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}*/
}
