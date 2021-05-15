using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LayerMask laserMask;
    public ParticleSystem particles;
    public Transform particleHandle;
    public BoxCollider box;

    private LineRenderer line;
    private ParticleSystem.ShapeModule shape;
    private ParticleSystem.EmissionModule emit;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        shape = particles.shape;
        emit = particles.emission;
    }

    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, transform.position);
        if(Physics.Raycast(transform.position, transform.up * -1, out hit, 20, laserMask))
        {
            line.SetPosition(1, hit.point);

            Vector3 scale = new Vector3(0.1f, hit.distance, 0.1f);
            shape.scale = scale;
            box.size = scale;

            emit.rateOverTime = 10 * hit.distance;

            particleHandle.position = Vector3.Lerp(transform.position, hit.point, 0.5f);
        }
        else
        {
            Vector3 end = transform.position + Vector3.up * -20;
            line.SetPosition(1, end);

            Vector3 scale = new Vector3(0.1f, 20, 0.1f);
            shape.scale = scale;
            box.size = scale;

            emit.rateOverTime = 200;

            particleHandle.position = Vector3.Lerp(transform.position, end, 0.5f);
        }
    }
}
