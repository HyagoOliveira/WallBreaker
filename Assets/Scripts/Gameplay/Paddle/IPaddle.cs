using UnityEngine;

namespace WallBreaker.Gameplay
{
    /// <summary>
    /// Interface used on objects able to be a Paddle.
    /// </summary>
    public interface IPaddle
    {
        /// <summary>
        /// The current Paddle speed.
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// The current Paddle size.
        /// </summary>
        Vector3 Size { get; }

        /// <summary>
        /// Initializes the Paddle.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Moves the Paddle horizontally using the given input.
        /// </summary>
        /// <param name="input">The input to move horizontally.</param>
        void MoveHorizontally(float input);
    }
}
