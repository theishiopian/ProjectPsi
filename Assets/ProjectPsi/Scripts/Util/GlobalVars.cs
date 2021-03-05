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
        try
        {
            return globalObjects[key];
        }
        catch
        {
            //TODO cleanup this garbo
            Debug.LogWarning("Attempted to get nonexistant key: " + key + ", if this isnt the player you may have a problem");
            return null;
        }
    }

    //local script
    public string key;
    public bool debug = false;

    private void Awake()
    {
        if (debug) Debug.Log("var awake");
        Add(key, this.gameObject);
    }
}