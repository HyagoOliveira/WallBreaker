using UnityEngine;

namespace WallBreaker.Gameplay
{
    /// <summary>
    /// Humble component class for <see cref="Ball"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class BallBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField, Min(0f)] private float initialSpeed = 6f;

        public IBall Ball { get; private set; }

        private void Reset()
        {
            body = GetComponent<Rigidbody2D>();
        }

        private void Awake() => InitializeBall();

        private void OnEnable() => Ball.Enable(initialSpeed);

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var hasContacts = collision.contactCount > 0;
            if (hasContacts)
            {
                var normal = collision.GetContact(0).normal;
                Ball.EnterCollision(new TransformAdapter(collision.transform), normal);
            }
        }

        public void InitializeBall()
        {
            Ball = new Ball(
                new RandomAdapter(),
                new Rigidbody2DAdapter(body),
                new TransformAdapter(transform)
            );
        }
    }
}