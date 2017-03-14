using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for updating a particle system.
/// </summary>
public class PointCloud : MonoBehaviour
{
    public GameObject particleGameObject;

    private Component particleSystem;
    private List<SphericalCoordinates> points;
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
    private ParticleSystem.Particle[] CreateParticles(List<SphericalCoordinates> positions)
    {
        ParticleSystem.Particle[] particleCloud = new ParticleSystem.Particle[positions.Count];

        for (int i = 0; i < positions.Count; i++)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = positions[i].ToCartesian();
            particle.startColor = Color.green;
            particle.startSize = 0.1f;
            particleCloud[i] = particle;
        }
          return particleCloud;
    }

    /// <summary>
    /// Updates the points to be added to the point cloud (the latest from the lidar sensor)
    /// </summary>
    /// <param name="points"></param>
    public void UpdatePoints(List<SphericalCoordinates> points)
    {
        this.points = points;
        pointsUpdate = true;
    }
}
