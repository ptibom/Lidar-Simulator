/*
* MIT License
* 
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius, 
* Tobias Alldén, Martin Chemander, Sherry Davar
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class SaveLoadTester : MonoBehaviour {

	public Dictionary<float, LinkedList<SphericalCoordinate>> randomData;
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

		//SaveManager.SaveToCsv(randomData, "savedRandomData");
		//loadedData = LoadManager.LoadCsv ("savedRandomData");

		foreach (KeyValuePair<float, LinkedList<SphericalCoordinate>>  kv in loadedData) {
			foreach(SphericalCoordinate sc in kv.Value) {
				Debug.Log (kv.Key.ToString () + " : " + sc.ToString ());
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
