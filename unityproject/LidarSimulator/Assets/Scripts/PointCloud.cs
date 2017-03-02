using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloud : MonoBehaviour
{
    ParticleSystem.Particle[] particleCloud;
    public ParticleSystem particleSystem;
    public List<SphericalCoordinates> points;
    bool pointsUpdate = true;

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
            SetParticles(points);
            particleSystem.SetParticles(particleCloud, particleCloud.Length);
            pointsUpdate = false;
        }
    }


    public void SetParticles(List<SphericalCoordinates> positions)
    {
        particleCloud = new ParticleSystem.Particle[positions.Count];

        for (int i = 0; i < positions.Count; i++)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();
            particle.position = positions[i].ToCartesian();
            particle.startColor = Color.green;
            particle.startSize = 0.1f;
            particleCloud[i] = particle;

        }
        pointsUpdate = true;
    }
}
