using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			MergeLists(newTime,prevList);
		}
	}


	IEnumerator MergeLists(float newTime, List<SphericalCoordinates> prevList) {
		List<SphericalCoordinates> diffList = new List<SphericalCoordinates>();

		List<SphericalCoordinates> mergedList = prevList.Union (currentHits).ToList ();
		data[Time.fixedTime] = mergedList;
		yield return null;
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
