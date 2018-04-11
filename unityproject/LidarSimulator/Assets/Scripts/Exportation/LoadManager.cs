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

public class LoadManager : MonoBehaviour
{


    void Start()
    {


    }
    public static  IEnumerator LoadCsv(String filename, LidarStorage storage)
    {
        StreamReader sr = new StreamReader(File.OpenRead(filename));
        Dictionary<float, List<LinkedList<SphericalCoordinate>>> data = new Dictionary<float, List<LinkedList<SphericalCoordinate>>>();
        bool internalData = false; //The data to be read was created by our program


        if(sr.Peek().Equals('s')) // First line starts with "sep..." internal representation.
        {
            internalData = true;
            for(int i = 0; i<2;i++)
            {
                sr.ReadLine(); // Discard first two lines
            }
        }
        while (!sr.EndOfStream)
        {
            try
            {
                float key = 0;



                List<float> values = new List<float>();
                LinkedList<SphericalCoordinate> coorValues = new LinkedList<SphericalCoordinate>();
                string[] columns = sr.ReadLine().Split(' ');


                try
                { 
                    if (columns.Length == 4) // Kitty data
                    {
                        key = 1;
                        float x = float.Parse(columns[0]);
                        float z = float.Parse(columns[1]);
                        float y = float.Parse(columns[2]);
                        Vector3 vector = new Vector3(x, y, z);
                        SphericalCoordinate sc = new SphericalCoordinate(vector);
                        coorValues.AddLast(sc);
                    } else if(columns.Length == 8) // Data from simulation
                    {
                        key = float.Parse(columns[0]);
                        float x = float.Parse(columns[1]);
                        float z = float.Parse(columns[2]);
                        float y = float.Parse(columns[3]);
                        float  radius = float.Parse(columns[4]);
                        float inclination = float.Parse(columns[5]);
                        float azimuth = float.Parse(columns[6]);
                        int laserId = int.Parse(columns[7]);
                        Vector3 vector = new Vector3(x, y, z);
                        SphericalCoordinate sc = new SphericalCoordinate(radius,inclination,azimuth,vector,laserId);
                        coorValues.AddLast(sc);
                    } else
                    {
                        throw new FormatException();
                    }

                }
                catch (FormatException e)
                {
                    Debug.Log("Exception! |time|radius|inclination|azimuth" + "|" + columns[0] + "|" + columns[2] + "|" + columns[3] + "|" + columns[4]);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.Log("Length:" + columns.Length);
                }
            
 
                    
                    List<LinkedList<SphericalCoordinate>> oldList;
                List<LinkedList<SphericalCoordinate>> dataList = new List<LinkedList<SphericalCoordinate>>();
                dataList.Add(coorValues);
                    if (!data.TryGetValue(key, out oldList))
                    {
                        data.Add(key, dataList);

                    }
                    else
                    {
                    List<LinkedList<SphericalCoordinate>> existingList = data[key];
                    existingList.Add(coorValues);                        

                    }
                
            } catch(Exception e)
            {
                Debug.Log("Unreadable data: " + e);
            }
        }

        Debug.Log("Setting data in: " + storage.GetHashCode() + " with length" + data.Count);
            storage.SetData(data);

        yield return null;
        }
    }

