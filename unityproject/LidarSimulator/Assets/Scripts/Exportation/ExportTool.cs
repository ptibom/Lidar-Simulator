using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace AssemblyCSharp
{
    /**
	 * A class for parsing and serializing the lidar data into a JSON stream.
	 * */
    public class ExportLoadData : MonoBehaviour
    {


        public ExportLoadData()
        {
        }

        // Serializes the collected lidar data.
        private String SerializeLidarData(Dictionary<float, List<SphericalCoordinates>> hitList)
        {
            return JsonUtility.ToJson(hitList);
        }


        // Parses lidar data.
        private Dictionary<float, List<SphericalCoordinates>> ParseLidarData(String data)
        {
            return JsonUtility.FromJson<Dictionary<float, List<SphericalCoordinates>>>(data);
        }


        // Saves the generated data into a file.
        public Boolean SaveLidarData(Dictionary<float, List<SphericalCoordinates>> data)
        {
			// Opens a file selection dialog for a PNG file and overwrites any
			// selected texture with the contents.


            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/lidarData.lidar");
            bf.Serialize(JsonUtility.ToJson(data));
            file.Close();
            return true;
        }
        //  Opens a file and, parses and returns the data
        public Dictionary<float, List<SphericalCoordinates>> OpenData()
        {
          if (File.Exists(Application.persistentDataPath + "/lidarData.lidar")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "lidarData.lidar", FileMode.Open);
            Dictionary<float,List<SphericalCoordinates>> data = (Dictionary<float,List<SphericalCoordinates>>) bf.Deserialize();
            file.Close();


            return data;
          } else {
            throw new FileNotFoundException();
          }

        }
    }
}
