using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float regenerationDelay = 3;

    private float health = 100;//yes float health is silly, but it makes regeneration so much easier

    private void Awake()
    {
        GlobalVars.playerHealth = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;

        if(!(timer > 0))
        {
            ModifyHealth(Time.deltaTime * 10);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ModifyHealth(-10);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void ModifyHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0 ,100);
        if(amount < 0)timer = regenerationDelay;
    }
}
