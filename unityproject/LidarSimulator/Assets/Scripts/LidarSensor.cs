using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    private bool isHit;
    private float lastUpdate = 0;
    public float rotationSpeedHz = 1.0f;
    public float rotationAnglePerStep = 45.0f;
    public float rayDistance = 100f;
    public float simulationSpeed = 1;

    // Use this for initialization
    private void Start () {
        Time.timeScale = simulationSpeed; // For now, only be set before start in editor.
        Time.fixedDeltaTime = 0.002f;
        Vector3 direction = transform.TransformDirection(Vector3.back);
		ray = new Ray(transform.position, direction);
    }

    // Update is called once per frame
    private void Update () {
        // For debugging, shows visible ray in real time.
        float distance = rayDistance;
        if (isHit)
        {
            distance = hit.distance;
        }
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime - lastUpdate > 1/(360/rotationAnglePerStep)/rotationSpeedHz)
        {
            // Update the time
            lastUpdate = Time.fixedTime;

            // Perform rotation
            transform.Rotate(0, rotationAnglePerStep, 0);

            // Update the ray
            ray.direction = transform.TransformDirection(Vector3.back);
            ray.origin = transform.position;

            // Perform raycast
            isHit = Physics.Raycast(ray, out hit, rayDistance);
            if (isHit)
            {
                //print("Found object at distance: " + hit.distance);
            }
            else
            {
                //print("No hit");
            }
        }
    }
}
