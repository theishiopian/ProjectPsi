using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[ExecuteAlways]
[RequireComponent(typeof(ParticleSystem))]
public class ParticleOrbitalField : MonoBehaviour
{
    public ParticleSystem system;
    public float strength = 1;

    Particle[] particles;

    private void Start()
    {
        particles = new Particle[system.main.maxParticles];
    }

    private void Update()
    {
        if(particles == null) particles = new Particle[system.main.maxParticles];
        int alive = system.GetParticles(particles, system.main.maxParticles);

        for(int i = 0; i < alive; i++)
        {
            Vector3 normal = particles[i].position.normalized;
            if (particles[i].velocity.sqrMagnitude == 0)
            {
                Vector3 velocity = CreateRandomTangentVector(normal) * 5;
                particles[i].velocity = velocity;
            }
            else
            {
                particles[i].velocity += -normal * strength;
            }
        }

        system.SetParticles(particles, alive);
    }

    private Vector3 CreateRandomTangentVector(Vector3 normal)
    {
        //Creates a vector tangental to the one passed to this function.  Note that the angle of the tangent is random.
        float x = UnityEngine.Random.Range(-100, 100);
        float y = UnityEngine.Random.Range(-100, 100);
        float z = UnityEngine.Random.Range(-100, 100);
        Vector3 randomVector = new Vector3(x, y, z);
        randomVector = randomVector / randomVector.magnitude;
        Vector3 t1 = Vector3.Cross(normal, randomVector);
        Vector3 t2 = Vector3.Cross(normal, Vector3.up);
        Vector3 tangent; //= new Vector3(0, 0, 0);
        if (t1.magnitude > t2.magnitude)
        {
            tangent = t1;
        }
        else
        {
            tangent = t2;
        }
        return tangent;
    }
}