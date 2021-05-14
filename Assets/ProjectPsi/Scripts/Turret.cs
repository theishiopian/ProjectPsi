using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public ParticleSystem muzzleflash;
    public ParticleSystem shells;
    public Transform shootPoint;
    public Transform bracket;
    public Transform gun;

    public LayerMask mask;

    private GameObjectPool bullets;
    private bool tracking = false;
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        if(tracking)
        {
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.eulerAngles = new Vector3(0, gun.eulerAngles.y, 0);
        }
        else
        {

        }
        Debug.Log(tracking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tracking = true;
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tracking = false;
            target = null;
        }
    }
}
