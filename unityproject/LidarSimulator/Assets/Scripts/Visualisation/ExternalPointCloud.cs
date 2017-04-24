using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalPointCloud : MonoBehaviour {
    public GameObject meshObject;


    private List<Mesh> meshes;
    private int maxParticlesPerChunk = 65000;
    

    // Use this for initialization
    void Start()
    {
        meshes = new List<Mesh>();
        Mesh startMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = startMesh;
    }

    void CreateMesh(LinkedList<SphericalCoordinate> coordinates)
    {

        if (coordinates.Count > maxParticlesPerChunk) // Max particle size per mesh 
        {
            List<LinkedList<SphericalCoordinate>> chunks = SplitIntoChunks(coordinates);
        }
        /**
        if (worldPoints.Length > maxParticlesPerChunk)
        {
            //TODO: 
        } else
        {
            meshes[0].vertices = worldPoints;
            meshes[0].colors = colors;
            meshes[0].SetIndices(indices,MeshTopology.Points,0);
            
        }      
    **/

    }

    /// <summary>
    /// Splits a list of spherical coordinates into chunks of length 64000, the max number of particles per mesh.
    /// </summary>
    /// <param name="coordinates">The coordinates to be split</param>
    /// <returns>A list containing the chunks of points</returns>
    private List<LinkedList<SphericalCoordinate>> SplitIntoChunks(LinkedList<SphericalCoordinate> coordinates)
    {
        List<LinkedList<SphericalCoordinate>> chunks = new List<LinkedList<SphericalCoordinate>>();
        LinkedList<SphericalCoordinate> currentChunk = new LinkedList<SphericalCoordinate>();

        for(LinkedListNode<SphericalCoordinate> it = coordinates.First; it != null; it = it.Next)
        {
            currentChunk.AddLast(it.Value);
            if(currentChunk.Count%maxParticlesPerChunk == 0)
            {
                chunks.Add(currentChunk);
                currentChunk = new LinkedList<SphericalCoordinate>();
            } else if(it.Next == null)
            {
                chunks.Add(currentChunk); // final value
            }
        }
        return chunks;
    }


    /// <summary>
    /// Creates the given number of meshes(the meshprefab)
    /// </summary>
    /// <param name="count"></param>
    /// <returns>A list containing the names of the created prefabs</returns>
    private List<string> CreateNeededMeshes(int count)
    {
        List<string> nameList = new List<string>();
        for(int i = 0; i< count; i++)
        {
            GameObject newGO = Instantiate(meshObject, new Vector3(0,0,0), Quaternion.identity);
            newGO.name = "MeshObject" + i;
        }

        return nameList;
    }


}

