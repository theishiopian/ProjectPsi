using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Health : MonoBehaviour, IDamageable
{
    public void Damage(float amount)
    {
        throw new System.NotImplementedException();
    }

    public bool IsAlive()
    {
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
