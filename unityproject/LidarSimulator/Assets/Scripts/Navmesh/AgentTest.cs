using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour {
	public Transform target;
	public float minDist;

	UnityEngine.AI.NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			if (target.GetComponent<WayPoint> () != null) {
				//Debug.Log ("Target is not null");
				if (Vector3.Distance (transform.position, target.transform.position) < minDist) {
					//Debug.Log ("Reached Target, switching");
					target = target.GetComponent<WayPoint> ().next.transform;
				}

				agent.SetDestination (target.position);
			}
		}
	}
}
