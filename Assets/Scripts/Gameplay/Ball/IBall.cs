using System;
using UnityEngine;

namespace WallBreaker.Gameplay
{
    /// <summary>
    /// Interface used on objects able to be a Ball.
    /// </summary>
    public interface IBall
    {
        /// <summary>
        /// Event fired when this ball collides with any GameObject.
        /// </summary>
        event Action OnCollide;

        /// <summary>
        /// Event fired when this ball collides with a Paddle GameObject.
        /// </summary>
        event Action OnCollideWithPaddle;

        /// <summary>
        /// The Rigidbody provider.
        /// </summary>
        IRigidbody Body { get; }

        /// <summary>
        /// The Transform provider.
        /// </summary>
        ITransform Transform { get; }

        /// <summary>
        /// The total hit count. This number goes up every time the ball hits any surface.
        /// </summary>
        int HitCount { get; }

        /// <summary>
        /// The current ball speed.
        /// </summary>
        float Speed { get; set; }

        /// <summary>
        /// The current ball direction.
        /// </summary>
        Vector2 Direction { get; }

        /// <summary>
        /// Enables the ball using the given initial speed.
        /// <para>It'll initialize ball with semi-random direction.</para>
        /// </summary>
        /// <param name="initialSpeed">The initial speed.</param>
        void Enable(float initialSpeed);

        /// <summary>
        /// Enters collision with the given transform provider.
        /// </summary>
        /// <param name="transform">The colliding Transform.</param>
        /// <param name="normal">The normal direction from where it hits.</param>
        void EnterCollision(ITransform transform, Vector3 normal);
    }
}