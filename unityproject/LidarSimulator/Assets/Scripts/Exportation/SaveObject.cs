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


using System.Collections.Generic ;
using UnityEngine;
using System ;
using System.Runtime.Serialization ;
[Serializable]
public class SaveObject
{

    private float time;
    private int ID;
    private float radius;
    private float inclination;
    private float azimuth;
   
   
    public SaveObject(float time, int ID, float radius, float inclinaton, float azimuth)
    {

        this.time = time;
        this.ID=ID;
        this.radius = radius;
        this.inclination = inclinaton;
        this.azimuth = azimuth;


    }
    

    public float GetRadius()
    {
        return radius;
    }
    public float Gettime()
    {
        return time ;
    }

    public float GetInclination()
    {
        return inclination;
    }
    public float GetId()
    {
        return ID;
    }
    public float GetAzimuth()
    {
        return azimuth;
    }
}

