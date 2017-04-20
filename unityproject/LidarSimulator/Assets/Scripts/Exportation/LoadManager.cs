using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class LoadManager : MonoBehaviour
{


    void Start()
    {


    }
    public static  Dictionary<float, LinkedList<SphericalCoordinate>> LoadCsv(String filename)
    {
        StreamReader sr = new StreamReader(File.OpenRead(filename));
     
        Dictionary<float, LinkedList<SphericalCoordinate>> data = new Dictionary<float, LinkedList<SphericalCoordinate>>();

        while (!sr.EndOfStream)
        {
            float key = 0;

            List<float> values = new List<float>();
            LinkedList<SphericalCoordinate> coorValues = new LinkedList<SphericalCoordinate>();
            string []columns = sr.ReadLine().Split(';');

            for (int i = 0; i < columns.Length; i++)
            {

                 key = float.Parse(columns[0]);

                //float Id = columns[2];
                float radius = float.Parse(columns[1]);
                float inclination = float.Parse(columns[2]);
                float azimuth = float.Parse( columns[3]);
                SphericalCoordinate sc = new SphericalCoordinate(radius, inclination, azimuth);
                coorValues.AddLast(sc);
            }
            data.Add(key, coorValues); 

            }
        return data;
        }
    }

