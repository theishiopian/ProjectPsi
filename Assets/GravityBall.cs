using System.Collections;
using System.Collections.Generic;
using Unity.Labs.SuperScience;
using UnityEngine;

public class GravityBall : MonoBehaviour
{
    public float radius = 5;
    public float gravity = 9.81f;

    private PhysicsTracker tracker;

    // Start is called before the first frame update
    void Start()
    {
        tracker = new PhysicsTracker();
        this.gameObject.SetActive(false);
    }

    Collider[] objects;
    Rigidbody body;

    private void FixedUpdate()
    {
        tracker.Update(transform.position, transform.rotation, Time.fixedDeltaTime);
        objects = Physics.OverlapSphere(transform.position, radius);

        for(int i = 0; i != objects.Length; i++)
        {
            body = objects[i].GetComponent<Rigidbody>();

            if(body != null)
            {
                body.AddForce((body.position - transform.position).normalized * gravity / Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
    }
}
