using System;
using UnityEngine;

/// <summary>
/// A class representing spherical coordinates. These are created by the lidar sensor.
/// @author: Tobias Alldén
/// </summary>
public class SphericalCoordinate
{
    private Vector3 globalWorldCoordinate; // Useful for some things. The other coordinates are local.
    private int laserId;
    private float radius;
    private float inclination;
    private float azimuth;

    public SphericalCoordinate(float radius, float inclination, float azimuth, Vector3 globalWorldCoordinate, int laserId)
    {
        this.radius = radius;
        this.inclination = (90 + inclination)*(2*Mathf.PI/360);
        this.azimuth = azimuth * (2 * Mathf.PI / 360);
        this.globalWorldCoordinate = globalWorldCoordinate;
        this.laserId = laserId;
    }

    // Constructor based on cartesian coordinates
	/// <summary>
	/// Initializes a new instance of the <see cref="SphericalCoordinate"/> class using cartesian coordinates.
	/// </summary>
	/// <param name="coordinates">Coordinates.</param>
    public SphericalCoordinate(Vector3 coordinates)
    {
        // Det här är fel (todo)

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
        cartesian.z = radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth);
        cartesian.x = radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth);
        cartesian.y = radius * Mathf.Cos(inclination);
        return cartesian;
    }

	/// <summary>
	/// Gets the radius.
	/// </summary>
	/// <returns>The radius.</returns>
    public float GetRadius()
    {
        return this.radius;
    }
    /// <summary>
    /// Gets the inclination.
    /// </summary>
    /// <returns>The inclination.</returns>
    public float GetInclination()
    {
        return this.radius;
    }
    /// <summary>
    /// Gets the azimuth.
    /// </summary>
    /// <returns>The azimuth.</returns>
    public float GetAzimuth()
    {
        return this.azimuth;
    }

    /// <summary>
    /// Clones this instance of the class
    /// </summary>
    /// <returns></returns>
    public SphericalCoordinate Clone()
    {
        return new SphericalCoordinate(this.radius, this.inclination, this.azimuth, 
            new Vector3(globalWorldCoordinate.x, globalWorldCoordinate.y, globalWorldCoordinate.z), this.laserId);
    }

    /// <summary>
    /// Overriding the equals method to be able to avoid float pooint errors.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        double eps = 0.01;
        SphericalCoordinate other = (SphericalCoordinate)obj;
        return (Math.Abs(this.azimuth - other.azimuth) < eps
            && Math.Abs(this.inclination - other.inclination) < eps
            && Math.Abs(this.radius - other.radius) < eps);
    }

    /// <summary>
    /// Override hash code
    /// </summary>
    /// <returns></returns>
	/*public String ToString() {
		return radius.ToString () + ";" + inclination.ToString () + ";" + azimuth.ToString () + ":::";
	}*/
	
    public override int GetHashCode()
    {
        return  (int)Math.Floor(azimuth * 3 + inclination * 13 + radius * 11);
    }


	public String ToString() {
		return "Radius: " + radius.ToString () + " Inclination: " + inclination.ToString () + " Azimuth: " + azimuth.ToString () +
		" World coordinates: " + globalWorldCoordinate.ToString () + " LaserID: " + laserId.ToString () + " ::: ";
	}
}