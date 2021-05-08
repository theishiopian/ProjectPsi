using Impact.Utility;
using Impact.Utility.ObjectPool;
using UnityEngine;

namespace Impact.Interactions.Decals
{
    /// <summary>
    /// Standard implementation of ImpactDecalBase.
    /// </summary>
    [AddComponentMenu("Impact/Impact Decal")]
    public class ImpactDecal : ImpactDecalBase
    {
        /// <summary>
        /// Modes for setting how a decal should be rotated.
        /// </summary>
        public enum DecalRotationMode
        {
            /// <summary>
            /// Randomly rotate the decal on the surface normal axis.
            /// </summary>
            Random = 0,
            /// <summary>
            /// Rotate the decal to match the interaction velocity.
            /// </summary>
            Velocity = 1,
            /// <summary>
            /// Only rotate to match the interaction normal.
            /// </summary>
            Normal = 2
        }

        /// <summary>
        /// Modes for setting which transform axis should point towards the surface.
        /// </summary>
        public enum DecalAxis
        {
            ZDown = 0,
            ZUp = 1,
            YDown = 2,
            YUp = 3
        }

        [SerializeField]
        private int _poolSize = 50;
        [SerializeField]
        private float _decalDistance = 0.01f;
        [SerializeField]
        private DecalRotationMode _rotationMode = DecalRotationMode.Random;
        [SerializeField]
        private DecalAxis _axis = DecalAxis.ZDown;
        [SerializeField]
        private bool _parentToObject = true;

        /// <summary>
        /// The size of the pool to be created for this decal.
        /// </summary>
        public override int PoolSize
        {
            get { return _poolSize; }
            set { _poolSize = value; }
        }

        /// <summary>
        /// The distance the decal should be placed from the surface.
        /// </summary>
        public float DecalDistance
        {
            get { return _decalDistance; }
            set { _decalDistance = value; }
        }

        /// <summary>
        /// How the decal should be rotated.
        /// </summary>
        public DecalRotationMode RotationMode
        {
            get { return _rotationMode; }
            set { _rotationMode = value; }
        }

        /// <summary>
        /// Which axis should be pointed towards the surface.
        /// </summary>
        public DecalAxis Axis
        {
            get { return _axis; }
            set { _axis = value; }
        }

        /// <summary>
        /// Should the decal be parented to the object it is placed on?
        /// </summary>
        public bool ParentToObject
        {
            get { return _parentToObject; }
            set { _parentToObject = value; }
        }

        private DestroyMessenger parentObject;

        /// <summary>
        /// Places the decal at the given point.
        /// </summary>
        /// <param name="interactionResult">The interaction result this decal is being created for.</param>
        /// <param name="point">The point at which to place the decal.</param>
        /// <param name="normal">The surface normal used to set the decal rotation.</param>
        public override void SetupDecal(DecalInteractionResult interactionResult, Vector3 point, Vector3 normal)
        {
            transform.position = point + normal * _decalDistance;

            if (RotationMode == DecalRotationMode.Random)
            {
                transform.rotation = getNormalRotation(normal);
                rotateRandom();
            }
            else if (RotationMode == DecalRotationMode.Velocity)
            {
                transform.rotation = getVelocityRotation(normal, interactionResult.OriginalData.Velocity);
            }
            else
            {
                transform.rotation = getNormalRotation(normal);
            }

            if (ParentToObject && interactionResult.OriginalData.OtherObject)
            {
                transform.SetParent(interactionResult.OriginalData.OtherObject.transform);

                parentObject = interactionResult.OriginalData.OtherObject.GetOrAddComponent<DestroyMessenger>();
                parentObject.OnDestroyed += onParentDestroyed;
            }
            else
                transform.SetParent(OriginalParent, true);
        }

        private Quaternion getNormalRotation(Vector3 normal)
        {
            if (_axis == DecalAxis.ZDown)
                return Quaternion.LookRotation(-normal);
            else if (_axis == DecalAxis.ZUp)
                return Quaternion.LookRotation(normal);
            else if (_axis == DecalAxis.YDown)
            {
                Quaternion q = Quaternion.LookRotation(-normal);
                return q * Quaternion.Euler(90, 0, 0);
            }
            else if (_axis == DecalAxis.YUp)
            {
                Quaternion q = Quaternion.LookRotation(normal);
                return q * Quaternion.Euler(90, 0, 0);
            }

            return Quaternion.LookRotation(normal);
        }

        private Quaternion getVelocityRotation(Vector3 normal, Vector3 velocity)
        {
            if (_axis == DecalAxis.ZDown)
                return Quaternion.LookRotation(-normal, velocity);
            else if (_axis == DecalAxis.ZUp)
                return Quaternion.LookRotation(normal, velocity);
            else if (_axis == DecalAxis.YDown)
            {
                Quaternion q = Quaternion.LookRotation(-normal, velocity);
                return q * Quaternion.Euler(90, 0, 0);
            }
            else if (_axis == DecalAxis.YUp)
            {
                Quaternion q = Quaternion.LookRotation(normal, -velocity);
                return q * Quaternion.Euler(90, 0, 0);
            }

            return Quaternion.LookRotation(normal, velocity);
        }

        private void rotateRandom()
        {
            if (Axis == DecalAxis.ZDown || Axis == DecalAxis.ZUp)
                transform.Rotate(new Vector3(0, 0, Random.value * 360f), Space.Self);
            else
                transform.Rotate(new Vector3(0, Random.value * 360f, 0), Space.Self);
        }

        private void onParentDestroyed()
        {
            MakeAvailable();
        }

        public override void Retrieve()
        {
            if (parentObject)
                parentObject.OnDestroyed -= onParentDestroyed;
            parentObject = null;

            base.Retrieve();
        }

        public override void MakeAvailable()
        {
            if (parentObject)
                parentObject.OnDestroyed -= onParentDestroyed;
            parentObject = null;

            base.MakeAvailable();
        }
    }
}
