using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalPointCloud : MonoBehaviour {
    public GameObject meshObject;
    private int maxParticlesPerChunk = 65000;
    

    // Use this for initialization
    void Start()
    {

    }

    public void CreateCloud(LinkedList<SphericalCoordinate> coordinates)
    {
        Debug.Log("Creating Chunks");
        List<List<Vector3>> chunks = SplitIntoChunks(coordinates);
        Debug.Log("Created: " + chunks.Count + " Chunks");

        Debug.Log("Creating needed Meshobjects");
        List<MeshFilter> meshFilterList = CreateNeededMeshes(chunks.Count);
        Debug.Log("Created: " + meshFilterList.Count + " Meshobjects");


        List<Mesh> meshes = new List<Mesh>();

        Debug.Log("Creating VerticeList");
        List<int[]> vertices = CreateVertices(chunks);
        Debug.Log("Created: " + vertices.Count + " VerticeLists");
         

        for(int i = 0; i< chunks.Count; i++)
        {
            MeshFilter meshFilter = meshFilterList[i];
            Mesh m = new Mesh();
            m.SetVertices(chunks[i]);
            m.SetColors(CreateColorsForPointList(chunks[i]));
            m.SetIndices(vertices[i], MeshTopology.Points,0);
            meshes.Add(m);
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
        List<Vector3> currentChunk = new List<Vector3>();
        List<List<Vector3>> chunks = new List<List<Vector3>>();

        for (LinkedListNode<SphericalCoordinate> it = coordinates.First; it != null; it = it.Next)
        {
            currentChunk.Add(it.Value.GetWorldCoordinate()); //Remake to use world points 

            if (currentChunk.Count%maxParticlesPerChunk == 0)
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
    private List<MeshFilter> CreateNeededMeshes(int count)
    {
    List<MeshFilter> meshFilterList = new List<MeshFilter>();
        for(int i = 0; i < count; i++)
        {
            GameObject newGO = Instantiate(meshObject, new Vector3(0,0,0), Quaternion.identity);
            newGO.transform.SetParent(GameObject.Find("PSystBase").transform);
            meshFilterList.Add(newGO.GetComponent<MeshFilter>());
        }
    return meshFilterList;
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
            if (coordinates[i].y < 0.5)
            {
                c = Color.red;
            }
            else if (coordinates[i].y > 0.5 && coordinates[i].y < 0.7)
            {
                c = Color.yellow;
            }
            else
            {
                c = Color.green;
            }
            colorList.Add(c);
        }
        return colorList;
    }


    private List<int[]> CreateVertices(List<List<Vector3>> chunks)
    {
        List<int[]> verticeList = new List<int[]>();
        int startIndex = 0;
        for(int j = 0; j<chunks.Count; j++) 
        {
            int[] currentList = new int[chunks[j].Count];
            for (int i = 0; i< chunks[j].Count ; i++)
            {
                currentList[i] = startIndex+i;
            }
            verticeList.Add(currentList);
        }
        
        return verticeList;
    }


}


