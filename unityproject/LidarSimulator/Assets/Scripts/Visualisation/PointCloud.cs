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

    private ParticleSystem particalusSystem;
    //private LinkedList<SphericalCoordinates> points;
    //private bool pointsUpdate = false;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        particleGameObject = GameObject.FindGameObjectWithTag("pSystem");
        particalusSystem = particleGameObject.GetComponent<ParticleSystem>();
        LidarSensor.OnScanned += OnUpdatePoints;
    }

    /// <summary>
    /// Updates the particle systems particles if there is new points. 
    /// </summary>
    void Update()
    {
        /*
        if (pointsUpdate)
        {
            particalusSystem.Pause();
            ParticleSystem.Particle[] particleCloud = CreateParticles(points);

            if (particleCloud.Length != 0)
            {
                particalusSystem.SetParticles(particleCloud, particleCloud.Length);
            }
            
            pointsUpdate = false;
            particalusSystem.Play();
        }*/
    }

    /// <summary>
    /// Creates an array of Shuriken Particles for the lidar sensor hits.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions)
    {

        ParticleSystem.Particle[] oldPoints = new ParticleSystem.Particle[particalusSystem.particleCount];
        particalusSystem.GetParticles(oldPoints);

        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();

        foreach (ParticleSystem.Particle p in oldPoints)
        {
            if (p.remainingLifetime > 0 )
            {
                particleCloud.Add(p);
            }
        }

        for (LinkedListNode<SphericalCoordinates> it = positions.First; it != null; it = it.Next)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = it.Value.ToCartesian();
            particle.startColor = Color.green;
            particle.startSize = 0.1f;
            particle.startLifetime = 1f;
            particle.remainingLifetime = 1f;
            particleCloud.Add(particle);
        }

        return particleCloud.ToArray();
    }

    /// <summary>
    /// Updates the points to be added to the point cloud (the latest from the lidar sensor)
    /// </summary>
    /// <param name="points"></param>
    public void OnUpdatePoints(LinkedList<SphericalCoordinates> points)
    {

        ParticleSystem.Particle[] particleCloud = CreateParticles(points);
        if (particleCloud.Length != 0)
        {
            particalusSystem.SetParticles(particleCloud, particleCloud.Length);
        }
        
    }
}
