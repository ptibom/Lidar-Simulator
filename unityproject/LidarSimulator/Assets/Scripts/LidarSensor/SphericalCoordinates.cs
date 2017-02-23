using System;
using UnityEngine;

public class SphericalCoordinates
{
    private float radius;
    private float inclination;
    private float azimuth;

    public SphericalCoordinates(float radius, float inclination, float azimuth)
    {
        this.radius = radius;
        this.inclination = inclination;
        this.azimuth = azimuth;
    }

    // Constructor based on cartesian coordinates
    public SphericalCoordinates(Vector3 coordinates)
    {
        this.radius = Mathf.Sqrt(Mathf.Pow(coordinates.x, 2) + Mathf.Pow(coordinates.y, 2) + Mathf.Pow(coordinates.z, 2));

        if (radius == 0)
        {
            inclination = 0;
            azimuth = 0;
        }
        this.inclination = Mathf.Atan(coordinates.z / radius);
        this.azimuth = Mathf.Atan(coordinates.y / coordinates.x);
    }

    private float GetRadius()
    {
        return this.radius;
    }
    private float GetInclination()
    {
        return this.radius;
    }
    private float GetAzimuth()
    {
        return this.azimuth;
    }
}