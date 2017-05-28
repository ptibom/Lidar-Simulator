using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExternalVisualization : MonoBehaviour {
	private Dictionary<float, List<LinkedList<SphericalCoordinate>>> pointTable;
    private LidarStorage lidarStorage;
    private ExternalPointCloud externalPointCloud;
	public GameObject pSystemGameObject, nextBtn, prevBtn, mainPanel,backBtn, loadingPanel, loadWheel;
	private ParticleSystem pSystem;
	private int currentListPosition; 
	public Button nextButton,prevButton,openButton,backButton;
    public Text lapText;
    public Toggle fullCloudToggle, lapToggle;
    public TestFileBrowser fileBrowser;

    private Coroutine loadingPoints;

	public void Start() {
		pSystemGameObject  = GameObject.Find("particlesSyst");
        //nextBtn = GameObject.Find("Next");
        //prevBtn = GameObject.Find("Prev");
        backBtn = GameObject.Find("BackButton");
        mainPanel = GameObject.Find("MainPanel");

        pSystem = pSystemGameObject.GetComponent<ParticleSystem>();
        //nextButton = nextBtn.GetComponent<Button>();
        //prevButton = prevBtn.GetComponent<Button>();
        backButton = backBtn.GetComponent<Button>();
        openButton = GameObject.Find("Open").GetComponent<Button>();
        //lapText = GameObject.Find("LapText").GetComponent<Text>();
        fileBrowser = GameObject.Find("FileBrowser").GetComponent<TestFileBrowser>();
        lidarStorage = GameObject.FindGameObjectWithTag("Lidar").GetComponent<LidarStorage>(); ;
        externalPointCloud = GetComponent<ExternalPointCloud>();
        //loadingPanel = GameObject.Find("LoadingPanel");
        //loadWheel = GameObject.Find("LoadImage");


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
            //currentListPosition = 0;
            //prevBtn.SetActive(false);
            //nextBtn.SetActive(false);
            mainPanel.SetActive(true);
            //lapText.enabled = false;
            backBtn.SetActive(false);
        } else if(state == State.FullCloud)
        {
            //prevBtn.SetActive(false);
            //nextBtn.SetActive(false);
            //lapText.enabled = false;
            mainPanel.SetActive(false);
            backBtn.SetActive(true);
            backButton.onClick.AddListener(Reset);

        }
        else
        {
            currentListPosition = 0;
            //prevBtn.SetActive(true);
            //nextBtn.SetActive(true);
            mainPanel.SetActive(false);
            //lapText.enabled = true;
            backBtn.SetActive(true);
            //nextButton.onClick.AddListener(LoadNext);
            //prevButton.onClick.AddListener(LoadPrev);
            //backButton.onClick.AddListener(Reset);
        }
    }
   

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadNext();
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            LoadPrev();
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
	/// Tells the particle system to load the next set of points. 
	/// </summary>
	public void LoadNext()
	{
        if (pointTable != null && pointTable.Count != 0) {
            if (currentListPosition + 1 < pointTable.Count) {
                currentListPosition += 1;
                ParticleSystem.Particle[] particles = CreateParticles(pointTable, currentListPosition);
                pSystem.SetParticles(particles, particles.Length);
                lapText.text = "Lap: " + currentListPosition;
            }
        } else
        {
            pointTable = lidarStorage.GetData();
        }
    }
	/// <summary>
	/// Tells the particle system to load the previous set of points. 
	/// </summary>
	public void LoadPrev()
	{
        if (pointTable != null)
        {
            if (currentListPosition - 1 >= 0)
            {
                currentListPosition -= 1;
                
                ParticleSystem.Particle[] particles = CreateParticles(pointTable, currentListPosition);
                pSystem.Clear();
                pSystem.SetParticles(particles, particles.Length);
                pSystem.Play();
                lapText.text = "Lap: " + currentListPosition;

            }
        } else
        {
            this.pointTable = lidarStorage.GetData();
        }
    }

    /// <summary>
    /// Resets the visualization to it's initial state
    /// </summary>
    private void Reset()
    {
        externalPointCloud.Clear();
        SetState(State.Default);
    }


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
