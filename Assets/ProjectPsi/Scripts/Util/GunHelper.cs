using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    void Fire(GameObject target);
}

public class GunHelper : MonoBehaviour
{
    [SerializeField]
    public MonoBehaviour gun;

    // Start is called before the first frame update
    void Start()
    {
        if(!gun is IGun)
        {
            Debug.Log("invalid gun");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        (gun as IGun).Fire(other);
    }
}
