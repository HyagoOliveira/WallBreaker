using UnityEngine;

namespace WallBreaker.Gameplay
{
    /// <summary>
    /// The default Paddle.
    /// </summary>
    public sealed class Paddle : IPaddle
    {
        /// <summary>
        /// The maximum speed allowed.
        /// </summary>
        public const float MAX_SPEED = 100F;

        public float Speed
        {
            get => speed;
            set => speed = Mathf.Clamp(value, 0f, MAX_SPEED);
        }

        public Vector3 Size => collider.Size;

        private float speed;

        private readonly IRigidbody body;
        private readonly ICollider collider;

        public Paddle(IRigidbody body, ICollider collider)
        {
            this.body = body;
            this.collider = collider;
        }

        public void Initialize()
        {
            body.IsKinematic = true;
        }

        public void MoveHorizontally(float input)
        {
            input = Mathf.Clamp(input, -1F, 1F);
            var hasInput = Mathf.Abs(input) > 0F;
            var horizontal = hasInput ? input * Speed : 0F;

            body.Velocity = Vector3.right * horizontal;
        }
    }
}