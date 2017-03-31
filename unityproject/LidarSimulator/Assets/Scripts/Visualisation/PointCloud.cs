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

    private ParticleSystem[] particalusSystem;

    //private LinkedList<SphericalCoordinates> points;
    //private bool pointsUpdate = false;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        particalusSystem = new ParticleSystem[10];
        particleGameObject = GameObject.Find("pSystem1");
        particalusSystem[0] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem2");
        particalusSystem[1] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem3");
        particalusSystem[2] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem4");
        particalusSystem[3] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem5");
        particalusSystem[4] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem6");
        particalusSystem[5] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem7");
        particalusSystem[6] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem8");
        particalusSystem[7] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem9");
        particalusSystem[8] = particleGameObject.GetComponent<ParticleSystem>();
        particleGameObject = GameObject.Find("pSystem10");
        particalusSystem[9] = particleGameObject.GetComponent<ParticleSystem>();
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
    private ParticleSystem.Particle[] CreateParticles(int number, LinkedList<SphericalCoordinates> positions)
    {

        ParticleSystem.Particle[] oldPoints = new ParticleSystem.Particle[particalusSystem[number].particleCount];
        particalusSystem[number].GetParticles(oldPoints);

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
        if (particalusSystem[9].particleCount < 1000)
        {
            for (int i = 0; i < 10; i++)
            {
                if (particalusSystem[i].particleCount < 1000)
                {
                    ParticleSystem.Particle[] particleCloud = CreateParticles(i, points);
                    if (particleCloud.Length != 0)
                    {
                        particalusSystem[i].SetParticles(particleCloud, particleCloud.Length);
                    }
                    break;
                }

            }
        }
           
    }
}
