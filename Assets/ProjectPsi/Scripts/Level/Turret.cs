using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : AbstractHealth, IGun
{
    [Header("Objects")]
    public ParticleSystem bullets;
    public ParticleSystem muzzleflash;
    public ParticleSystem shells;
    public Transform shootPoint;
    public Transform bracket;
    public Transform gun;
    public AudioSource gunSound;
    public float damage = 10;

    [Header("Tracking Settings")]
    public LayerMask losMask;

    private bool tracking = false;
    private bool firing = false;
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.rotation = Quaternion.Lerp(bracket.rotation, Quaternion.LookRotation((target.position - transform.position).normalized, transform.up), Time.deltaTime * 3);
            bracket.localEulerAngles = new Vector3(0, bracket.localEulerAngles.y, 0);
            gun.localEulerAngles = new Vector3(gun.localEulerAngles.x, -90, 0);

            RaycastHit hit;

            bool didHit = Physics.SphereCast(shootPoint.position, 0.5f, shootPoint.forward, out hit, 100, losMask, QueryTriggerInteraction.Ignore);
            Debug.Log(!didHit ? "No target" : "Target: " + hit.collider);
            if (didHit && hit.collider.CompareTag("Player"))
            {
                if (!firing) StartFiring();
            }
            else
            {
                if (firing) StopFiring();
            }
        }
        else
        {
            if (firing) StopFiring();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > 5)
        {
            Damage(collision.impulse.magnitude * 0.01f);
        }
    }

    private void StartFiring()
    {
        muzzleflash.Play();
        shells.Play();
        bullets.Play();
        gunSound.Play();
        firing = true;
    }

    private void StopFiring()
    {
        muzzleflash.Stop();
        muzzleflash.Clear();
        shells.Stop();
        bullets.Stop();
        gunSound.Stop();
        firing = false;
    }

    public void Fire(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = GlobalVars.Get("player_rig").GetComponent<PlayerHealth>();

            health.Damage(damage);
        }
    }
}
