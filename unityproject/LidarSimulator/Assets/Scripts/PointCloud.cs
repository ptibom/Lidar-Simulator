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
            SetParticles(points);
            if(particleCloud.Length != 0)
            {
               ((ParticleSystem)(particleSystem)).SetParticles(particleCloud, particleCloud.Length);
                //pointsUpdate = false;

            }
        }
    }


    private void SetParticles(List<SphericalCoordinates> positions)
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
    }

    public void UpdatePoints(List<SphericalCoordinates> points)
    {
        this.points = points;
        pointsUpdate = true;
    }
}
