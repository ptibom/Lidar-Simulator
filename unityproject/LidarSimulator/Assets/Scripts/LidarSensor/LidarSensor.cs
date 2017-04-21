/*
* @author: Philip Tibom
*/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Philip Tibom
/// Simulates the lidar sensor by using ray casting.
/// </summary>
public class LidarSensor : MonoBehaviour {
    private float lastUpdate = 0;

    private List<Laser> lasers;
    private float horizontalAngle = 0;
   
    public int numberOfLasers = 2;
    public float rotationSpeedHz = 1.0f;
    public float rotationAnglePerStep = 45.0f;
    public float rayDistance = 100f;
    public float upperFOV = 30f;
    public float lowerFOV = 30f;
    public float offset = 0.001f;
    public float upperNormal = 30f;
    public float lowerNormal = 30f;
    public static event NewPoints OnScanned;
    public static event StorePoints StoreEvent;
    public static event NewLap NewRotationEvent;
    public delegate void NewLap();
    public delegate void StorePoints(float timeStamp);
    public delegate void NewPoints(LinkedList<SphericalCoordinate> hits);
    public float storeInterval = 0.25f; // How often do we want to update the data structure
    public float lapTime = 0;
    private float lastLapTime = 0;

    private bool isPlaying = false;

	public GameObject pointCloudObject;

    private LidarStorage dataStructure = new LidarStorage();
	private float previousUpdate;

    public GameObject lineDrawerPrefab;

    // Use this for initialization
    private void Start()
    {
        LidarMenu.OnStartSimulation += UpdateSettings;
        LidarMenu.OnStopSimulation += PauseSensor;
    }

    public void UpdateSettings(int numberOfLasers, float rotationSpeedHz, float rotationAnglePerStep, float rayDistance, float upperFOV,
        float lowerFOV, float offset, float upperNormal, float lowerNormal)
    {
        this.numberOfLasers = numberOfLasers;
        this.rotationSpeedHz = rotationSpeedHz;
        this.rotationAnglePerStep = rotationAnglePerStep;
        this.rayDistance = rayDistance;
        this.upperFOV = upperFOV;
        this.lowerFOV = lowerFOV;
        this.offset = offset;
        this.upperNormal = upperNormal;
        this.lowerNormal = lowerNormal;

        InitiateLasers();
    }

    private void InitiateLasers()
    {
        // Initialize number of lasers, based on user selection.
        if (lasers != null)
        {
            foreach (Laser l in lasers)
            {
                Destroy(l.GetRenderLine().gameObject);
            }
        }

        lasers = new List<Laser>();

        float upperTotalAngle = upperFOV / 2;
        float lowerTotalAngle = lowerFOV / 2;
        float upperAngle = upperFOV / (numberOfLasers / 2);
        float lowerAngle = lowerFOV / (numberOfLasers / 2);
        offset = (offset / 100) / 2; // Convert offset to centimeters.
        for (int i = 0; i < numberOfLasers; i++)
        {
            GameObject lineDrawer = Instantiate(lineDrawerPrefab);
            lineDrawer.transform.parent = gameObject.transform; // Set parent of drawer to this gameObject.
            if (i < numberOfLasers / 2)
            {
                lasers.Add(new Laser(gameObject, lowerTotalAngle + lowerNormal, rayDistance, -offset, lineDrawer, i));

                lowerTotalAngle -= lowerAngle;
            }
            else
            {
                lasers.Add(new Laser(gameObject, upperTotalAngle - upperNormal, rayDistance, offset, lineDrawer, i));
                upperTotalAngle -= upperAngle;
            }
        }

        isPlaying = true;
    }

    public void PauseSensor()
    {
        isPlaying = false;
    }

    // Update is called once per frame
    private void Update ()
    {
        // For debugging, shows visible ray in real time.
        /*
        foreach (Laser laser in lasers)
        {
            laser.DebugDrawRay();
        }
        */
    }

    private void FixedUpdate()
    {
        // Do nothing, if the simulator is paused.
        if (!isPlaying)
        {
            return;
        }

        // Check if number of steps is greater than possible calculations by unity.
        float numberOfStepsNeededInOneLap = 360 / rotationAnglePerStep;
        float numberOfStepsPossible = 1 / Time.fixedDeltaTime / 5;
        float precalculateIterations = 1;
        // Check if we need to precalculate steps.
        if (numberOfStepsNeededInOneLap > numberOfStepsPossible)
        {
            precalculateIterations = (int)(numberOfStepsNeededInOneLap / numberOfStepsPossible);
            if (360 % precalculateIterations != 0)
            {
                precalculateIterations += 360 % precalculateIterations;
            }
        }

        // Check if it is time to step. Example: 2hz = 2 rotations in a second.
        if (Time.fixedTime - lastUpdate > (1/(numberOfStepsNeededInOneLap)/rotationSpeedHz) * precalculateIterations)
        {
            // Update current execution time.
            lastUpdate = Time.fixedTime;
            LinkedList<SphericalCoordinate> hits = new LinkedList<SphericalCoordinate>();

            for (int i = 0; i < precalculateIterations; i++)
            {
                // Perform rotation.
                transform.Rotate(0, rotationAnglePerStep, 0);
                horizontalAngle += rotationAnglePerStep; // Keep track of our current rotation.
                if (horizontalAngle >= 360)
                {
                    horizontalAngle -= 360;
					//GameObject.Find("RotSpeedText").GetComponent<Text>().text =  "" + (1/(Time.fixedTime - lastLapTime));
                    lastLapTime = Time.fixedTime;
                    if(NewRotationEvent != null)
                    {
                        NewRotationEvent();
                    }
                }


                // Execute lasers.
                foreach (Laser laser in lasers)
                {
                    RaycastHit hit = laser.ShootRay();
                    float distance = hit.distance;
                    float verticalAngle = laser.GetVerticalAngle();
                    hits.AddLast(new SphericalCoordinate(distance, verticalAngle, horizontalAngle, hit.point, laser.GetLaserId()));
                }
            }

            
            // Notify listeners that the lidar sensor have scanned points. 
			if (OnScanned != null  && pointCloudObject != null && pointCloudObject.activeInHierarchy)
            {
                OnScanned(hits);
            }
            
            if (Time.fixedTime - previousUpdate > storeInterval) {
                // Notify data structure that it is time to store the collected points
				if(StoreEvent != null)
                {
                    StoreEvent(Time.fixedTime);
                    previousUpdate = Time.fixedTime;
                }
            }
        }
    }

    public LidarStorage GetLidarStorage()
    {
        return dataStructure;
    }
}
