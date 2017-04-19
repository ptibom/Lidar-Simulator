using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparenceFuckery : MonoBehaviour {
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;



    // Use this for initialization
    void Start () {
		mat1.SetInt("_ZWrite", 1);
        mat2.SetInt("_ZWrite", 1);
        mat3.SetInt("_ZWrite", 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
