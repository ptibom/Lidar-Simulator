﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour {
    private float lastUpdate = 0;

    private List<Laser> lasers = new List<Laser>();
    private List<RaycastHit> hits = new List<RaycastHit>();

    public int numberOfLasers = 2;
    public float rotationSpeedHz = 1.0f;
    public float rotationAnglePerStep = 45.0f;
    public float rayDistance = 100f;
    public float simulationSpeed = 1;
    public float verticalFOV = 30f;


    // Use this for initialization
    private void Start () {
        Time.timeScale = simulationSpeed; // For now, only be set before start in editor.
        Time.fixedDeltaTime = 0.002f; // Necessary for simulation to be detailed. Default is 0.02f.

        // Initialize number of lasers, based on user selection.
        float completeAngle = verticalFOV/2;
        float angle = verticalFOV / numberOfLasers;
        for (int i = 0; i < numberOfLasers; i++) {
            lasers.Add(new Laser(gameObject, completeAngle, rayDistance));
            completeAngle -= angle;
        }
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
            foreach (Laser laser in lasers)
            {
                hits.Add(laser.ShootRay());
            }
        }
    }
}
