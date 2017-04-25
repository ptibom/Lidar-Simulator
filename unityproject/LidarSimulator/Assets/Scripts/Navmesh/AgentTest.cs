using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour {
	public Transform target;
	public float minDist;

	UnityEngine.AI.NavMeshAgent agent;

	public Animator anime;

	void Awake() {
		PlayButton.OnPlayToggled += StartOrFreeze;
	}

	public void StartOrFreeze(bool b){
		if (b == true) {
			Resume ();
		} 
		else if (b == false) {
			Freeze ();
		}
	}

	// Use this for initialization
	void Start() {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		anime = GetComponent<Animator> ();
		Freeze ();
	}

	public void Freeze() {
		agent.SetDestination (transform.position);
		if (anime != null) {
			anime.enabled = false;
		}
	}

	public void Resume() {
		agent.SetDestination (target);
		if (anime != null) {
			anime.enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (target != null) {
			if (target.GetComponent<WayPoint> () != null) {
				//Debug.Log ("Target is not null");
				if (Vector3.Distance(transform.position, target.transform.position) < minDist) {
                    //Debug.Log ("Reached Target, switching");
                    if (target.GetComponent<WayPoint>().next != null)
                    {
                        target = target.GetComponent<WayPoint>().next.transform;
                    }
				}
				agent.SetDestination(target.position);
			}
		}
	}
}
