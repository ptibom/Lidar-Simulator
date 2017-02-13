using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    private bool isHit;

    // Use this for initialization
    private void Start () {
        Vector3 direction = transform.TransformDirection(Vector3.back);
		ray = new Ray(transform.position, direction);
    }
	
	// Update is called once per frame
	private void Update () {
        float distance = 100f;
        if (isHit)
        {
            distance = hit.distance;
        }
        // For debugging, shows visible ray in real time.
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 0.2f);
    }

    private void FixedUpdate()
    {
        ray.direction = transform.TransformDirection(Vector3.back);
        ray.origin = transform.position;
        isHit = Physics.Raycast(ray, out hit, 100.0f);
        if (isHit)
        {
            print("Found object at distance: " + hit.distance);
        }
        else
        {
            print("No hit");
        }
    }
}
