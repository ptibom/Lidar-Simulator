using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExternalVisualization : MonoBehaviour {
	private Dictionary<float, LinkedList<SphericalCoordinates>> pointTable;
	public GameObject pSystemGameObject;
	private ParticleSystem pSystem;
	private int currentListPosition; 
	public Button nextButton,prevButton,openButton;
    public Text lapText, mainText, posOpen;

	public void Start() {
		pSystemGameObject  = GameObject.Find("particlesSyst");
		pSystem = pSystemGameObject.GetComponent<ParticleSystem>();
		currentListPosition = 0;
		nextButton = GameObject.Find("Next").GetComponent<Button>();
		prevButton = GameObject.Find("Prev").GetComponent<Button>();
        openButton = GameObject.Find("Open").GetComponent<Button>();
        lapText = GameObject.Find("LapText").GetComponent<Text>();
        mainText = GameObject.Find("MainText").GetComponent<Text>();
        posOpen = GameObject.Find("PosOpen").GetComponent<Text>();


        nextButton.onClick.AddListener(LoadNext);
		prevButton.onClick.AddListener(LoadPrev);
		openButton.onClick.AddListener(LoadPoints);
        




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
        openButton.transform.SetPositionAndRotation(posOpen.transform.position,Quaternion.identity);
        mainText.enabled = false;

	}

	/// <summary>
	/// Tells the particle system to load the next set of points. 
	/// </summary>
	public void LoadNext()
	{
        if (pointTable != null) {
            if (currentListPosition + 1 < pointTable.Count) {
                currentListPosition += 1;
                ParticleSystem.Particle[] particles = CreateParticles(pointTable[currentListPosition]);
                pSystem.SetParticles(particles, particles.Length);
                lapText.text = "Lap: " + currentListPosition;
            }
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
                ParticleSystem.Particle[] particles = CreateParticles(pointTable[currentListPosition]);
                pSystem.SetParticles(particles, particles.Length);
                lapText.text = "Lap: " + currentListPosition;

            }
        }
    }


	private ParticleSystem.Particle[] CreateParticles(LinkedList<SphericalCoordinates> positions)
	{
		List<ParticleSystem.Particle> particleCloud = new List<ParticleSystem.Particle>();

		for (LinkedListNode<SphericalCoordinates> it = positions.First; it != null; it = it.Next)
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
			particle.startLifetime = 0.2f;
			particle.remainingLifetime = 1f;
			particleCloud.Add(particle);
		}

		return particleCloud.ToArray();
	}









	}
