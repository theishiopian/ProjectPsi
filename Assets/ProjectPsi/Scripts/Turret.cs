using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Objects")]
    public ParticleSystem muzzleflash;
    public ParticleSystem shells;
    public Transform shootPoint;
    public Transform bracket;
    public Transform gun;

    [Header("Tracking Settings")]
    public LayerMask losMask;

    [Header("Pool Settings")]
    public GameObject prefab;
    public int poolSize = 30;

    private GameObjectPool bullets;
    private bool tracking = false;
    private bool firing = false;
    private Transform target;
    private float fireTime = 0;

    private void Start()
    {
        bullets = new GameObjectPool(prefab, poolSize, null);
    }

    // Update is called once per frame
    void Update()
    {
        if(tracking)
        {
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.rotation = Quaternion.Lerp(bracket.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.localEulerAngles = new Vector3(0, bracket.localEulerAngles.y, 0);
            gun.localEulerAngles = new Vector3(gun.localEulerAngles.x, -90, 0);

            RaycastHit hit;

            bool didHit = Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, losMask);

            if(didHit && hit.collider.CompareTag("Player"))
            {
                StartFiring();
            }
            else
            {
                StopFiring();
            }
        }
        else
        {
            //gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.identity, Time.deltaTime * 3);
            //bracket.rotation = Quaternion.Lerp(bracket.rotation, Quaternion.Euler(0,-90,0), Time.deltaTime * 3);
            StopFiring();
        }
        
        if(firing)
        {
            fireTime += Time.deltaTime;

            if(fireTime > 0.1f)
            {
                fireTime = 0;
                Rigidbody bullet = bullets.Activate(shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>();

                bullet.AddRelativeForce(0,0,10, ForceMode.Impulse);
            }
        }
        else
        {
            fireTime = 0;
        }
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

    private void StartFiring()
    {
        muzzleflash.Play();
        shells.Play();
        firing = true;
    }

    private void StopFiring()
    {
        muzzleflash.Stop();
        shells.Stop();
        firing = false;
    }
}
