using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class SaveManager : MonoBehaviour
{
	public SaveObject [] datalist;

	void Start (){
		///filename to save lidar data
		string filename= "/lidardata.csv" ;
		/// list of class SaveObject
		datalist = new SaveObject [4];
		for (int i = 0; i < datalist.Length; i++) {
			datalist [i] = new SaveObject ();
		}
		///initialization
		datalist[0] = new SaveObject () { time  = 1, ID=1, radius=11,inclination=17,azimuth=81};
		datalist[1] = new SaveObject () { time  = 2 , ID=2,radius = 22,inclination=27,azimuth=82 };
		datalist[2]= new SaveObject () { time = 3, ID=3, radius  = 33, inclination=37,azimuth=83};
		datalist[3] = new SaveObject () {time = 4 , ID=4, radius  = 44,inclination=44,azimuth=84 };
		///Save in csv format
		SaveToCsv (datalist, filename);

	}


	public void SaveToCsv(SaveObject[]  datalist, string filname){


		///filename to save lidar data
		string filename = "/lidardata.csv";
		string filnamn = Application.persistentDataPath + filename;
		///datatable for rows to be added
		List<string[]> dataTable = new List<string[]> ();
		/// header in csv file
		string[] header = new string[5]; 

		header [0] = "Time";
		header [1] = "ID";
		header [2] = "Radius";
		header [3] = "Inclination";
		header [4] = "Azimuth";
		dataTable.Add (header);
		///add rows for each object in the object list to the data table
		for (int k = 0; k < 4; k++) {

			string[] rows = new string [5];
			rows [0] = datalist [k].time.ToString ();
			rows [1] = datalist [k].ID.ToString ();
			rows [2] = datalist [k].radius.ToString ();
			rows [3] = datalist [k].inclination.ToString ();
			rows [4] = datalist [k].azimuth.ToString ();
			dataTable.Add (rows);

		}

		///put each row in data table to an array
		string[][] output = new string[dataTable.Count][];
		for (int i = 0; i < output.Length; i++) {
			output [i] = dataTable [i];
		}

		///add delimiter between strings in each row in the data table
		StringBuilder sb = new StringBuilder ();

		int length = output.GetLength (0);
		string delimiter = ";";
		for (int r = 0; r < length; r++) {

			sb.AppendLine (string.Join (delimiter, output[r]));
		}
		///write lines to output file
		StreamWriter outputstream = System.IO.File.CreateText (filnamn);
		/// write separator as the first  line in the file för CSV file to be oppened correctly
		outputstream.WriteLine ("sep=;");

		outputstream.WriteLine (sb);
		outputstream.Close();
		Debug.Log("datapath"+Application.persistentDataPath );

	}
}
