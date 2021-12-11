using UnityEngine;
using UnityEngine.InputSystem;

namespace WallBreaker.Gameplay
{
    /// <summary>
    /// Behaviour for Paddle. This components fallows the Humble Object pattern.
    /// <para>Access the <see cref="Paddle"/> property after Awake.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PaddleBehaviour : MonoBehaviour
    {
        [SerializeField, Tooltip("The current Paddle speed.")]
        private float speed = 5f;

        /// <summary>
        /// The Paddle instance. <b>Use it only after Awake</b>.
        /// </summary>
        public IPaddle Paddle { get; private set; }

        private void Awake() => InitializePaddle();

        private void OnValidate()
        {
            if (Paddle != null) Paddle.Speed = speed;
        }

        public void OnMove(InputValue input)
        {
            var horizontalInput = input.Get<float>();
            Paddle.MoveHorizontally(horizontalInput);
        }

        private void InitializePaddle()
        {
            Paddle = new Paddle(
                RigidbodyAdapterFactory.Create(gameObject),
                ColliderAdapterFactory.Create(gameObject)
            )
            {
                Speed = speed
            };
        }
    }
}