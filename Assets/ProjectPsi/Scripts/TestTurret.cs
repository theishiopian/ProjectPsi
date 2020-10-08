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

        InvokeRepeating("Cleanup", 5, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Vector3 heading = target.transform.position - transform.position;

                GameObject b = Instantiate(prefab, spawner.position, Quaternion.identity);

                b.GetComponent<Rigidbody>().AddForce(heading.normalized * force, ForceMode.Impulse);
                bullets.Add(b);
                timer = interval;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("projectile"))
        {
            float force = collision.impulse.magnitude / Time.fixedDeltaTime;
            health -= (int)force;
            //Debug.Log("impact of " + force + " detected by turret, health at " + health);
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
        Destroy(bullets[0]);
        bullets.Remove(bullets[0]);
    }
}