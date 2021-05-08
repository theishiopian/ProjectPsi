using System;
using UnityEngine;

namespace Impact.Utility.ObjectPool
{
    /// <summary>
    /// A generic class for handling object pools.
    /// </summary>
    /// <typeparam name="T">The type of object that this pool contains.</typeparam>
    public class ObjectPool<T> : MonoBehaviour where T : PooledObject
    {
        [SerializeField]
        protected int _poolSize;

        /// <summary>
        /// Array of all objects in the pool.
        /// </summary>
        protected T[] pooledObjects;

        /// <summary>
        /// The index of pooledObjects that was available when the most recent request for an object was made.
        /// </summary>
        protected int lastAvailable;

        /// <summary>
        /// The object the pool will make copies of.
        /// This can be null, and you can override createPooledObjectInstance if you need special instantiation code.
        /// </summary>
        public T Template { get; set; }

        /// <summary>
        /// The configured number of objects that are in this pool.
        /// </summary>
        public int PoolSize
        {
            get { return _poolSize; }
            set { _poolSize = value; }
        }

        /// <summary>
        /// Instantiates the given number of objects and makes them a part of this pool.
        /// </summary>
        /// <param name="poolSize">The number of objects to create.</param>
        public virtual void Initialize(int poolSize)
        {
            pooledObjects = new T[poolSize];

            for (int i = 0; i < pooledObjects.Length; i++)
            {
                pooledObjects[i] = createPooledObjectInstance(i);
                pooledObjects[i].OriginalParent = this.transform;
                pooledObjects[i].transform.SetParent(this.transform);
                pooledObjects[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Creates and returns a new instance of an object to be put into the pool. Typically this object will be the Template object.
        /// </summary>
        /// <param name="index">The index of the pool this object is being created at.</param>
        /// <returns>The instantiated object.</returns>
        protected virtual T createPooledObjectInstance(int index)
        {
            T instance = Instantiate(Template, this.transform);
            instance.gameObject.name = Template.name + "_" + index;
            return instance;
        }

        /// <summary>
        /// Attempts to retrieve an object from the pool.
        /// </summary>
        /// <returns>The object retrieved from the pool, or null if no object could be retreieved.</returns>
        public virtual T GetObject()
        {
            return GetObject(o => o.IsAvailable());
        }

        /// <summary>
        /// Attempts to retrieve an object from the pool.
        /// </summary>
        /// <param name="predicate">Function that should return true if an object is available for use, false otherwise.</param>
        /// <returns>The object retrieved from the pool, or null if no object could be retreieved.</returns>
        public virtual T GetObject(Func<T, bool> predicate)
        {
            int checkedIndices = 0;
            int i = lastAvailable;

            while (checkedIndices < pooledObjects.Length)
            {
                T a = pooledObjects[i];

                if (predicate(a))
                {
                    lastAvailable = i;
                    a.Retrieve();
                    return a;
                }

                i++;
                checkedIndices++;

                if (i >= pooledObjects.Length)
                    i = 0;
            }

            return null;
        }

        /// <summary>
        /// Destroys all objects when the pool is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            for (int i = 0; i < pooledObjects.Length; i++)
            {
                Destroy(pooledObjects[i]);
            }
        }
    }
}