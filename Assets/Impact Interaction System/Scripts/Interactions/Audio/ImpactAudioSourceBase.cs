﻿using Impact.Utility.ObjectPool;
using UnityEngine;

namespace Impact.Interactions.Audio
{
    /// <summary>
    /// Base Component for audio sources created by audio interactions.
    /// This can be inherited from if special audio source functionality is needed.
    /// This type is used by the ImpactAudioPool.
    /// </summary>
    public abstract class ImpactAudioSourceBase : PooledObject
    {
        /// <summary>
        /// The size of the object pool to be created for this audio source.
        /// </summary>
        public abstract int PoolSize { get; set; }

        /// <summary>
        /// The priority associated with this audio source.
        /// </summary>
        public abstract int Priority { get; }

        /// <summary>
        /// Plays audio for the given audio interaction result.
        /// </summary>
        /// <param name="interactionResult">The result to get the audio clip, volume, and pitch from.</param>
        /// <param name="point">The point to play the audio at.</param>
        /// <param name="priority">The priority of the interaction.</param>
        public abstract void PlayAudio(AudioInteractionResult interactionResult, Vector3 point, int priority);

        /// <summary>
        /// Updates the volume and pitch of the audio for sliding and rolling.
        /// </summary>
        /// <param name="volume">The new volume.</param>
        /// <param name="pitch">The new pitch.</param>
        public abstract void UpdateAudio(float volume, float pitch);

        /// <summary>
        /// Stops the audio source.
        /// </summary>
        public abstract void StopAudio();
    }
}
