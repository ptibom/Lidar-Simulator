/*
* MIT License
* 
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius, 
* Tobias Alldén, Martin Chemander, Sherry Davar
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the external visualization
/// </summary>
public class ExternalVisualization : MonoBehaviour {
	
	public GameObject pSystemGameObject, nextBtn, prevBtn, mainPanel,backBtn, loadingPanel, loadWheel;
	public Button nextButton,prevButton,openButton,backButton;
    public Text lapText;
    public Toggle fullCloudToggle, lapToggle;
    public TestFileBrowser fileBrowser;

	private int currentListPosition; 
    private Coroutine loadingPoints;
    private Dictionary<float, List<LinkedList<SphericalCoordinate>>> pointTable;
    private LidarStorage lidarStorage;
    private ExternalPointCloud externalPointCloud;
    private ParticleSystem pSystem;



    public void Start() {
		pSystemGameObject  = GameObject.Find("particlesSyst");
        backBtn = GameObject.Find("BackButton");
        mainPanel = GameObject.Find("MainPanel");
        pSystem = pSystemGameObject.GetComponent<ParticleSystem>();
        backButton = backBtn.GetComponent<Button>();
        openButton = GameObject.Find("Open").GetComponent<Button>();
        fileBrowser = GameObject.Find("FileBrowser").GetComponent<TestFileBrowser>();
        lidarStorage = GameObject.FindGameObjectWithTag("Lidar").GetComponent<LidarStorage>(); ;
        externalPointCloud = GetComponent<ExternalPointCloud>();


        openButton.onClick.AddListener(LoadPoints);
        SetState(State.Default);

        ExportManager.Loading += Loading;
        LidarStorage.HaveData += DataExists;
	}



    /// <summary>
    /// The different states the external visualization can be in.
    /// </summary>
    private enum State
    {
        Default, FullCloud, LapCloud
    }

    /// <summary>
    /// Sets the state of the visualization.
    /// </summary>
    private void SetState(State state)
    {
        if(state == State.Default)
        {
            mainPanel.SetActive(true);
            backBtn.SetActive(false);
        } else if(state == State.FullCloud)
        {
            mainPanel.SetActive(false);
            backBtn.SetActive(true);
            backButton.onClick.AddListener(Reset);
        }
        else
        {
            mainPanel.SetActive(false);
            backBtn.SetActive(true);
        }
    }


	/// <summary>
	/// Opens the file dialog and loads a set of previously loaded points. 
	/// </summary>
	public void LoadPoints()
	{
        fileBrowser.ToggleFileBrowser();
    }
    private LinkedList<SphericalCoordinate> createList(Dictionary<float, List<LinkedList<SphericalCoordinate>>> data)
    {
        LinkedList<SphericalCoordinate> newList = new LinkedList<SphericalCoordinate>();
        foreach (var list in data.Values)
        {
            foreach (var entity in list)
            {
                for (LinkedListNode<SphericalCoordinate> it = entity.First; it != null; it = it.Next)
                {
                    newList.AddLast(it.Value);
                }
            }
        }
        return newList;
    }

    /// <summary>
    /// Creates a single linked list filled with spherical coordinates from a data tablöe
    /// </summary>
    /// <returns></returns>
    private LinkedList<SphericalCoordinate> SquashTable(Dictionary<float, List<LinkedList<SphericalCoordinate>>> data)
    {
        Debug.Log("Squashing table for: " + data.Count);
        LinkedList<SphericalCoordinate> newList = new LinkedList<SphericalCoordinate>();

        foreach (var list in data.Values)
        {
            foreach (var entity in list)
            {
                foreach (SphericalCoordinate s in entity)
                {
                    newList.AddLast(s);
                }
            }
        }
        return newList;
    }


    /// <summary>
    /// Resets the visualization to it's initial state
    /// </summary>
    private void Reset()
    {
        externalPointCloud.Clear();
        SetState(State.Default);
    }

    /// <summary>
    /// Creates a a set of particles from the data.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    /// <returns></returns>
	private ParticleSystem.Particle[] CreateParticles(Dictionary<float,List<LinkedList<SphericalCoordinate>>> data, int position)
	{
		List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();
        LinkedList<SphericalCoordinate> list = new LinkedList<SphericalCoordinate>();
        int pos = 0;
        foreach (var coorlist in data)
        {
            foreach (var v in coorlist.Value)
            {
                if (pos == position)
                {
                    list = v;
                    break;
                }
                pos++;
            }
        }

		for (LinkedListNode<SphericalCoordinate> it = list.First; it != null; it = it.Next)
		{           
                ParticleSystem.Particle particle = new ParticleSystem.Particle();
                particle.position = it.Value.ToCartesian();
                if (it.Value.GetInclination() < 3)
                {
                    particle.startColor = Color.red;
                }
                else if (it.Value.GetInclination() > 3 && it.Value.GetInclination() < 7)
                {
                    particle.startColor = Color.yellow;
                }
                else
                {
                    particle.startColor = Color.green;
                }

                particle.startSize = 0.1f;
                particle.startLifetime =100f;
                particle.remainingLifetime = 100f;
                particleCloud.Add(particle);            
		}

		return particleCloud.ToArray();
	}

    /// <summary>
    /// Shows the loading dialog
    /// </summary>
    private void Loading(Coroutine async)
    {
        this.loadingPoints = async;

    }


    /// <summary>
    /// Is called when the data structure have data, in order to load the data to the visualization when it have been loaded in the data structure
    /// </summary>
    private void DataExists()
    {
        loadingPoints = null;
        this.pointTable = lidarStorage.GetData();        
        SetState(State.FullCloud);
        //Set Camera 
        foreach (var v in pointTable.Values)
        {
            foreach (var c in v)
            {
                Vector3 firstCoordinate = v[0].First.Value.GetWorldCoordinate();
                GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = new Vector3(firstCoordinate.x - 5, 10, firstCoordinate.y);
                break;
            }

        }
            externalPointCloud.CreateCloud(SquashTable(pointTable));               

    }





	}
