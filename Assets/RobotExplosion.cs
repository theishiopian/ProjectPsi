using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotExplosion : MonoBehaviour
{
    public List<Rigidbody> parts;
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(Rigidbody part in parts)
        {
            part.AddExplosionForce(10, transform.position, 1, 0.3f, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
