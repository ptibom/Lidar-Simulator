using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///



/// <summary>
/// Script for updating a particle system.
/// @author: Tobias Alldén
/// </summary>
public class PointCloud : MonoBehaviour
{
    public GameObject particleGameObject;

    private Component particleSystem;
    private LinkedList<SphericalCoordinates> points;
    private bool pointsUpdate = false;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        particleGameObject = GameObject.FindGameObjectWithTag("pSystem");
        particleSystem = particleGameObject.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Updates the particle systems particles if there is new points. 
    /// </summary>
    void Update()
    {
        if (pointsUpdate)
        {
            ParticleSystem.Particle[] particleCloud = CreateParticles(points);

            if (particleCloud.Length != 0)
            {
               ((ParticleSystem)(particleSystem)).SetParticles(particleCloud, particleCloud.Length);                 
            }
        }
    }

    /// <summary>
    /// Creates an array of Shuriken Particles for the lidar sensor hits.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions)
    {
        ParticleSystem.Particle[] particleCloud = new ParticleSystem.Particle[positions.Count];

        int i = 0;
        foreach(SphericalCoordinates sc in positions)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = sc.ToCartesian();
            particle.startColor = Color.green;
            particle.startSize = 0.1f;
            particleCloud[i] = particle;
            i++;
        }
          return particleCloud;
    }

    /// <summary>
    /// Updates the points to be added to the point cloud (the latest from the lidar sensor)
    /// </summary>
    /// <param name="points"></param>
    public void UpdatePoints(LinkedList<SphericalCoordinates> points)
    {
        this.points = points;
        pointsUpdate = true;
    }
}
