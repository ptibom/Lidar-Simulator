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
    public bool clearOnPause = true;; // clears particles on pause



    private int lapCounter = 0;
    private Dictionary<int, int> particleSystemLapCounter; // The id of the particleSystem and the lap count
    private int lastParticleSystemLastUpdate;
    private bool isEnabled;
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

        for (int i = 0; i < maxParticleSystems / 2; i++)
        {
            ParticleSystem p = (Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity)).GetComponent<ParticleSystem>();
            p.transform.SetParent(GameObject.Find("ParticleSystems").transform);
            particleSystemIdMap.Add(i,p);
            particleSystemLapCounter.Add(i,lapCounter);

        }
        LidarSensor.OnScanned += OnUpdatePoints;
        LidarSensor.NewRotationEvent += NewLap;
        isEnabled = true;
    }

    /// <summary>
    /// Either updates the used particle system, or creates a new one if it is nesescary.
    /// </summary>
    void UpdateParticleSystemIfNeeded()
    {
        int nextParticleSystem = usedParticleSystem+1;
        if(nextParticleSystem >= particleSystemIdMap.Count) 
        {
            if(nextParticleSystem >= maxParticleSystems || lastParticleSystemLastUpdate != lapCounter) // Either full or new lap
            {
                usedParticleSystem = 0;
                particleSystemLapCounter.Remove(usedParticleSystem);
                particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
                Debug.Log("Relap");
            } else
            {
                usedParticleSystem += 1;
                ParticleSystem p = (Instantiate(particleGameObject, pointCloudBase.transform.position, Quaternion.identity)).GetComponent<ParticleSystem>();
                particleSystemIdMap.Add(usedParticleSystem,p);
                p.transform.SetParent(GameObject.Find("ParticleSystems").transform);                
                //particleSystemLapCounter.Add(usedParticleSystem, lapCounter);
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
    private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions, int particleSystemID)
    {
        List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();
        for (LinkedListNode<SphericalCoordinates> it = positions.First; it != null; it = it.Next)
        {
            if (it.Value.GetRadius() == 0)
            {
            }
            else
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
                particle.startLifetime = 10f;
                particle.remainingLifetime = 1f;
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
        UpdateParticleSystemIfNeeded();
        ParticleSystem.Particle[] particleCloud = CreateParticles(points, usedParticleSystem);
        particleSystemIdMap[usedParticleSystem].SetParticles(particleCloud, particleCloud.Length);
        particleSystemIdMap[usedParticleSystem].Play();
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
    public void UpdateSpecs(int numLasers, float rotAngle)
    {
       // maxParticleSystems = (int) Mathf.Ceil(360*numLasers/(2000*rotAngle));


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
