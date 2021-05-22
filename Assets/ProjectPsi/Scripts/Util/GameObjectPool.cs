using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is an object pool for game objects
/// 
/// To use it, create a variable of type 'GameObjectPool' in a 
/// script, then in start initilize it using the constructer (e.g pool = new GameObjectPool(params here))
/// 
/// license: GNU GPL 3.0 https://www.gnu.org/licenses/gpl-3.0.en.html
/// </summary>
public class GameObjectPool
{
    //Type for cleanup operation delegate
    public delegate void CleanupOperation(GameObject toClean);

    //vars that should be publically changeable
    public bool shouldGrow = false;
    public Transform parent;
    
    //private vars
    private Queue<GameObject> available;
    private Queue<GameObject> inUse;
    private GameObject prefab;

    /// <summary>
    /// The Constructor, used to initilize a new Object Pool
    /// </summary>
    /// <param name="prefab">The template to use for the pool's objects</param>
    /// <param name="initialSize">The amount of objects to initilize</param>
    /// <param name="shouldGrow">Optional parameter. Set to true to allow the pool to grow itself if needed</param>
    /// <param name="parent">Optional parameter. Use this if you want your pooled objects parented to something</param>
    public GameObjectPool(GameObject prefab, int initialSize, Transform parent, bool shouldGrow = false)
    {
        //init queues
        available = new Queue<GameObject>();
        inUse = new Queue<GameObject>();

        //init vars
        this.parent = parent;
        this.prefab = prefab;
        this.shouldGrow = shouldGrow;

        //allocate objects
        for (int i = 0; i < initialSize; i++)
        {
            GrowPool(this.prefab);
        }
    }

    //this is a doc comment, it creates a tooltip for the method. hover over the method name to see it!
    /// <summary>
    /// This method gets a fresh object from the pool
    /// </summary>
    /// <param name="pos">The position to move the fresh object to</param>
    /// <param name="rot">The rotation to bestow upon the fresh object</param>
    /// <param name="recycleOp">An optional delegate for recycling old objects</param>
    /// <returns>The object that was activated, so you can do stuff to it</returns>
    public GameObject Activate(Vector3 pos, Quaternion rot, CleanupOperation recycleOp = null)
    {
        GameObject toActivate;
        if (available.Count == 1)
        {
            //Debug.LogWarning("Out of Objects");
            if (shouldGrow)
            {
                GrowPool(prefab);
            }
            else
            {
                Recycle(recycleOp);
            }
        }

        //Debug.Log("Activating");
        //DebugState();
        toActivate = available.Dequeue();
        //Debug.Log("Activating " + toActivate.name);
        toActivate.transform.position = pos;
        toActivate.transform.rotation = rot;
        toActivate.SetActive(true);
        inUse.Enqueue(toActivate);

        //Debug.Log("Done Activating");
        //DebugState();

        return toActivate;
    }

    /// <summary>
    /// This method manually tells the pool to recycle. By default the pool does this automatically
    /// </summary>
    /// <param name="cleanup">An optional delegate for recycling old objects</param>
    public void Recycle(CleanupOperation cleanup = null)
    {
        GameObject toRecycle;
        //Debug.Log("Recycling");
        //DebugState();
        toRecycle = inUse.Dequeue();
        //Debug.Log("Recycling " + toRecycle.name);

        if(cleanup != null)
        {
            cleanup.Invoke(toRecycle);
        }

        toRecycle.SetActive(false);
        available.Enqueue(toRecycle);
        //Debug.Log("Done Recyling");
        //DebugState();
    }

    public int GetPoolSize()
    {
        return available.Count + inUse.Count;
    }
    int num = 0;
    //internal method for creating objects for the pool
    private void GrowPool(GameObject prefab)
    {
        GameObject toMake;
        //Debug.Log("Growing Pool");
        //DebugState();
        toMake = Object.Instantiate(prefab, parent);
        toMake.name = (++num).ToString();
        //toMake.hideFlags = HideFlags.HideInHierarchy;//makes your pooled objects invisible in the hierarchy, comment out to be able to see them
        available.Enqueue(toMake);
        toMake.SetActive(false);
        //Debug.Log("Done Growing");
        //DebugState();
    }

    private void DebugState()
    {
        Debug.Log("Available: " + available.Count);
        Debug.Log("In Use: " + inUse.Count);
    }
}
