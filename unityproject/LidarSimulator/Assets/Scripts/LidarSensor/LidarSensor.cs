using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour {
    private float lastUpdate = 0;

    private List<Laser> lasers = new List<Laser>();
    private List<RaycastHit> hits = new List<RaycastHit>();
    private float horizontalAngle = 0;
   
	public int numberOfLasers = 2; //(Skrivet som Lasers per set i UI't)
	public float rotationSpeedHz = 1.0f; //(Kopplat till UI'n)
    public float rotationAnglePerStep = 5.0f; //(Kopplat till UI'n)
	public float rayDistance = 100f; //(Kopplat till UI'n)
    public float simulationSpeed = 1;
	public float verticalFOV = 30f; //(Funktion skriven, UI ej skapat)
	public float offset = 0.001f;	//(Funktion skriven, UI ej skapat)


    // Use this for initialization
    private void Start ()
    {
        Time.timeScale = simulationSpeed; // For now, only be set before start in editor.
        Time.fixedDeltaTime = 0.002f; // Necessary for simulation to be detailed. Default is 0.02f.

        // Initialize number of lasers, based on user selection.
        float completeAngle = verticalFOV/2;
        float angle = verticalFOV / numberOfLasers;
        int setOfLasers = numberOfLasers / 4;
        for (int i = 0; i < numberOfLasers; i++)
        {
            float rayOffset = offset;
            if (i < setOfLasers || i >= setOfLasers * 2 && i < setOfLasers * 3)
            {
                rayOffset = -offset;
            }
            lasers.Add(new Laser(gameObject, completeAngle, rayDistance, rayOffset));
            completeAngle -= angle;
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
                // Example use: new coordinate(distance, horizontalAngle, verticalAngle)
            }
        }
    }
}
