using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class DataStructure : MonoBehaviour {

	private Dictionary<float, List<SphericalCoordinates>> data;
	private List<SphericalCoordinates> currentHits;
	private float prevTime; // Timestamp for previous data entry.


	public DataStructure()
	{
		this.data = new Dictionary<float, List<SphericalCoordinates>>();
		this.currentHits = new List<SphericalCoordinates>();
	}

	/// <summary>
	/// Adds a coorninate to the current hits list. 
	/// </summary>
	public void AddHit(SphericalCoordinates hit)
	{
		currentHits.Add(hit);

	}

	/// <summary>
	/// Updates the points of the data structure
	/// </summary>
	/// <param name="newTime"></param>
	public void UpdatePoints(float newTime)
	{

		List<SphericalCoordinates> prevList;
		data.TryGetValue(prevTime, out prevList);

		if (prevList == null && currentHits.Count != 0)
		{
			data[Time.fixedTime] = currentHits;
			prevTime = newTime;
			currentHits = new List<SphericalCoordinates>();
		}
		else
		{
            new Thread(delegate ()
            {
                Merge(newTime, currentHits, prevList, data);
            }).Start();
            prevTime = newTime;
		}
	}

    /// <summary>
    /// Function for merging two lists and insert them into a data structure.
    /// </summary>
    /// <param name="newTime"></param>
    /// <param name="newCoordinates"></param>
    /// <param name="previousCoordinates"></param>
    /// <param name="dataStructure"></param>
    public static void Merge(float newTime, List<SphericalCoordinates> newCoordinates, List<SphericalCoordinates> previousCoordinates, Dictionary<float,List<SphericalCoordinates>> dataStructure) 
    {
        List<SphericalCoordinates> diffList = new List<SphericalCoordinates>();
        List<SphericalCoordinates> mergedList = previousCoordinates.Union(newCoordinates).ToList();
        dataStructure[newTime] = mergedList;
    }

	/// <summary>
	/// Returns the last set of coordinates gathered. 
	/// </summary>
	/// <returns></returns>
	public List<SphericalCoordinates> GetLatestHits()
	{
		List<SphericalCoordinates> list;

		data.TryGetValue(prevTime, out list);

		return list;
	}


}
