using System.Collections.Generic;
using UnityEngine;

namespace Impact.Utility.ObjectPool
{
    /// <summary>
    /// Keeps track of a group of object pools with templates of the same type.
    /// </summary>
    /// <typeparam name="TPool">The type of pool that is being grouped.</typeparam>
    /// <typeparam name="TTemplate">The type of the template object.</typeparam>
    public class ObjectPoolGroup<TPool, TTemplate> where TPool : ObjectPool<TTemplate> where TTemplate : PooledObject
    {
        private List<TPool> pools = new List<TPool>();

        /// <summary>
        /// Gets an existing pool with the given template, or creates it if it does not exist.
        /// </summary>
        /// <param name="template">The template object.</param>
        /// <param name="poolSize">The size of the pool if it needs to be created.</param>
        /// <returns>The existing or created pool.</returns>
        public TPool GetOrCreatePool(TTemplate template, int poolSize)
        {
            if (template == null)
                return null;

            TPool pool = GetPool(template);
            if (pool != null)
                return pool;

            return CreatePool(template, poolSize);
        }

        /// <summary>
        /// Attempts to get an existing pool with the given template.
        /// </summary>
        /// <param name="template">The template object.</param>
        /// <returns>The existing pool or null if no pool was found.</returns>
        public TPool GetPool(TTemplate template)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i].Template == template)
                    return pools[i];
            }

            return null;
        }

        /// <summary>
        /// Creates a new pool.
        /// </summary>
        /// <param name="template">The template object.</param>
        /// <param name="poolSize">The size of the pool.</param>
        /// <returns>The created pool.</returns>
        public TPool CreatePool(TTemplate template, int poolSize)
        {
            GameObject g = new GameObject("Object Pool (" + template.name + ")");
            Object.DontDestroyOnLoad(g);

            TPool pool = g.AddComponent<TPool>();
            pool.Template = template;
            pool.Initialize(poolSize);

            Add(pool);

            return pool;
        }

        /// <summary>
        /// Adds a pool to the group.
        /// </summary>
        /// <param name="pool">The pool to add.</param>
        public void Add(TPool pool)
        {
            pools.Add(pool);
        }

        /// <summary>
        /// Removes a pool from the group.
        /// </summary>
        /// <param name="pool">The pool to remove.</param>
        public void Remove(TPool pool)
        {
            pools.Remove(pool);
        }
    }
}
