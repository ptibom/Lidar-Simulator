using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AssemblyCSharp
{
	/**
	 * A class for parsing and serializing the lidar data into a JSON stream.
	 * */
	public class JSONParser
	{
		
		OpenFileDialog openDialog;
        


		public JSONParser ()
		{
			this.openDialog = new OpenFileDialog ();
		}

        /**
		 * Serializes lidar data.
		 * */
        public String serializeLidarData(Dictionary<float, List<SphericalCoordinates>> hitList)
        {
            return JsonConvert.SerializeObject(hitList);
        }

        /**
		 * Parses lidar data.
		 * */
        public Dictionary<float, List<SphericalCoordinates>> parseLidarData(String data)
        {
            return JsonConvert.DeserializeObject<Dictionary<float, List<SphericalCoordinates>>>(data);
        }
    
        /**
         * Saves the generated data into a file.
         * */
        public Boolean saveLidarData(Dictionary<float, List<SphericalCoordinates>> data)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "lidar.lidardata";
            save.Filter = "Lidar File | *.lidardata";

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.WriteLine(serializeLidarData(data));
                writer.Dispose();
                writer.Close();
                return true;
            } else
            {
                return false;
            }

        }

        /**
         * Opens a file and returns the data
         * */
        public Dictionary<float, List<SphericalCoordinates>> openData()
        {
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(open.FileName);
                Dictionary<float, List<SphericalCoordinates>>  hitList = parseLidarData(sr.ReadToEnd());
                sr.Close();
                return hitList;
            } else
            {
                throw new FileLoadException();
            }

        }


    }
}

