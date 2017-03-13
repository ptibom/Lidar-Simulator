using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LidarSensor : MonoBehaviour {
    private float lastUpdate = 0;

    private List<Laser> lasers = new List<Laser>();
    private List<RaycastHit> hits = new List<RaycastHit>();
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
	public DataStructure dataStructure;
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



            // Execute lasers.
            foreach (Laser laser in lasers)
            {
                RaycastHit hit = laser.ShootRay();
                float distance = hit.distance;
                float verticalAngle = laser.GetVerticalAngle();

                dataStructure.AddHit(new SphericalCoordinates(hit.point));
                //points.Add(new SphericalCoordinates(distance, horizontalAngle, verticalAngle));
                // Example use: new coordinate(distance, horizontalAngle, verticalAngle)
            }

            if(Time.fixedTime - prev > 0.25) {
                dataStructure.UpdatePoints(Time.fixedTime);
                prev = Time.fixedTime;
            }
           
        }
    }

    public List<SphericalCoordinates> GetLastHits()
    {

        return dataStructure.GetLatestHits ();
    }
}
