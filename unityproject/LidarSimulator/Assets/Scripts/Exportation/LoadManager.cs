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
        int line = 0;
        while (!sr.EndOfStream)
        {
            if (line < 3)
            {
                line++;
            }
            else 
            {
                float key = 0;

                List<float> values = new List<float>();
                LinkedList<SphericalCoordinate> coorValues = new LinkedList<SphericalCoordinate>();
                string[] columns = sr.ReadLine().Split(';');

                if (columns.Length == 5) {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        try
                        {
                            //float id = columns[2];
                            float radius = float.Parse(columns[2]);
                            float inclination = float.Parse(columns[3]);
                            float azimuth = float.Parse(columns[4]);
                            SphericalCoordinate sc = new SphericalCoordinate(radius, inclination, azimuth, new Vector3(), 0); // TODO: load world coordinate and ID??
                            coorValues.AddLast(sc);
                        }
                        catch (FormatException e)
                        {
                            Debug.Log("Exception! |time|radius|inclination|azimuth" + "|" + columns[0] + "|" + columns[2] + "|" + columns[3] + "|" + columns[4]);
                        } catch (ArgumentOutOfRangeException e)
                        {
                            Debug.Log("Length:" + columns.Length);
                        }



                        key = float.Parse(columns[0]);


                    }
                    LinkedList<SphericalCoordinate> oldList;
                    if (!data.TryGetValue(key, out oldList))
                    {
                        data.Add(key, coorValues);
                    }
                    else
                    {
                        foreach (var v in coorValues)
                        {
                            oldList.AddLast(v);
                        }
                    }
                }
            }
        }
        return data;
        }
    }

