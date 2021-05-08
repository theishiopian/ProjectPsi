using Impact.Utility.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Impact.Interactions.Decals
{
    /// <summary>
    /// An object pool used for placing decals from DecalInteractionResults.
    /// </summary>
    public class ImpactDecalPool : ObjectPool<ImpactDecalBase>
    {
        private static ObjectPoolGroup<ImpactDecalPool, ImpactDecalBase> poolGroup = new ObjectPoolGroup<ImpactDecalPool, ImpactDecalBase>();

        /// <summary>
        /// Create a pool for the given decal template.
        /// </summary>
        /// <param name="template">The decal prefab to create a pool for.</param>
        public static void PreloadPoolForDecal(ImpactDecalBase template)
        {
            poolGroup.GetOrCreatePool(template, template.PoolSize);
        }

        /// <summary>
        /// Retrieve a decal from the pool.
        /// This will ALWAYS return a decal. If all decals are unavailable, the oldest active decal will be used.
        /// </summary>
        /// <param name="collisionResult">The result to create the decal for.</param>
        /// <param name="point">The point at which to place the decal.</param>
        /// <param name="normal">The surface normal for the decal rotation.</param>
        /// <returns>An ImpactDecal instance.</returns>
        public static ImpactDecalBase CreateDecal(DecalInteractionResult collisionResult, Vector3 point, Vector3 normal)
        {
            if (collisionResult.DecalTemplate == null)
                return null;

            ImpactDecalPool pool = poolGroup.GetOrCreatePool(collisionResult.DecalTemplate, collisionResult.DecalTemplate.PoolSize);

            if (pool != null)
            {
                ImpactDecalBase a = pool.GetObject();
                if (a != null)
                    a.SetupDecal(collisionResult, point, normal);

                return a;
            }

            return null;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            poolGroup.Remove(this);
        }

        /// <summary>
        /// Gets a decal from the pool. 
        /// If all decals are unavailable, gets the oldest active decal.
        /// </summary>
        /// <returns>An Impact Decal instance</returns>
        public override ImpactDecalBase GetObject()
        {
            int checkedIndices = 0;
            int i = lastAvailable;

            int oldestIndex = 0;
            float oldestTime = float.MaxValue;

            while (checkedIndices < pooledObjects.Length)
            {
                ImpactDecalBase a = pooledObjects[i];

                if (a.LastRetrievedFrame < oldestTime)
                {
                    oldestTime = a.LastRetrievedFrame;
                    oldestIndex = i;
                }

                if (a.IsAvailable())
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

            pooledObjects[oldestIndex].Retrieve();
            return pooledObjects[oldestIndex];
        }
    }
}