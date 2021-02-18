using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisTarget : MonoBehaviour
{
    public static GameObject Target { get; private set; }

    [Header("Objects")]
    public Transform look;
    public ParticleSystem outline;

    [Header("Targeting Settings")]
    public float castRadius = 0.25f;
    public float sphereRadius = 0.5f;
    public float castDistance = 150;
    public LayerMask castMask;
    public LayerMask sphereMask;
    public string liftTag = "Liftable";
    public string grabTag = "Item";

    private RaycastHit hit;
    private Collider[] overlaps;

    // Update is called once per frame
    void Update()
    {
        if(Physics.SphereCast(look.position, castRadius, look.forward, out hit, castDistance, castMask))
        {
            overlaps = Physics.OverlapSphere(hit.point, sphereRadius, sphereMask);
            float distance = Mathf.Infinity;
            Collider theOne = null;

            foreach(Collider c in overlaps)
            {
                float newDist = Vector3.Distance(hit.point, c.transform.position);
                if (newDist < distance && (c.CompareTag(liftTag) || c.CompareTag(grabTag)))
                {
                    distance = newDist;
                    theOne = c;
                }
            }

            if (theOne)
            {
                Target = theOne.gameObject;
                SetOutline();
            }
            else
            {
                Target = null;
                ResetOutline();
            }
        }
        else
        {
            Target = null;
            ResetOutline();
        }

        //Debug.Log(Target);
    }

    

    void SetOutline()
    {
        ParticleSystem.ShapeModule shape = outline.shape;

        MeshFilter filter = Target.GetComponent<MeshFilter>();

        shape.mesh = filter.mesh;

        outline.transform.SetParent(Target.transform);
        outline.transform.localPosition = Vector3.zero;
        outline.transform.localEulerAngles = Vector3.zero;
        outline.Play();
    }

    void ResetOutline()
    {
        outline.Stop();
    }
}
