 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for updating a particle system.
/// @author: Tobias Alldén
/// </summary>
public class PointCloud : MonoBehaviour
{
    public GameObject particleGameObject;
    public GameObject pointCloudBase;
    public bool clearOnPause = true; // clears particles on pause

    private int maxParticleSystems = 50;
    private int maxParticlesPerCloud = 100; // maximum number of particles in a cloud
    private float particleSize = 0.1f;
    private int lapCounter = 0;
    private Dictionary<int, int> particleSystemLapCounter; // The id of the particleSystem and the lap count
    private int lastParticleSystemLastUpdate;
    private bool isEnabled = false;
    private int usedParticleSystem = 0;
    private Dictionary<int,ParticleSystem> particleSystemIdMap;

    //private LinkedList<SphericalCoordinates> points;
    //private bool pointsUpdate = false;

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        particleSystemIdMap = new Dictionary<int, ParticleSystem>();
        pointCloudBase = GameObject.FindGameObjectWithTag("PointCloudBase");
        particleSystemLapCounter = new Dictionary<int, int>();
       // LidarSensor.UpdateLidarSpecifications += UpdateSpecs;
        CreateNeededParticleSystems();
        LidarSensor.OnScanned += OnUpdatePoints;
        LidarSensor.NewRotationEvent += NewLap;
        isEnabled = true;
        LidarMenuScript.PassGuiValsOnStart += UpdateSpecs;


    }

    /// <summary>
    /// Either updates the used particle system, or creates a new one if it is nesescary.
    /// </summary>
    void UpdateParticleSystemIfNeeded()
    {
        
        if(particleSystemLapCounter[usedParticleSystem] != lapCounter)
        {
            particleSystemIdMap[usedParticleSystem].Clear();
            particleSystemLapCounter.Remove(usedParticleSystem);
            particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
        } else
        {
            if(particleSystemIdMap[usedParticleSystem].particleCount > maxParticlesPerCloud)
            {
                int nextParticleSystem = (usedParticleSystem + 1) % maxParticleSystems;
                if(nextParticleSystem >= particleSystemIdMap.Count)
                {
                    GameObject newGO = Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity);
                    newGO.name = "pSyst" + nextParticleSystem;
                    ParticleSystem p = newGO.GetComponent<ParticleSystem>();
                    p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
                    particleSystemIdMap.Add(nextParticleSystem, p);
                    particleSystemLapCounter.Add(nextParticleSystem, lapCounter);
                    usedParticleSystem = nextParticleSystem;
                } else // exists
                {
                    usedParticleSystem = nextParticleSystem;
                    UpdateParticleSystemIfNeeded();
                }
            }
        }


        /**
        if(particleSystemIdMap[usedParticleSystem].particleCount >= maxParticlesPerCloud)
        {
            int nextParticleSystem = (usedParticleSystem + 1)% maxParticleSystems;
            if (nextParticleSystem >= particleSystemIdMap.Count)
            {
                if (nextParticleSystem >= maxParticleSystems || lastParticleSystemLastUpdate != lapCounter) // Either full or new lap
                {
                    usedParticleSystem = 0;
                    particleSystemLapCounter.Remove(usedParticleSystem);
                    particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
                    lastParticleSystemLastUpdate = lapCounter;
                }
                else
                {
                    usedParticleSystem += 1;
                    GameObject newGO = Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity);
                    newGO.name = "pSyst" + usedParticleSystem;
                    ParticleSystem p = newGO.GetComponent<ParticleSystem>();
                    particleSystemIdMap.Add(usedParticleSystem, p);
                    p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
                }
            }
            else
            {
                usedParticleSystem = (usedParticleSystem + 1) % maxParticleSystems;
                particleSystemLapCounter.Remove(usedParticleSystem);
                particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
            }
        }     
    **/



    }

    //TODO: Find a way to fill each system before next iteration. 

    /// <summary>
    /// Creates an array of Shuriken Particles for the lidar sensor hits.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions, int particleSystemID)
    {
        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();
        ParticleSystem currentParticleSystem = particleSystemIdMap[usedParticleSystem];


        if (currentParticleSystem.particleCount < maxParticlesPerCloud)
        {
            ParticleSystem.Particle[] oldParticles = new ParticleSystem.Particle[currentParticleSystem.particleCount];
            currentParticleSystem.GetParticles(oldParticles);
            particleCloud.AddRange(oldParticles);
        }

        for (LinkedListNode<SphericalCoordinates> it = positions.First; it != null; it = it.Next)
        {
            if (it.Value.GetRadius() != 0)
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
                particle.startLifetime = 1f;
                particle.remainingLifetime = 2f;
                particleCloud.Add(particle);
            }
            
        }
        return particleCloud.ToArray();
    }

    /// <summary>
    /// Updates the points to be added to the point cloud (the latest from the lidar sensor)
    /// </summary>
    /// <param name="points"></param>
    public void OnUpdatePoints(LinkedList<SphericalCoordinates> points)
    {
        if (isEnabled) {
            UpdateParticleSystemIfNeeded();
            ParticleSystem.Particle[] particleCloud = CreateParticles(points, usedParticleSystem);
            particleSystemIdMap[usedParticleSystem].SetParticles(particleCloud, particleCloud.Length);
            particleSystemIdMap[usedParticleSystem].Play();
        }
    }



    /// <summary>
    /// Resumes the point cloud after a pause
    /// </summary>
    public void Play()
    {
        LidarSensor.OnScanned += OnUpdatePoints;
        LidarSensor.NewRotationEvent += NewLap;
        isEnabled = true;

        if(clearOnPause == false)
        {
            foreach (var entity in particleSystemIdMap)
            {
                entity.Value.Play();
            }
        }
    }

    /// <summary>
    /// Pauses the visualization
    /// </summary>
    public void Pause()
    {
        LidarSensor.OnScanned -= OnUpdatePoints;
        LidarSensor.NewRotationEvent -= NewLap;
        isEnabled = false;

        if(clearOnPause)
        {
            foreach (var entity in particleSystemIdMap)
            {
                entity.Value.Clear();
            }
            usedParticleSystem = 0;
        } else
        {
            foreach (var entity in particleSystemIdMap)
            {
                entity.Value.Pause();
            }
        }

    }


