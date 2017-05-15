using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour {
	public Transform target;
	public float minDist;
	bool frozen = false;

	public static List<AgentTest> agents = new List<AgentTest> ();

	UnityEngine.AI.NavMeshAgent agent;

	public Animator[] anime;

	void Awake() {
		agents.Add (this);
		PlayButton.OnPlayToggled += StartOrFreeze;
	}

    void OnDestroy()
    {
        PlayButton.OnPlayToggled -= StartOrFreeze;
    }

	public static void StartOrFreeze(bool b){
		if (b == true) {
			ResumeAll ();
		} 
		else if (b == false) {
			FreezeAll ();
		}
	}

	public static void ResumeAll(){
		agents.RemoveAll(item => item == null);
		foreach (AgentTest AT in agents) {
			AT.Resume ();
		}
	}

	public static void FreezeAll (){
		agents.RemoveAll(item => item == null);
		foreach(AgentTest AT in agents) {
			AT.Freeze ();
		}
	}

	// Use this for initialization
	void Start() {
		agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent> ();
		anime = GetComponentsInChildren<Animator> ();
		Freeze ();
	}

	public void Freeze() {
		agent.SetDestination (transform.position);
		if (anime != null) {
			foreach(Animator a in anime){
				a.enabled = false;
			}
		}
		frozen = true;
	}

	public void Resume() {
		agent.SetDestination (target.position);
		foreach(Animator a in anime){
			a.enabled = true;
		}
		frozen = false;
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
		if (frozen) {
			agent.SetDestination (transform.position);
		}
	}
}
