using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ExternalPointCloud : MonoBehaviour {
    private List<Mesh> meshes;
    int numPoints = 60000;

    // Use this for initialization
    void Start()
    {
        meshes = new List<Mesh>();
        Mesh startMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = startMesh;
    }

    void CreateMesh(Vector3[] worldPoints, int[] indices, Color[] colors)
    {
        if(worldPoints.Length > 65000)
        {
            //TODO: 
        } else
        {
            meshes[0].vertices = worldPoints;
            meshes[0].colors = colors;
            meshes[0].SetIndices(indices,MeshTopology.Points,0);
            
        }      

    }

}

