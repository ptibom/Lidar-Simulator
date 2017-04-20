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
    public static  Dictionary<float, LinkedList<SphericalCoordinates>> LoadCsv(String filename)
    {
        StreamReader sr = new StreamReader(File.OpenRead(filename));
     
        Dictionary<float, LinkedList<SphericalCoordinates>> data = new Dictionary<float, LinkedList<SphericalCoordinates>>();

        while (!sr.EndOfStream)
        {
            float key = 0;

            List<float> values = new List<float>();
            LinkedList<SphericalCoordinates> coorValues = new LinkedList<SphericalCoordinates>();
            string []columns = sr.ReadLine().Split(';');

            for (int i = 0; i < columns.Length; i++)
            {

                 key = float.Parse(columns[0]);

                //float Id = columns[2];
                float radius = float.Parse(columns[1]);
                float inclination = float.Parse(columns[2]);
                float azimuth = float.Parse( columns[3]);
                SphericalCoordinates sc = new SphericalCoordinates(radius, inclination, azimuth);
                coorValues.AddLast(sc);
            }
            data.Add(key, coorValues); 

            }
        return data;
        }
    }

