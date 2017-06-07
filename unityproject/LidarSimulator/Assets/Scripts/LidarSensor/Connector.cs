using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Connector class for connecting the point cloud to the lidar sensor, should be a component in the sensor.
///  @author: Tobias Alldén
/// </summary>
public class Connector : MonoBehaviour {

    public GameObject pointCloudBaseGameObject;
    public GameObject lidarGameObject;
    private PointCloud pointCloud;
    private LidarSensor lidarSensor;


	// Use this for initialization
	void Start () {
        pointCloudBaseGameObject = GameObject.FindGameObjectWithTag("PointCloudBase");
        pointCloud = pointCloudBaseGameObject.GetComponent<PointCloud>();
        lidarGameObject = GameObject.FindGameObjectWithTag("Lidar");
        lidarSensor = lidarGameObject.GetComponent<LidarSensor>();
	}
	
	/// <summary>
    /// Update method runs every iteration, tells the point cloud to update it's points.
    /// </summary>
	void Update () {
    }
}
