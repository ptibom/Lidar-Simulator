/*
* MIT License
* 
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius, 
* Tobias Alldén, Martin Chemander, Sherry Davar
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

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
