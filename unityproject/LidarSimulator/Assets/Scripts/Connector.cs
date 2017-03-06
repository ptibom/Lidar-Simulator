using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

    public GameObject pointCloudBase;
    public GameObject Lidar;
    private List<SphericalCoordinates> prev;

    private PointCloud pointCloud;
    private LidarSensor lidarSensor;


	// Use this for initialization
	void Start () {
        pointCloud = pointCloudBase.GetComponent<PointCloud>();
        lidarSensor = Lidar.GetComponent<LidarSensor>();
	}
	
	// Update is called once per frame
	void Update () {
        List<SphericalCoordinates> hits = lidarSensor.GetLastHits();
        if (hits != null && hits != prev)
        {
            pointCloud.UpdatePoints((hits));
            prev = hits;

        }
        else
        {
            Debug.Log("Is null");
        }
    }
}
