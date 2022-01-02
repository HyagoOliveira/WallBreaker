using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace WallBreaker.Gameplay
{
    public class BallTests
    {
        public class BaseBallTests
        {
            public Ball Ball { get; private set; }

            public IRandom Random { get; private set; }
            public IRigidbody Body { get; private set; }
            public ITransform Transform { get; private set; }

            [SetUp]
            public void Setup()
            {
                Random = Substitute.For<IRandom>();
                Body = Substitute.For<IRigidbody>();
                Transform = Substitute.For<ITransform>();

                Ball = new Ball(Random, Body, Transform);
            }
        }

        public class Speed : BaseBallTests
        {
            [Test]
            public void Clamps_To_0_When_Set_Negative_Value()
            {
                Ball.Speed = -1F;
                Assert.Zero(Ball.Speed);
            }

            [Test]
            public void Clamps_To_MAX_SPEED_When_Set_Invalid_Positive_Value()
            {
                Ball.Speed = Ball.MAX_SPEED + 1F;
                Assert.AreEqual(expected: Ball.MAX_SPEED, Ball.Speed);
            }
        }

        public class Enable : BaseBallTests
        {
            [Test]
            public void Uses_RANDOM_ROTATION()
            {
                Ball.Enable(default);
                Random.Received().Range(-Ball.RANDOM_ROTATION, Ball.RANDOM_ROTATION);
            }

            [Test]
            public void Sets_Speed_Using_Given_InitialSpeed()
            {
                var expected = 1F;
                Ball.Enable(expected);
                Assert.AreEqual(expected, Ball.Speed);
            }

            [Test]
            public void Sets_Direction_Using_Random_Rotation()
            {
                var rotation = 1F;
                Vector2 expected = Quaternion.Euler(0.0f, 0.0f, rotation) * Vector2.down;
                Random.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(rotation);

                Ball.Enable(default);

                Assert.AreEqual(expected, Ball.Direction);
            }

            [Test]
            public void Sets_Velocity_Using_Direction_And_Speed()
            {
                var speed = 1F;
                var rotation = 1F;
                var direction = Quaternion.Euler(0.0f, 0.0f, rotation) * Vector2.down;
                var expected = direction * speed;
                Random.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(rotation);

                Ball.Enable(speed);

                Assert.AreEqual(expected, Ball.Body.Velocity);
            }
        }

        public class EnterCollision : BaseBallTests
        {
            [Test]
            public void Increases_HitCount_By_1()
            {
                var increment = 1F;
                var expected = Ball.HitCount + increment;
                var transform = Substitute.For<ITransform>();
                transform.GameObject.Returns(new GameObject());

                Ball.EnterCollision(transform, default);

                Assert.AreEqual(expected, Ball.HitCount);
            }

            [Test]
            public void Raises_OnCollide()
            {
                var transform = Substitute.For<ITransform>();
                transform.GameObject.Returns(new GameObject());
                var raised = false;
                Ball.OnCollide += () => raised = true;

                Ball.EnterCollision(transform, default);

                Assert.IsTrue(raised);
            }

            [Test]
            public void Raises_OnCollideWithPaddle_When_Colliding_With_Paddle()
            {
                var gameObject = new GameObject { tag = Tags.PADDLE };
                var transform = Substitute.For<ITransform>();
                transform.GameObject.Returns(gameObject);
                var raised = false;
                Ball.OnCollideWithPaddle += () => raised = true;

                Ball.EnterCollision(transform, default);

                Assert.IsTrue(raised);
            }

            [Test]
            public void Doesnt_Raise_OnCollideWithPaddle_When_Not_Colliding_With_Paddle()
            {
                var gameObject = new GameObject { tag = "Player" };
                var transform = Substitute.For<ITransform>();
                transform.GameObject.Returns(gameObject);
                var raised = false;
                Ball.OnCollideWithPaddle += () => raised = true;

                Ball.EnterCollision(transform, default);

                Assert.IsFalse(raised);
            }

            [Test]
            public void Reflects_Direction_Using_Given_Normal()
            {
                var normal = Vector2.up;
                Vector2 expected = Vector3.Reflect(Ball.Direction, normal);
                var transform = Substitute.For<ITransform>();
                transform.GameObject.Returns(new GameObject());

                Ball.EnterCollision(transform, normal);

                Assert.AreEqual(expected, Ball.Direction);
            }

            [Test]
            public void Sets_Velocity_Using_Direction_And_Speed()
            {
                var normal = Vector2.up;
                var direction = Vector3.Reflect(Ball.Direction, normal);
                var expected = direction * Ball.Speed;
                var transform = Substitute.For<ITransform>();
                transform.GameObject.Returns(new GameObject());

                Ball.EnterCollision(transform, normal);

                Assert.AreEqual(expected, Ball.Body.Velocity);
            }
        }
    }
}