using Impact.Utility.ObjectPool;
using UnityEngine;

namespace Impact.Interactions.Particles
{
    /// <summary>
    /// Standard implementation of ImpactParticlesBase.
    /// </summary>
    [AddComponentMenu("Impact/Impact Particles")]
    public class ImpactParticles : ImpactParticlesBase
    {
        [SerializeField]
        private int _poolSize = 50;

        private ParticleSystem[] particles;

        /// <summary>
        /// The priority associated with these particles.
        /// </summary>
        public override int Priority { get; set; }

        /// <summary>
        /// The size of the object pool created for these particles.
        /// </summary>
        public override int PoolSize
        {
            get { return _poolSize; }
            set { _poolSize = value; }
        }

        private void Awake()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        /// <summary>
        /// Emit particles for the given interaction result.
        /// </summary>
        /// <param name="interactionResult">The result to emit particles for.</param>
        /// <param name="point">The point at which to emit the particles.</param>
        /// <param name="normal">The surface normal for rotating the particles.</param>
        /// <param name="priority">The priority of the particles.</param>
        public override void EmitParticles(ParticleInteractionResult interactionResult, Vector3 point, Vector3 normal, int priority)
        {
            Priority = priority;

            UpdateTransform(point, normal);

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        /// <summary>
        /// Update the position and rotation of the particles.
        /// </summary>
        /// <param name="point">The new position.</param>
        /// <param name="normal">The new normal used to get the rotation.</param>
        public override void UpdateTransform(Vector3 point, Vector3 normal)
        {
            transform.position = point;
            transform.rotation = Quaternion.LookRotation(normal);
        }

        /// <summary>
        /// Stops all particle systems.
        /// </summary>
        public override void Stop()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
            }
        }

        private void Update()
        {
            if (!IsAvailable())
            {
                bool isAlive = false;
                for (int i = 0; i < particles.Length; i++)
                {
                    if (particles[i].IsAlive())
                        isAlive = true;
                }

                if (!isAlive)
                    MakeAvailable();
            }
        }
    }
}
