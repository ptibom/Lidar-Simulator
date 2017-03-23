using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// The data structure in which to save the lidar data.
///  @author: Tobias Alldén
/// </summary>
public class DataStructure {

	private Dictionary<float, LinkedList<SphericalCoordinates>> data;
	private LinkedList<SphericalCoordinates> currentHits;
	private float prevTime; // Timestamp for previous data entry.


	public DataStructure()
	{
		this.data = new Dictionary<float, LinkedList<SphericalCoordinates>>();
		this.currentHits = new LinkedList<SphericalCoordinates>();
	}

	/// <summary>
	/// Adds a coorninate to the current hits LinkedList. 
	/// </summary>
	public void AddHit(SphericalCoordinates hit)
	{
		currentHits.AddLast(hit);

	}

	/// <summary>
	/// Updates the points of the data structure
	/// </summary>
	/// <param name="newTime"></param>
	public void UpdatePoints(float newTime)
	{

		LinkedList<SphericalCoordinates> prevLinkedList;
		data.TryGetValue(prevTime, out prevLinkedList);

		if (prevLinkedList == null && currentHits.Count != 0)
		{
			data[Time.fixedTime] = currentHits;
			prevTime = newTime;
			currentHits = new LinkedList<SphericalCoordinates>();
		}
		else
		{
            new Thread(delegate ()
            {
                Merge(newTime, currentHits, prevLinkedList, data);
            }).Start();
            prevTime = newTime;
		}
	}

    /// <summary>
    /// Function for merging two LinkedLists and insert them into a data structure.
    /// </summary>
    /// <param name="newTime"></param>
    /// <param name="newCoordinates"></param>
    /// <param name="previousCoordinates"></param>
    /// <param name="dataStructure"></param>
    public static void Merge(float newTime, LinkedList<SphericalCoordinates> newCoordinates, LinkedList<SphericalCoordinates> previousCoordinates, Dictionary<float,LinkedList<SphericalCoordinates>> dataStructure) 
    {
        LinkedList<SphericalCoordinates> diffList = new LinkedList<SphericalCoordinates>();
        foreach(SphericalCoordinates sc in newCoordinates)
        {
            if(!previousCoordinates.Contains(sc))
            {
                diffList.AddLast(sc);
            }
        }
        LinkedList<SphericalCoordinates> newList = new LinkedList<SphericalCoordinates>();
        foreach(SphericalCoordinates sc in previousCoordinates)
        {
            if(newCoordinates.Contains(sc))
            {
                newList.AddLast(sc);
            }
        }
        foreach(SphericalCoordinates sc in diffList)
        {
            newList.AddLast(sc);
        }

        dataStructure[newTime] = newList;
    }

	/// <summary>
	/// Returns the last set of coordinates gathered. 
	/// </summary>
	/// <returns></returns>
	public LinkedList<SphericalCoordinates> GetLatestHits()
	{
		LinkedList<SphericalCoordinates> LinkedList;

		data.TryGetValue(prevTime, out LinkedList);

		return LinkedList;
	}


}
