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
    public int maxParticlesPerCloud = 2000; // maximum number of particles in a cloud
	public float particleSize = 0.01f;
    private int lapCounter = 0;
    private Dictionary<int, int> particleSystemLapCounter; // The id of the particleSystem and the lap count

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
        particleSystemLapCounter = new Dictionary<int, int>();

        for (int i = 0; i < maxParticleSystems / 2; i++)
        {
            ParticleSystem p = (Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity)).GetComponent<ParticleSystem>();
            p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
            particalusSystem.Add(p);
            particleSystemLapCounter.Add(i,lapCounter);

        }
        LidarSensor.OnScanned += OnUpdatePoints;
        LidarSensor.NewRotationEvent += NewLap;
    }

    /// <summary>
    /// Either updates the used particle system, or creates a new one if it is nesescary.
    /// </summary>
    void UpdateParticleSystemIfNeeded()
    {
        int nextParticleSystem = usedParticleSystem+1;
        if(nextParticleSystem >= particalusSystem.Count) 
        {
            if(nextParticleSystem >= maxParticleSystems || particleSystemLapCounter[usedParticleSystem] != lapCounter) // Either full or new lap
            {
                usedParticleSystem = 0;
                particleSystemLapCounter.Remove(usedParticleSystem);
                particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
                Debug.Log("Relap");
            } else
            {
                usedParticleSystem += 1;
                ParticleSystem p = (Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity)).GetComponent<ParticleSystem>();
                particalusSystem.Add(p);
                p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
                particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
            }
        } else
        {
            usedParticleSystem = (usedParticleSystem + 1) % maxParticleSystems;
            particleSystemLapCounter.Remove(usedParticleSystem);
            particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
        }
        
    }

    /// <summary>
    /// Creates an array of Shuriken Particles for the lidar sensor hits.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions, int particleSystemID)    {


        //TODO: If current particle systems count is over transform, create new particle system, update usedParticleSystem, count modulo something, so that there is a finite number of particle systems. 
        ParticleSystem.Particle[] oldPoints = new ParticleSystem.Particle[particalusSystem[particleSystemID].particleCount];
        particalusSystem[particleSystemID].GetParticles(oldPoints);

        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();
        if (particalusSystem[particleSystemID].particleCount < maxParticlesPerCloud)
        {
            foreach (ParticleSystem.Particle p in oldPoints)
            {
                if (p.remainingLifetime > 0)
                {
                    particleCloud.Add(p);
                }
            }
        }



        for (LinkedListNode<SphericalCoordinates> it = positions.First; it != null; it = it.Next)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = it.Value.ToCartesian();
            if (it.Value.GetInclination() < 3)
            {
                particle.startColor = Color.red;
            }
            else if (it.Value.GetInclination() > 3 && it.Value.GetInclination() < 7)
            {
                particle.startColor = Color.yellow;
            }
            else
            {
                particle.startColor = Color.green;
            }

            particle.startSize = particleSize;
            particle.startLifetime = 0.2f;
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
        UpdateParticleSystemIfNeeded();
        Debug.Log("Used Particle System: " + usedParticleSystem);

        ParticleSystem.Particle[] particleCloud = CreateParticles(points, usedParticleSystem);
        particalusSystem[usedParticleSystem].SetParticles(particleCloud, particleCloud.Length);
        particalusSystem[usedParticleSystem].Play();
                

       }

    /// <summary>
    /// Is signalled when the lidar sensor has completed a lap, increments lap counter, used to distinguish wether a new particle system will be created.
    /// </summary>
    public void NewLap()
    {
        lapCounter++;
    }

	void OnDisable() {
		LidarSensor.OnScanned -= OnUpdatePoints;
		LidarSensor.NewRotationEvent -= NewLap;
	}
 	
	void OnEnable(){
		LidarSensor.OnScanned += OnUpdatePoints;
		LidarSensor.NewRotationEvent += NewLap;
	}
}
