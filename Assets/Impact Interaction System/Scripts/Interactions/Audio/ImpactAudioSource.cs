using Impact.Utility.ObjectPool;
using UnityEngine;

namespace Impact.Interactions.Audio
{
    /// <summary>
    /// Component for playing audio interactions.
    /// </summary>
    [AddComponentMenu("Impact/Impact Audio Source")]
    public class ImpactAudioSource : ImpactAudioSourceBase
    {
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private int _poolSize = 20;

        /// <summary>
        /// The size of the pool to be created for this Audio Source.
        /// </summary>
        public override int PoolSize
        {
            get { return _poolSize; }
            set { _poolSize = value; }
        }

        private int priority;

        /// <summary>
        /// The priority associated with this audio source.
        /// </summary>
        public override int Priority { get { return priority; } }

        private float baseVolume, basePitch;

        private void Reset()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void Awake()
        {
            baseVolume = _audioSource.volume;
            basePitch = _audioSource.pitch;
        }

        /// <summary>
        /// Plays audio for the given audio interaction result.
        /// </summary>
        /// <param name="interactionResult">The result to get the audio clip, volume, and pitch from.</param>
        /// <param name="point">The point to play the audio at.</param>
        /// <param name="priority">The priority of the interaction.</param>
        public override void PlayAudio(AudioInteractionResult interactionResult, Vector3 point, int priority)
        {
            transform.position = point;

            _audioSource.loop = interactionResult.LoopAudio;
            _audioSource.clip = interactionResult.AudioClip;
            _audioSource.volume = baseVolume * interactionResult.Volume;
            _audioSource.pitch = basePitch * interactionResult.Pitch;

            _audioSource.Play();

            this.priority = priority;
        }

        /// <summary>
        /// Updates the volume and pitch of the audio for sliding and rolling.
        /// </summary>
        /// <param name="volume">The new volume.</param>
        /// <param name="pitch">The new pitch.</param>
        public override void UpdateAudio(float volume, float pitch)
        {
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
        }

        /// <summary>
        /// Stops the audio source.
        /// </summary>
        public override void StopAudio()
        {
            _audioSource.Stop();
        }

        private void Update()
        {
            if (!IsAvailable() && !_audioSource.isPlaying)
            {
                MakeAvailable();
            }
        }
    }
}
