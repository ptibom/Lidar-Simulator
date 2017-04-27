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
    public static  IEnumerator LoadCsv(String filename, LidarStorage storage)
    {
        StreamReader sr = new StreamReader(File.OpenRead(filename));
        Dictionary<float, LinkedList<SphericalCoordinate>> data = new Dictionary<float, LinkedList<SphericalCoordinate>>();
        int line = 0;
        while (!sr.EndOfStream)
        {
            try {
                float key = 0;

                List<float> values = new List<float>();
                LinkedList<SphericalCoordinate> coorValues = new LinkedList<SphericalCoordinate>();
                string[] columns = sr.ReadLine().Split(';');

                if (columns.Length == 5) {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        try
                        {
                            /**UNCOMMENT FOR KITTY DATA
                             * 
                            //float id = columns[2];
                            float radius = float.Parse(columns[2]);
                            float inclination = float.Parse(columns[3]);
                            float azimuth = float.Parse(columns[4]);
                            SphericalCoordinate sc = new SphericalCoordinate(radius, inclination, azimuth); // TODO: load world coordinate and ID??
                            coorValues.AddLast(sc);                       

                        **/


                            float x = float.Parse(columns[1]);
                            float y = float.Parse(columns[2]);
                            float z = float.Parse(columns[3]);
                            Vector3 vector = new Vector3(x,y,z);

                            SphericalCoordinate sc = new SphericalCoordinate(vector);
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
            } catch(Exception e)
            {
                Debug.Log("Unreadable data: " + e);
            }
        }
        storage.SetData(data);

        yield return null;
        }
    }

