using UnityEngine;
using Impact.Utility.ObjectPool;

namespace Impact.Interactions.Audio
{
    /// <summary>
    /// Object pool for playing audio from AudioInteractionResults.
    /// </summary>
    public class ImpactAudioPool : ObjectPool<ImpactAudioSourceBase>
    {
        private static ObjectPoolGroup<ImpactAudioPool, ImpactAudioSourceBase> poolGroup = new ObjectPoolGroup<ImpactAudioPool, ImpactAudioSourceBase>();

        /// <summary>
        /// Create a pool for the given audio source template.
        /// </summary>
        /// <param name="template">The decal prefab to create a pool for.</param>
        public static void PreloadPoolForAudioSource(ImpactAudioSourceBase template)
        {
            poolGroup.GetOrCreatePool(template, template.PoolSize);
        }


        /// <summary>
        /// Plays audio for an AudioInteractionResult.
        /// </summary>
        /// <param name="interactionResult">The interaction result to use data from.</param>
        /// <param name="point">The world position to play the audio at.</param>
        /// <param name="priority">The priority of the sound. Audio sources with a priority less than this will be "stolen" and used to play this audio.</param>
        /// <returns>The ImpactAudioSource that will be used to play the audio. Will be null if no audio sources are available.</returns>
        public static ImpactAudioSourceBase PlayAudio(AudioInteractionResult interactionResult, Vector3 point, int priority)
        {
            if (interactionResult.AudioSourceTemplate == null)
                return null;

            ImpactAudioPool pool = poolGroup.GetOrCreatePool(interactionResult.AudioSourceTemplate, interactionResult.AudioSourceTemplate.PoolSize);

            if (pool != null)
            {
                ImpactAudioSourceBase a = pool.GetObject();

                //If all objects are in use, try to get one with a lower priority
                if (a == null)
                    a = pool.GetObject((p) => p.Priority < priority);

                if (a != null)
                    a.PlayAudio(interactionResult, point, priority);

                return a;
            }

            return null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            poolGroup.Remove(this);
        }
    }
}
