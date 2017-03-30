
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

