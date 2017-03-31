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
    public GameObject pointCloudBase;

    private int usedParticleSystem = 0;

    private List<ParticleSystem> particalusSystem;

    //private LinkedList<SphericalCoordinates> points;
    //private bool pointsUpdate = false;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        pointCloudBase = GameObject.FindGameObjectWithTag("PointCloudBase");
        Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity);
        

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

        //TODO: If current particle systems count is over transform, create new particle system, update usedParticleSystem, count modulo something, so that there is a finite number of particle systems. 
        ParticleSystem.Particle[] oldPoints = new ParticleSystem.Particle[particalusSystem[0].particleCount];
        particalusSystem[0].GetParticles(oldPoints);

        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();

        foreach (ParticleSystem.Particle p in oldPoints)
        {
            //TODO: 
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
            particle.startLifetime = 50f;
            particle.remainingLifetime = 50f;
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
        //TODO: Perhaps make this event driven
           ParticleSystem.Particle[] particleCloud = CreateParticles(points);
             if (particleCloud.Length != 0)
             {
                 particalusSystem[0].SetParticles(particleCloud, particleCloud.Length);
             }
           

       }
          
}
