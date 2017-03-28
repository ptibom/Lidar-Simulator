/*
* @author: Philip Tibom
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LidarSensor : MonoBehaviour {
    private float lastUpdate = 0;

    private List<Laser> lasers = new List<Laser>();
    private float horizontalAngle = 0;
   
    public int numberOfLasers = 2;
    public float rotationSpeedHz = 1.0f;
    public float rotationAnglePerStep = 45.0f;
    public float rayDistance = 100f;
    public float simulationSpeed = 1;
    public float upperFOV = 30f;
    public float lowerFOV = 30f;
    public float offset = 0.001f;
    public float upperNormal = 30f;
    public float lowerNormal = 30f;
    public static event NewPoints OnScanned;
    public static event StorePoints StoreEvent;
    public delegate void StorePoints(float timeStamp);
    public delegate void NewPoints(LinkedList<SphericalCoordinates> hits);
    public float storeInterval = 0.25f; // How often do we want to update the data structure

    private LidarStorage dataStructure = new LidarStorage();
	private float previousUpdate;





    // Use this for initialization
    private void Start ()
    {
        Time.timeScale = simulationSpeed; // For now, only be set before start in editor.
        Time.fixedDeltaTime = 0.002f; // Necessary for simulation to be detailed. Default is 0.02f.


        // Initialize number of lasers, based on user selection.
        float upperTotalAngle = upperFOV / 2;
        float lowerTotalAngle = lowerFOV / 2;
        float upperAngle = upperFOV / (numberOfLasers / 2);
        float lowerAngle = lowerFOV / (numberOfLasers / 2);
        for (int i = 0; i < numberOfLasers; i++)
        {
            if (i < numberOfLasers/2)
            {
                lasers.Add(new Laser(gameObject, lowerTotalAngle + lowerNormal, rayDistance, -offset));

                lowerTotalAngle -= lowerAngle;
            }
            else
            {
                lasers.Add(new Laser(gameObject, upperTotalAngle - upperNormal, rayDistance, 0));
                upperTotalAngle -= upperAngle;
            }
            
        }
    }

    // Update is called once per frame
    private void Update ()
    {
        // For debugging, shows visible ray in real time.
        foreach (Laser laser in lasers)
        {
            // Uncomment this line to disable drawing
            laser.DrawRay();
        }
    }

    private void FixedUpdate()
    {
        // Check if it is time to step. Example: 2hz = 2 rotations in a second.
        if (Time.fixedTime - lastUpdate > 1/(360/rotationAnglePerStep)/rotationSpeedHz)
        {
            // Update current execution time.
            lastUpdate = Time.fixedTime;

            // Perform rotation.
            transform.Rotate(0, rotationAnglePerStep, 0);
            horizontalAngle += rotationAnglePerStep; // Keep track of our current rotation.
            if (horizontalAngle >= 360)
            {
                horizontalAngle -= 360;
            }


            LinkedList<SphericalCoordinates> hits = new LinkedList<SphericalCoordinates>();
            // Execute lasers.
            foreach (Laser laser in lasers)
            {
                RaycastHit hit = laser.ShootRay();
                float distance = hit.distance;
                float verticalAngle = laser.GetVerticalAngle();
                hits.AddLast(new SphericalCoordinates(distance, verticalAngle, horizontalAngle));
            }

            // Notify listeners that the lidar sensor have scanned points. 
            if(OnScanned != null)
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
}
