using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void Damage(float amount)
    {
        //throw new System.NotImplementedException();
        return;
    }

    public bool IsAlive()
    {
        //throw new System.NotImplementedException();
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
