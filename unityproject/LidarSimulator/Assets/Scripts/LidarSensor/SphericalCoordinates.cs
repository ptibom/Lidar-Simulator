using System;
using UnityEngine;

/// <summary>
/// A class representing spherical coordinates. These are created by the lidar sensor.
/// @author: Tobias Alldén
/// </summary>
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
	/// <summary>
	/// Initializes a new instance of the <see cref="SphericalCoordinates"/> class using cartesian coordinates.
	/// </summary>
	/// <param name="coordinates">Coordinates.</param>
    public SphericalCoordinates(Vector3 coordinates)
    {
        this.radius = Mathf.Sqrt(Mathf.Pow(coordinates.x, 2) + Mathf.Pow(coordinates.y, 2) + Mathf.Pow(coordinates.z, 2));

        if (radius == 0)
        {
            inclination = 0;
            azimuth = 0;
        }
        else
        {
            this.inclination = Mathf.Acos(coordinates.z / radius);
            if (coordinates.x != 0)
            {
                this.azimuth = Mathf.Atan(coordinates.y / coordinates.x);
            }
            else
            {
                this.azimuth = 0;
            }
        }
    }

    /// <summary>
    /// Converts a spherical coordinate to a cartesian equivalent. 
    /// </summary>
    /// <returns></returns>
    public Vector3 ToCartesian()
    {
        Vector3 cartesian = new Vector3();
        cartesian.x = ((float)(radius * Math.Sin(inclination) * Math.Cos(azimuth)));
        cartesian.y = ((float)(radius * Math.Sin(inclination) * Math.Sin(azimuth)));
        cartesian.z = ((float)(radius * Math.Cos(inclination)));
        return cartesian;
    }

	/// <summary>
	/// Gets the radius.
	/// </summary>
	/// <returns>The radius.</returns>
    private float GetRadius()
    {
        return this.radius;
    }
	/// <summary>
	/// Gets the inclination.
	/// </summary>
	/// <returns>The inclination.</returns>
    private float GetInclination()
    {
        return this.radius;
    }
	/// <summary>
	/// Gets the azimuth.
	/// </summary>
	/// <returns>The azimuth.</returns>
    private float GetAzimuth()
    {
        return this.azimuth;
    }

    /// <summary>
    /// Clones this instance of the class
    /// </summary>
    /// <returns></returns>
    public SphericalCoordinates Clone()
    {
        return new SphericalCoordinates(this.radius,this.inclination,this.azimuth);
    }

    /// <summary>
    /// Overriding the equals method to be able to avoid float pooint errors.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        double eps = 0.01;
        SphericalCoordinates other = (SphericalCoordinates)obj;
        return (Math.Abs(this.azimuth - other.azimuth) < eps
            && Math.Abs(this.inclination - other.inclination) < eps
            && Math.Abs(this.radius - other.radius) < eps);
    }

    /// <summary>
    /// Override hash code
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return  (int)Math.Floor(azimuth * 3 + inclination * 13 + radius * 11);
    }


}