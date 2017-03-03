using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloud : MonoBehaviour
{
    ParticleSystem.Particle[] particleCloud;
    public Component particleSystem;
    List<SphericalCoordinates> points;
    bool pointsUpdate = false;

    // Use this for initialization
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pointsUpdate)
        {
            Debug.Log("Update");
            SetParticles(points);
            ((ParticleSystem)(particleSystem)).SetParticles(particleCloud, particleCloud.Length);
        }
    }


    private void SetParticles(List<SphericalCoordinates> positions)
    {
        Debug.Log("position: " + positions.Count);
        particleCloud = new ParticleSystem.Particle[positions.Count];
        Debug.Log("Cloud length: " + positions.Count);

        for (int i = 0; i < positions.Count; i++)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = positions[i].ToCartesian();
            particle.startColor = Color.green;
            particle.startSize = 0.1f;
            particleCloud[i] = particle;

        }
    }

    public void UpdatePoints(List<SphericalCoordinates> points)
    {
        this.points = points;
        pointsUpdate = true;
    }
}
