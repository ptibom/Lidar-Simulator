using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AssemblyCSharp
{
    /**
	 * A class for parsing and serializing the lidar data into a JSON stream.
	 * */
    public class ExportLoadData
    {


        public ExportLoadData()
        {
        }

        // Serializes the collected lidar data.		 
        public String SerializeLidarData(Dictionary<float, List<SphericalCoordinates>> hitList)
        {
            return JsonUtility.ToJson(hitList);
        }


        // Parses lidar data.		 
        public Dictionary<float, List<SphericalCoordinates>> ParseLidarData(String data)
        {
            return JsonUtility.FromJson<Dictionary<float, List<SphericalCoordinates>>>(data);
        }


        // TODO: Saves the generated data into a file.        
        public Boolean SaveLidarData(Dictionary<float, List<SphericalCoordinates>> data)
        {

        }
        // TODO: Opens a file and, parses and returns the data         
        public Dictionary<float, List<SphericalCoordinates>> OpenData()
        {

        }
    }
}

