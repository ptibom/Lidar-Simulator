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

    void CreateCloud(LinkedList<SphericalCoordinate> coordinates)
    {
        List<List<Vector3>> chunks = SplitIntoChunks(coordinates);
        List<string> meshObjects = CreateNeededMeshes(chunks.Count);

        for(int i = 0; i< chunks.Count; i++)
        {
            MeshFilter meshFilter = GameObject.Find(meshObjects[i]).GetComponent<MeshFilter>();
            Mesh m = new Mesh();
            m.SetVertices(chunks[i]);
            m.SetColors(CreateColorsForPointList(chunks[i]));
            m.SetIndices(CreateIdsForCoordinates(chunks[i]), MeshTopology.Points,0);
            meshFilter.mesh = m;
        }
    }

    /// <summary>
    /// Splits a list of spherical coordinates into chunks of length 64000, the max number of particles per mesh.
    /// </summary>
    /// <param name="coordinates">The coordinates to be split</param>
    /// <returns>A list containing the chunks of points</returns>
    private List<List<Vector3>> SplitIntoChunks(LinkedList<SphericalCoordinate> coordinates)
    {
        List<List<Vector3>> chunks = new List<List<Vector3>>();
        List<Vector3> currentChunk = new List<Vector3>();
        int i = 0;

        for(LinkedListNode<SphericalCoordinate> it = coordinates.First; it != null; it = it.Next)
        {
            currentChunk[i] = (it.Value.ToCartesian()); //Remake to use world points 
            if(currentChunk.Count%maxParticlesPerChunk == 0)
            {
                chunks.Add(currentChunk);
                currentChunk = new List<Vector3>();
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

    /// <summary>
    /// Creates a array of colors for a given list of spherical coordinates
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns>A list of colors, based on their height</returns>
    private List<Color> CreateColorsForPointList(List<Vector3> coordinates)
    {
        List<Color> colorList = new List<Color>();

        for (int i = 0; i<coordinates.Count; i++)
        {
            Color c;
            if (coordinates[i].y < 1)
            {
                c = Color.red;
            }
            else if (coordinates[i].y > 1 && coordinates[i].y < 3)
            {
                c = Color.yellow;
            }
            else
            {
                c = Color.green;
            }
            colorList[i] = c;
        }
        return colorList;
    }


    private int[] CreateIdsForCoordinates(List<Vector3> coordinates)
    {
        int[] idList = new int[coordinates.Count];
        for (int i = 0; i<coordinates.Count; i++)
        {
            idList[i] = i;
        }
        return idList;
    }


}

