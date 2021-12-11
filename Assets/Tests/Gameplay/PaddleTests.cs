using UnityEngine;
using NSubstitute;
using NUnit.Framework;

namespace WallBreaker.Gameplay
{
    public class PaddleTests
    {
        public class BasePaddle
        {
            public Paddle Paddle { get; private set; }

            public IRigidbody Body { get; private set; }
            public ICollider Collider { get; private set; }

            [SetUp]
            public void Setup()
            {
                Body = Substitute.For<IRigidbody>();
                Collider = Substitute.For<ICollider>();

                Paddle = new Paddle(Body, Collider);
            }
        }

        public class Speed : BasePaddle
        {
            [Test]
            public void Clamps_To_0_When_Set_Negative_Value()
            {
                Paddle.Speed = -1F;
                Assert.Zero(Paddle.Speed);
            }

            [Test]
            public void Clamps_To_MAX_SPEED_When_Set_Invalid_Positive_Value()
            {
                Paddle.Speed = Paddle.MAX_SPEED + 1F;
                Assert.AreEqual(expected: Paddle.MAX_SPEED, Paddle.Speed);
            }
        }

        public class Size : BasePaddle
        {
            [Test]
            public void Return_Size_From_Collider()
            {
                var expected = Vector3.one;
                Collider.Size.Returns(expected);

                var actual = Paddle.Size;

                Assert.AreEqual(expected, actual);
            }
        }

        public class Initialize : BasePaddle
        {
            [Test]
            public void Enable_Rigidbody_IsKinematic()
            {
                Paddle.Initialize();
                Assert.IsTrue(Body.IsKinematic);
            }
        }

        public class MoveHorizontally : BasePaddle
        {
            [Test]
            public void Clamps_Invalid_Input_To_Negative_1()
            {
                var input = -2;
                Paddle.Speed = 1;
                var expectedVelocity = Vector3.left * Paddle.Speed;

                Paddle.MoveHorizontally(input);

                Assert.AreEqual(expectedVelocity, Body.Velocity);
            }

            [Test]
            public void Clamps_Invalid_Input_To_Positive_1()
            {
                var input = 2;
                Paddle.Speed = 1;
                var expectedVelocity = Vector3.right * Paddle.Speed;

                Paddle.MoveHorizontally(input);

                Assert.AreEqual(expectedVelocity, Body.Velocity);
            }

            [Test]
            public void Sets_Velocity_When_Valid_Input()
            {
                var input = 1;
                Paddle.Speed = 1;
                var expectedVelocity = Vector3.right * Paddle.Speed;

                Paddle.MoveHorizontally(input);

                Assert.AreEqual(expectedVelocity, Body.Velocity);
            }

            [Test]
            public void Sets_Velocity_To_0_When_Input_Is_0()
            {
                var input = 0;
                Paddle.Speed = 1;
                var expectedVelocity = Vector3.zero;

                Paddle.MoveHorizontally(input);

                Assert.AreEqual(expectedVelocity, Body.Velocity);
            }
        }
    }
}