using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour {
    private float lastUpdate = 0;

    private List<Laser> lasers = new List<Laser>();
    private Dictionary<float, List<SpericalCoordinates>> hitList; 
    //private List<RaycastHit> hits = new List<RaycastHit>();

    public int numberOfLasers = 2;
    public float rotationSpeedHz = 1.0f;
    public float rotationAnglePerStep = 45.0f;
    public float rayDistance = 100f;
    public float simulationSpeed = 1;
    public float verticalFOV = 30f;
    public GameObject originSensor;
    private float startTime;
    


    // Use this for initialization
    private void Start () {
        this.hitList = new Dictionary<float, List<SpericalCoordinates>>();
        Time.timeScale = simulationSpeed; // For now, only be set before start in editor.
        Time.fixedDeltaTime = 0.002f; // Necessary for simulation to be detailed. Default is 0.02f.
        

        // Initialize number of lasers, based on user selection.
        float completeAngle = verticalFOV/2;
        float angle = verticalFOV / numberOfLasers;
        for (int i = 0; i < numberOfLasers; i++) {
            lasers.Add(new Laser(gameObject, completeAngle, rayDistance));
            completeAngle -= angle;
        }
        startTime = Time.time;
    }

    // Update is called once per frame
    private void Update () {
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

            // Execute lasers.
            stopWatch.Stop();
            List<SphericalCoordinates> hits = new List<SphericalCoordinates>();
            foreach (Laser laser in lasers)
            {
                hits.Add(convertHit(laser.ShootRay));
                //hits.Add(laser.ShootRay());
            }

            hitList[(Time.time - startTime)] = hits;
            
        }
    }

    /**
     * Converts the hit to spherical Coordinates.  
     * */ 
    private SpericalCoordinates ConvertHit(RaycastHit hit)
    {
        return new SpericalCoordinates(hit.distance,hit.vertical);
    }

    /**
     * A class representing spherical coordinates.  
     */
      
    private class SpericalCoordinates
    {
        private float radius;
        private float inclination;
        private float azimuth;

        public SpericalCoordinates(float radius, float inclination, float azimuth)
        {
            this.radius = radius;
            this.inclination = inclination;
            this.azimuth = azimuth;
        }

        // Constructor based on cartesian coordinates
        public SpericalCoordinates(Vector3 coordinates)
        {
            this.radius = Mathf.Sqrt(Mathf.Pow(coordinates.x, 2) + Mathf.Pow(coordinates.y, 2) + Mathf.Pow(coordinates.z, 2));

            if (radius == 0)
            {
                inclination = 0;
                azimuth = 0;
            }
            this.inclination = Mathf.Atan(coordinates.z / radius);
            this.azimuth = Mathf.Atan(coordinates.y / coordinates.x);



        }

        private float getRadius()
        {
            return this.radius;
        }
        private float getInclination()
        {
            return this.radius;
        }
        private float getAzimuth()
        {
            return this.azimuth;
        }

    }

}
