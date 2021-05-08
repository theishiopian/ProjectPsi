using Impact.Utility.ObjectPool;
using UnityEngine;

namespace Impact.Interactions.Particles
{
    /// <summary>
    /// Base Component for particles emitted by particle interactions
    /// This can be inherited from if special decal particle is needed.
    /// This type is used by the ImpactParticlePool.
    /// </summary>
    public abstract class ImpactParticlesBase : PooledObject
    {
        /// <summary>
        /// The priority associated with these particles.
        /// </summary>
        public abstract int Priority { get; set; }

        /// <summary>
        /// The size of the object pool created for these particles.
        /// </summary>
        public abstract int PoolSize { get; set; }

        /// <summary>
        /// Emit particles for the given interaction result.
        /// </summary>
        /// <param name="interactionResult">The result to emit particles for.</param>
        /// <param name="point">The point at which to emit the particles.</param>
        /// <param name="normal">The surface normal for rotating the particles.</param>
        /// <param name="priority">The priority of the particles.</param>
        public abstract void EmitParticles(ParticleInteractionResult interactionResult, Vector3 point, Vector3 normal, int priority);

        /// <summary>
        /// Update the position and rotation of the particles.
        /// </summary>
        /// <param name="point">The new position.</param>
        /// <param name="normal">The new normal used to get the rotation.</param>
        public abstract void UpdateTransform(Vector3 point, Vector3 normal);

        /// <summary>
        /// Stops all particle systems.
        /// </summary>
        public abstract void Stop();
    }
}
