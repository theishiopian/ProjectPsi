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

    private GameObject cache;//this var is used for internal operations that require storing a game object

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
        cache = available.Dequeue();
        cache.transform.position = pos;
        cache.transform.rotation = rot;
        cache.SetActive(true);
        inUse.Enqueue(cache);
        
        if(available.Count == 0)
        {
            if(shouldGrow)
            {
                GrowPool(prefab);
            }
            else
            {
                Recycle(recycleOp);
            }
        }

        return cache;
    }

    /// <summary>
    /// This method manually tells the pool to recycle. By default the pool does this automatically
    /// </summary>
    /// <param name="cleanup">An optional delegate for recycling old objects</param>
    public void Recycle(CleanupOperation cleanup = null)
    {
        cache = inUse.Dequeue();

        if(cleanup != null)
        {
            cleanup.Invoke(cache);
        }

        cache.SetActive(false);
        available.Enqueue(cache);
    }

    public int GetPoolSize()
    {
        return available.Count + inUse.Count;
    }

    //internal method for creating objects for the pool
    private void GrowPool(GameObject prefab)
    {
        cache = Object.Instantiate(prefab, parent);
        cache.hideFlags = HideFlags.HideInHierarchy;//makes your pooled objects invisible in the hierarchy, comment out to be able to see them
        available.Enqueue(cache);
        cache.SetActive(false);
    }
}
