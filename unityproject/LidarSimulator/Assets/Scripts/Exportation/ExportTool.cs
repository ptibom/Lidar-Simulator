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

        // Saves the generated data into a file.
        public static Boolean SaveLidarData(Dictionary<float, List<SphericalCoordinates>> data, String fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/"+fileName+".lidar");
			bf.Serialize(file, (JsonUtility.ToJson(data)));
            file.Close();
			return File.Exists (Application.persistentDataPath + "/" + fileName + ".lidar"); 
        }
        //  Opens a file and, parses and returns the data
		public static Dictionary<float, List<SphericalCoordinates>> OpenData(String fileName)
        {
			if (File.Exists(Application.persistentDataPath + "/" + fileName + ".lidar")) {
            BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + ".lidar", FileMode.Open);
            Dictionary<float,List<SphericalCoordinates>> data = (Dictionary<float,List<SphericalCoordinates>>) bf.Deserialize(file);
            file.Close();
            return data;

          } else {
            throw new FileNotFoundException();
          }

        }

		// Returns all the files that can be loaded
		public static List<String> GetFiles() 
		{
			Path path = new DirectoryInfo (Application.persistentDataPath);
			List<String> files = new List<string> ();
			foreach (string file in System.IO.Directory.GetFiles(path))
			{ 
				files.Add (file);
			}
			return files;
		}
    }
}
