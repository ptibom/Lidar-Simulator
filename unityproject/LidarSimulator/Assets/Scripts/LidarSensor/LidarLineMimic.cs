using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarLineMimic : MonoBehaviour {
	
	public GameObject lineDrawerPrefab;

	public int numberOfLasers = 2;
	public float rotationSpeedHz = 1.0f;
	public float rotationAnglePerStep = 45.0f;
	public float rayDistance = 100f;
	public float upperFOV = 30f;
	public float lowerFOV = 30f;
	public float offset = 0.001f;
	public float upperNormal = 30f;
	public float lowerNormal = 30f;

	private int maxLasers = 64;

	private List<LaserMimic> lasersMimics = new List<LaserMimic>();

	// Use this for initialization
	public void InitializeLaserMimicList () {
		for (int i = 0; i < maxLasers; i++)
		{
			GameObject lineDrawer = Instantiate(lineDrawerPrefab);
			lineDrawer.transform.parent = gameObject.transform;
			lineDrawer.transform.position = transform.position;
			lineDrawer.transform.rotation = transform.rotation;
			lasersMimics.Add(new LaserMimic(0,0, lineDrawer, false));
		
		}
	}


	public void UpdateLines(){
		float upperTotalAngle = upperFOV / 2;
		float lowerTotalAngle = lowerFOV / 2;
		float upperAngle = upperFOV / (numberOfLasers / 2);
		float lowerAngle = lowerFOV / (numberOfLasers / 2);

		for (int i = 0; i < numberOfLasers; i++)
		{
			if (i < numberOfLasers/2)
			{
				lasersMimics[i].SetRayParameters(lowerTotalAngle + lowerNormal, -offset);
				lasersMimics [i].SetActive (true);
				lowerTotalAngle -= lowerAngle;
			}
			else
			{
				lasersMimics[i].SetRayParameters(upperTotalAngle - upperNormal, 0);
				lasersMimics [i].SetActive (true);
				upperTotalAngle -= upperAngle;
			}
		}

		for(int i = numberOfLasers; i < lasersMimics.Count; i++){
			lasersMimics [i].SetActive (false);
		}

		DrawRays ();
	}


	public void DrawRays(){
		foreach(LaserMimic lm in lasersMimics){
			lm.DrawRay ();
		}
	}

	public void DestroyLaserMimics(){
		foreach(LaserMimic lm in lasersMimics){
			Destroy (lm.lineDrawer);
		}
		lasersMimics.Clear ();
	}

	class LaserMimic {
		private float verticalAngle;
		private float offset;
		private RenderLine lineRenderer;
		private bool rayOn;
		private Transform originalTransform;
		private float rayDistance = 5f;

		public GameObject lineDrawer;

		public LaserMimic(float verticalAngle, float offset, GameObject lineDrawer, bool rayOn){
			this.verticalAngle = verticalAngle;
			this.offset = offset;
			this.lineDrawer = lineDrawer;
			lineRenderer = lineDrawer.GetComponent<RenderLine>();

			originalTransform = lineDrawer.transform;

			lineDrawer.transform.position = originalTransform.position + (originalTransform.up * offset);

			this.rayOn = rayOn;
		
		
			/*
			Quaternion q = Quaternion.AngleAxis(verticalAngle, Vector3.right);
			Vector3 direction = parentObject.transform.TransformDirection(q * Vector3.forward);
			ray.origin = parentObject.transform.position + (parentObject.transform.up * offset);
			ray.direction = direction;
			*/
		}

		public void SetRayParameters(float verticalAngle, float offset){
			this.verticalAngle = verticalAngle;
			this.offset = offset;
			lineDrawer.transform.position = originalTransform.position + (originalTransform.up * offset);

			//Debug.Log (new Vector3 (0, verticalAngle, 0));
			lineDrawer.transform.rotation = originalTransform.rotation;
			lineDrawer.transform.Rotate (new Vector3 (verticalAngle, 0, 0));

			//lineDrawer.transform.rotation = Quaternion.Euler(0,verticalAngle,0);
			//lineDrawer.transform.eulerAngles = Quaternion.Euler(0, verticalAngle, 0).eulerAngles;
			//Debug.Log (lineDrawer.transform.rotation.eulerAngles);

			//Quaternion q = Quaternion.AngleAxis(verticalAngle, Vector3.right);
			//Vector3 dir = lineDrawer.transform.TransformDirection(q * Vector3.forward);
			//Debug.Log (dir);
			//lineDrawer.transform.rotation = Quaternion.Euler(dir.x, dir.y, dir.z);



		}

		public void SetActive(bool state){
			rayOn = state;
		}

		public void DrawRay(){
			if (rayOn) {
				lineRenderer.DrawLine (lineDrawer.transform.forward * rayDistance + lineRenderer.transform.position);
			} else {
				lineRenderer.DrawLine (originalTransform.position);
			}
		}
	}
}
