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
    private int maxParticlesPerCloud = 500; // maximum number of particles in a particle system
    private float particleSize = 0.1f;
    private int lastParticleSystemLastUpdate;
    private bool isEnabled = false;
    private int usedParticleSystem = 0;
    int currentNumberOfSystems;
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
        currentNumberOfSystems = 0;
        CreateNeededParticleSystems();
        LidarSensor.OnScanned += OnUpdatePoints;
        isEnabled = true;
        LidarMenu.OnPassLidarValuesToPointCloud += UpdateSpecs;

    }

    void OnDestroy()
    {
        LidarSensor.OnScanned -= OnUpdatePoints;
        LidarMenu.OnPassLidarValuesToPointCloud -= UpdateSpecs;
    }

 

    //TODO: Find a way to fill each system before next iteration. 

    /// <summary>
    /// Creates an array of Shuriken Particles for the lidar sensor hits.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinate> positions, int particleSystemID)
    {
        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();
        ParticleSystem currentParticleSystem = particleSystemIdMap[usedParticleSystem];

        ParticleSystem.Particle[] oldParticles = new ParticleSystem.Particle[currentParticleSystem.particleCount];

        bool reuse = false;
        if (currentParticleSystem.particleCount <= maxParticlesPerCloud)
        {
            currentParticleSystem.GetParticles(oldParticles);
            particleCloud.AddRange(oldParticles);
        } else
        {
            reuse = true;
            currentParticleSystem.GetParticles(oldParticles);
            particleCloud.Clear();
        }
        int i = 0;
        foreach (var coordinate in positions)
        {
                ParticleSystem.Particle particle;
                if (reuse)
                {
                    particle = oldParticles[i];
                } else
                {
                    particle = new ParticleSystem.Particle();
                }
                particle.position = coordinate.ToCartesian();
                if (coordinate.GetRadius() < 5)
                {
                    particle.startColor = Color.red;

                }
                else if (coordinate.GetRadius() > 5 && coordinate.GetRadius() < 10)
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

            i++;
        }
        return particleCloud.ToArray();
    }

    /// <summary>
    /// Updates the points to be added to the point cloud (the latest from the lidar sensor)
    /// </summary>
    /// <param name="points"></param>
    public void OnUpdatePoints(LinkedList<SphericalCoordinate> points)
    {
        if (isEnabled) {
            if (particleSystemIdMap[usedParticleSystem].particleCount > maxParticlesPerCloud)
            {
                usedParticleSystem = (usedParticleSystem + 1) % maxParticleSystems;
            }
            ParticleSystem.Particle[] particleCloud = CreateParticles(points, usedParticleSystem);
            particleSystemIdMap[usedParticleSystem].SetParticles(particleCloud, particleCloud.Length);
        }
    }



    /// <summary>
    /// Resumes the point cloud after a pause
    /// </summary>
    public void Play()
    {
        LidarSensor.OnScanned += OnUpdatePoints;
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
    public void UpdateSpecs(int numberOfLasers, float rotationSpeed, float rotationAnglePerStep)
    {
        

        int maxNumParticlesPerLap = (int) Mathf.Ceil((360 * numberOfLasers) / rotationAnglePerStep); // maximum number of raycast hits per lap


        if(maxNumParticlesPerLap < 250000)
        {
            particleSize = 0.1f;
            
        }  else if(maxNumParticlesPerLap < 750000)
        {
            particleSize = 0.05f;
        } else
        {
            particleSize = 0.01f;
        }

        int newMaxparticleSystems = (int)Mathf.Ceil((float)maxNumParticlesPerLap / (float)maxParticlesPerCloud);

        if (this.maxParticleSystems < newMaxparticleSystems)
        {
            maxParticleSystems = newMaxparticleSystems;
            CreateNeededParticleSystems();
        }     

    }


    /// <summary>
    /// Creates the needed number of particle systems.
    /// </summary>
    private void CreateNeededParticleSystems()
    {
            for (int i = currentNumberOfSystems; i < maxParticleSystems; i++)
            {
                GameObject newGO = Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity);
                newGO.name = "pSyst" + i;
                ParticleSystem p = newGO.GetComponent<ParticleSystem>();
                p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
                particleSystemIdMap.Add(i, p);
                currentNumberOfSystems++;
            }

    }

}
