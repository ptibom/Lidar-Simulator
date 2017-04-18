using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarLineMimic : MonoBehaviour {

	[SerializeField]
	private GameObject lineDrawerPrefab;

	public int numberOfLasers = 2;
	public float upperFOV = 30f;
	public float lowerFOV = 30f;
	public float offset = 0.001f;
	public float upperNormal = 30f;
	public float lowerNormal = 30f;

	private int maxLasers = 64;
	private List<LaserMimic> lasersMimics = new List<LaserMimic>();

	void Start(){
		LidarMenuScript.OnLidarMenuValChanged += UpdateLidarValues;
	}

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

	public void UpdateLidarValues(int numberOfLasers, float upperFOV, float lowerFOV, float offset, float upperNormal, float lowerNormal){
		this.numberOfLasers = numberOfLasers;
		this.upperFOV = upperFOV;
		this.lowerFOV = lowerFOV;
		this.offset = offset;
		this.upperNormal = upperNormal;
		this.lowerNormal = lowerNormal;

		UpdateLines ();
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
				lasersMimics[i].SetRayParameters(lowerTotalAngle + lowerNormal, -offset, transform);
				lasersMimics [i].SetActive (true);
				lowerTotalAngle -= lowerAngle;
			}
			else
			{
				lasersMimics[i].SetRayParameters(upperTotalAngle - upperNormal, 0, transform);
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
			Destroy (lm.GetLineDrawerGObject());
		}
		lasersMimics.Clear ();
	}

	class LaserMimic {
		private float verticalAngle;
		private float offset;
		private RenderLine lineRenderer;
		private bool rayOn;
		private float rayDistance = 5f;
		private GameObject lineDrawer;

		public LaserMimic(float verticalAngle, float offset, GameObject lineDrawer, bool rayOn){
			this.verticalAngle = verticalAngle;
			this.offset = offset;
			this.lineDrawer = lineDrawer;
			lineRenderer = lineDrawer.GetComponent<RenderLine>();
			lineDrawer.transform.position = lineDrawer.transform.position + (lineDrawer.transform.up * offset);

			this.rayOn = rayOn;
		}

		public void SetRayParameters(float verticalAngle, float offset, Transform baseTransform){
			this.verticalAngle = verticalAngle;
			this.offset = offset;
			lineDrawer.transform.position = baseTransform.position + (baseTransform.up * offset);
			lineDrawer.transform.rotation = baseTransform.rotation;
			lineDrawer.transform.Rotate (new Vector3 (verticalAngle, 0, 0));
		}

		public void SetActive(bool state){
			rayOn = state;
		}

		public void DrawRay(){
			if (rayOn) {
				lineRenderer.DrawLine (lineDrawer.transform.forward * rayDistance + lineRenderer.transform.position);
			} else {
				lineRenderer.DrawLine (lineDrawer.transform.position);
			}
		}


		public GameObject GetLineDrawerGObject()
		{
			return lineDrawer;
		}	
	}
}
