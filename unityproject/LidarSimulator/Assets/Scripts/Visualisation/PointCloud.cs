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
    public int maxParticleSystems = 10;
    public int maxParticlesPerCloud = 10000; // maximum number of particles in a cloud
	public float particleSize = 0.01f;

    private int usedParticleSystem = 0;

    private List<ParticleSystem> particalusSystem;

    //private LinkedList<SphericalCoordinates> points;
    //private bool pointsUpdate = false;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        particalusSystem = new List<ParticleSystem>();
        pointCloudBase = GameObject.FindGameObjectWithTag("PointCloudBase");
        for(int i = 0; i < maxParticleSystems; i++)
        {
            particalusSystem.Add((Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity)).GetComponent<ParticleSystem>());

        }
        LidarSensor.OnScanned += OnUpdatePoints;
    }

    /// <summary>
    /// Updates the particle systems particles if there is new points. 
    /// </summary>
    void Update()
    {
        if (particalusSystem[usedParticleSystem].particleCount >= maxParticlesPerCloud)
        {
            usedParticleSystem = (usedParticleSystem + 1) % maxParticleSystems;
        }
        if (particalusSystem[usedParticleSystem].Equals(null))
        {
            particalusSystem.Add((Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity)).GetComponent<ParticleSystem>());
        }
    }

    /// <summary>
    /// Creates an array of Shuriken Particles for the lidar sensor hits.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions, int particleSystemID)
    {

        //TODO: If current particle systems count is over transform, create new particle system, update usedParticleSystem, count modulo something, so that there is a finite number of particle systems. 
        ParticleSystem.Particle[] oldPoints = new ParticleSystem.Particle[particalusSystem[particleSystemID].particleCount];
        particalusSystem[particleSystemID].GetParticles(oldPoints);

        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();

        foreach (ParticleSystem.Particle p in oldPoints)
        {
            if(p.remainingLifetime > 0)
            {
                particleCloud.Add(p);
            }
        }
        

        for (LinkedListNode<SphericalCoordinates> it = positions.First; it != null; it = it.Next)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = it.Value.ToCartesian();
            if(it.Value.GetInclination() < 3)
            {
                particle.startColor = Color.red;
            } else if(it.Value.GetInclination() > 3 && it.Value.GetInclination() < 7)
            {
                particle.startColor = Color.yellow;
            } else
            {
                particle.startColor = Color.green;
            }
           
            particle.startSize = particleSize;
            particle.startLifetime = 50f;
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
       
        ParticleSystem.Particle[] particleCloud = CreateParticles(points, usedParticleSystem);
        particalusSystem[usedParticleSystem].SetParticles(particleCloud, particleCloud.Length);
                

       }
          
}