/// <summary>
/// This method is called when the lidar specifications are changed. Calculates the number of particle systems needed, point size and number of particles / system.
/// </summary>
    public void UpdateSpecs(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep, float rayDistance, float upperFOV
        , float lowerFOV, float offset, float upperNormal, float lowerNormal)
    {
        Pause();
        int particlesPerSystem;

        int maxNumParticlesPerLap = (int) Mathf.Ceil((360 * numberOfLasers) / rotationAnglePerStep); // maximum number of raycast hits per lap


        if(maxNumParticlesPerLap < 250000)
        {
            particlesPerSystem = 3000;
            particleSize = 0.1f;
            
        }  else if(maxNumParticlesPerLap < 750000)
        {
            particlesPerSystem = 5000;
            particleSize = 0.05f;
        } else
        {
            particlesPerSystem = 10000;
            particleSize = 0.01f;
        }
        
        maxParticleSystems = (int)Mathf.Ceil((float)maxNumParticlesPerLap / (float)particlesPerSystem);
        CreateNeededParticleSystems();
        Play();


    }


    /// <summary>
    /// Creates the needed number of particle systems.
    /// </summary>
    private void CreateNeededParticleSystems()
    {
        int currentNumberOfSystems = particleSystemIdMap.Count;

        if (currentNumberOfSystems < maxParticleSystems / 2)
        {
            for (int i = currentNumberOfSystems; i < maxParticleSystems / 2; i++)
            {

                GameObject newGO = Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity);
                newGO.name = "pSyst" + i;
                ParticleSystem p = newGO.GetComponent<ParticleSystem>();
                p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
                particleSystemIdMap.Add(i, p);
                particleSystemLapCounter.Add(i, lapCounter);

            }
        } else if(currentNumberOfSystems > maxParticleSystems)
        {
            for(int i = maxParticleSystems; i< currentNumberOfSystems; i++)
            {
                particleSystemIdMap[i].Clear();
                particleSystemIdMap.Remove(i);
                particleSystemLapCounter.Remove(i);
                Destroy(GameObject.Find("pSyst" + i));

            }
        }       

    }

    /// <summary>
    /// Is signalled when the lidar sensor has completed a lap, increments lap counter, used to distinguish wether a new particle system will be created.
    /// </summary>
    public void NewLap()
    {
        if (isEnabled)
        { 
            lapCounter++;
        }
    }
}
