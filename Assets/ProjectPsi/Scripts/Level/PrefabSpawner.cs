using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;

    public void SpawnPrefab()
    {
        if(prefab != null)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("no prefab provided");
        }
    }
}
