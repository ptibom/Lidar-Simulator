using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

    public GameObject pointCloudBase;
    public GameObject Lidar;

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
        if (hits != null)
        {
            pointCloud.UpdatePoints((hits));

        }
        else
        {
            Debug.Log("Is null");
        }
    }
}
