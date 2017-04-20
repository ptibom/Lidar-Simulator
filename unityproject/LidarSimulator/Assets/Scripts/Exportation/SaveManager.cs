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
    //public SaveObject [] datalist;

    void Start()
    {
       
        Dictionary<float, List<SphericalCoordinates>> someData = new Dictionary<float, List<SphericalCoordinates>>();
        List<SphericalCoordinates> tempCoord = new List<SphericalCoordinates>();
        tempCoord.Add(new SphericalCoordinates(1f, 2f, 3f));
        tempCoord.Add(new SphericalCoordinates(2f, 3f, 2f));
        tempCoord.Add(new SphericalCoordinates(4f, 2f, 3f));
        someData.Add(0.1f, tempCoord);
        tempCoord.Add(new SphericalCoordinates(2f, 5f, 3f));
        tempCoord.Add(new SphericalCoordinates(3f, 4f, 2f));
        tempCoord.Add(new SphericalCoordinates(3f, 4f, 3f));
        someData.Add(0.2f, tempCoord);
        tempCoord.Add(new SphericalCoordinates(1f, 5f, 3f));
        tempCoord.Add(new SphericalCoordinates(2f, 5f, 2f));
        tempCoord.Add(new SphericalCoordinates(4f, 2f, 3f));
        someData.Add(0.3f, tempCoord);

        //SaveToCsv(someData, "/lidardata.csv");
    }
    //public void SaveToCsv(SaveObject[]  datalist, string filname){
    public static void SaveToCsv(Dictionary<float, LinkedList<SphericalCoordinates>> data, String filename)
    {    
        try
        {
          
            //string filnamn = Application.persistentDataPath + filename;
       
            ///datatable for rows to be added
            List<string[]> dataTable = new List<string[]>();
            /// object list
            /// 
            /// header in csv file
            string[] header = new string[5];

            header[0] = "Time";
            header[1] = "ID";
            header[2] = "Radius";
            header[3] = "Inclination";
            header[4] = "Azimuth";
            dataTable.Add(header);


            List<SaveObject> objlist = new List<SaveObject>();
            foreach (KeyValuePair<float, LinkedList<SphericalCoordinates>> coordinatePair in data)
            {
                foreach (SphericalCoordinates coordinate in coordinatePair.Value)
                {
                    string[] rows = new string[5];
                    rows[0] = coordinatePair.Key.ToString(); // The time
                    rows[1] = 999.ToString(); // The id
                    rows[2] = coordinate.GetRadius().ToString();
                    rows[3] = coordinate.GetInclination().ToString();
                    rows[4] = coordinate.GetAzimuth().ToString();
                    dataTable.Add(rows);
                }
            }

            ///put each row in data table to an array
            string[][] output = new string[dataTable.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = dataTable[i];
            }

            ///add delimiter between strings in each row in the data table
            StringBuilder sb = new StringBuilder();

            int length = output.GetLength(0);
            string delimiter = ";";
            for (int r = 0; r < length; r++)
            {

                sb.AppendLine(string.Join(delimiter, output[r]));
            }
            ///write lines to output file
            StreamWriter outputstream = System.IO.File.CreateText(filename);
            /// write separator as the first  line in the file för CSV file to be oppened correctly
            outputstream.WriteLine("sep=;");

            outputstream.WriteLine(sb);
            outputstream.Close();
    
        }
        catch (IOException e)
        {
            Debug.Log("Access violation, printing file.");
        }
    }
}
