using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public float damageOverTime = 10;
    public AudioSource noise;

    bool inContact = false;

    // Update is called once per frame
    void Update()
    {
        if(inContact)
        {
            PlayerHealth health = GlobalVars.Get("player_rig").GetComponent<PlayerHealth>();
            health.Damage(damageOverTime * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            inContact = true;
            if (noise != null)
            {
                noise.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            inContact = false;
            if (noise != null)
            {
                noise.Stop();
            }
        }
    }
}
