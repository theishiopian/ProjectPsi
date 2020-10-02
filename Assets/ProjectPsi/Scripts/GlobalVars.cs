using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    //global system
    private static Dictionary<string, GameObject> globalObjects = new Dictionary<string, GameObject>();

    public static void Add(string key, GameObject value)
    {
        globalObjects[key] = value;
    }

    public static GameObject Get(string key)
    {
        return globalObjects[key];
    }

    //local script
    public string key;

    private void Awake()
    {
        Add(key, this.gameObject);
        //Debug.Log(Get(key));
    }
}