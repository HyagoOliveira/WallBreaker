using System;
using UnityEngine;

namespace WallBreaker.Gameplay
{
    public sealed class Ball : IBall
    {
        public event Action OnCollide;
        public event Action OnCollideWithPaddle;

        public const float MAX_SPEED = 100F;

        public IRigidbody Body => body;
        public ITransform Transform => transform;

        public int HitCount { get; private set; }

        public float Speed
        {
            get => speed;
            set => speed = Mathf.Clamp(value, 0F, MAX_SPEED);
        }

        public Vector2 Direction { get; private set; }

        private float speed;

        private readonly IRandom random;
        private readonly IRigidbody body;
        private readonly ITransform transform;

        public Ball(
            IRandom random,
            IRigidbody body,
            ITransform transform
        )
        {
            this.body = body;
            this.random = random;
            this.transform = transform;
            SetupBody();
        }

        public void Enable(float initialSpeed)
        {
            var rotation = random.Range(-30.0f, 30.0f);

            Speed = initialSpeed;
            Direction = Quaternion.Euler(0.0f, 0.0f, rotation) * Vector2.down;
            Body.Velocity = Direction * Speed;
        }

        public void EnterCollision(ITransform transform, Vector3 normal)
        {
            HitCount++;
            OnCollide?.Invoke();

            var gameObject = transform.GameObject;
            var isPaddleCollision = gameObject.CompareTag(Tags.PADDLE);
            if (isPaddleCollision) OnCollideWithPaddle?.Invoke();

            Direction = Vector3.Reflect(Direction, normal);
            Body.Velocity = Direction * Speed;
        }

        private void SetupBody()
        {
            Body.IsKinematic = true;
        }
    }
}