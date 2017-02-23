using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleUpdateScript : MonoBehaviour {


	private ParticleSystem pSystem;
	private ParticleSystem.Particle[] particles;
	private int maxParticles;

	// Use this for initialization
	void Start () {


		initializeSystem ();


	}

	// Update is called once per frame
	void Update () {

		/*int numParticlesAlive = pSystem.GetParticles (particles);


		for (int i = 0; i < numParticlesAlive; i++)
		{
			particles [i].position = new Vector3 (0, i, 0);
		}

		pSystem.SetParticles (particles, particles.Length);
		*/

	}



	void initializeSystem(){
		pSystem = GetComponent<ParticleSystem> ();
		maxParticles = pSystem.main.maxParticles;
		particles = new ParticleSystem.Particle[maxParticles];
		pSystem.Emit (maxParticles);

	}

	public void updateParticlePos(HashSet<RaycastHit> hitPoints){

		int i = 0;
		foreach(RaycastHit hit in hitPoints){
			if(i >= maxParticles){
				Debug.Log ("aloha");
				break;
				}

			particles [i].position = hit.point;
			i++;

		}


		/*for (int i = 0; i < numPoints; i++)
		{
			particles [i].position = hitPoints[i].point;
		}*/
		Debug.Log (i);
		pSystem.SetParticles (particles, particles.Length);

		Debug.Log ("noi");



	}
}
