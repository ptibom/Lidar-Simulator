using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorScript : MonoBehaviour {

	private int castLength = 40;
	//private LineRenderer laserLine;
	private Camera cam;
	private HashSet<RaycastHit> pointList;

	void Start () {
		//laserLine = GetComponent<LineRenderer> ();
		cam = GetComponent<Camera> ();
		pointList = new HashSet<RaycastHit> ();
	}
		
	public void listMethod()
	{
		Vector3 rayOrigin = cam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		if(Physics.Raycast (rayOrigin, cam.transform.forward, out hit, castLength))
		{
			pointList.Add (hit);
		}
	}

	public int getListSize()
	{
		return pointList.Count;
	}

	public HashSet<RaycastHit> clearPointSet()
	{
		HashSet<RaycastHit> tmp = new HashSet<RaycastHit> ();
		tmp.UnionWith (pointList);
		pointList.Clear ();
		return tmp;
	}

}
