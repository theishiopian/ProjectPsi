using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurret : MonoBehaviour
{
    public GameObject prefab;
    public float force = 10;
    public float interval = 2;
    public Material deaded;
    public int health = 10;

    private GameObject target;
    private Transform spawner;
    private float timer;
    private bool isAlive = true;
    private List<GameObject> bullets = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        target = GlobalVars.Get("head");
        spawner = transform.GetChild(0);
        timer = interval;

        InvokeRepeating("Cleanup", interval, interval);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Vector3 heading = target.transform.position - (transform.position + Vector3.up);

                GameObject b = Instantiate(prefab, spawner.position, Quaternion.identity);

                b.GetComponent<Rigidbody>().AddForce(heading.normalized * force, ForceMode.Impulse);
                bullets.Add(b);
                timer = interval;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("projectile"))//dont hit self with own bullets
        {
            float force = collision.impulse.magnitude;
            health -= (int)(force/100);
            Debug.Log("impact of " + (int)(force / 100) + " detected by turret, health at " + health);
        }

        if(health <= 0)
        {
            isAlive = false;
            //Debug.Log("turret done deaded inside");
            this.GetComponent<MeshRenderer>().material = deaded;
        }
    }

    private void Cleanup()
    {
        if (bullets.Count > 0)
        {
            Destroy(bullets[0]);
            bullets.Remove(bullets[0]);
        }

    }
}