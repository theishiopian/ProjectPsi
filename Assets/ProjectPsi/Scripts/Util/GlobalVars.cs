using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    //global system
    private static Dictionary<string, GameObject> globalObjects = new Dictionary<string, GameObject>();

    public static void Add(string key, GameObject value)
    {
        globalObjects[key] = value;
        //if (!globalObjects.ContainsKey(key))
        //    globalObjects[key] = value;
        //else
        //{
        //    Debug.Log("Key " + key + " already present in dictionary with value of: " + globalObjects[key] + ", Destroying duplicate");
        //    Destroy(value);
        //}
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