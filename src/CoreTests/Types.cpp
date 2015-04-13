using namespace System;

namespace Slerpy
{
    namespace Tests
    {
        using namespace NUnit::Framework;

        [TestFixture]
        public ref class Types
        {
        private:
            static float const EPSILON = 0.0001f;

        public:
            [Test]
            static void Vector3D_Lerp()
            {
                Vector3D v1 = Vector3D(2.0f, 4.0f, 6.0f);
                Vector3D v2 = Vector3D(-2.0f, -4.0f, -6.0f);

                Vector3D v3 = Vector3D::Lerp(v1, v2, 0.75f);

                Assert::AreEqual(-1.0f, v3.X, EPSILON);
                Assert::AreEqual(-2.0f, v3.Y, EPSILON);
                Assert::AreEqual(-3.0f, v3.Z, EPSILON);
            }

            [Test]
            static void Vector3D_AdditionOperator()
            {
                Vector3D v1 = Vector3D(1.0f, 2.0f, 3.0f);
                Vector3D v2 = Vector3D(-2.0f, -4.0f, -6.0f);

                Vector3D v3 = v1 + v2;

                Assert::AreEqual(-1.0f, v3.X, EPSILON);
                Assert::AreEqual(-2.0f, v3.Y, EPSILON);
                Assert::AreEqual(-3.0f, v3.Z, EPSILON);
            }

            [Test]
            static void Vector3D_SubtractionOperator()
            {
                Vector3D v1 = Vector3D(-1.0f, -2.0f, -3.0f);
                Vector3D v2 = Vector3D(-2.0f, -4.0f, -6.0f);

                Vector3D v3 = v1 - v2;

                Assert::AreEqual(1.0f, v3.X, EPSILON);
                Assert::AreEqual(2.0f, v3.Y, EPSILON);
                Assert::AreEqual(3.0f, v3.Z, EPSILON);
            }

            [Test]
            static void Quaternion_Lerp()
            {
                Quaternion q1 = Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
                Quaternion q2 = Quaternion(0.0f, 1.0f, 0.0f, 0.0f);
                
                Quaternion q3 = Quaternion::Lerp(q1, q2, 0.75f);

                Assert::AreEqual(0.3162278f, q3.X, EPSILON);
                Assert::AreEqual(0.9486833f, q3.Y, EPSILON);
                Assert::AreEqual(0.0f, q3.Z, EPSILON);
                Assert::AreEqual(0.0f, q3.W, EPSILON);
            }

            [Test]
            static void Quaternion_FromEuler()
            {
                Quaternion q1 = Quaternion::FromEuler(0.0f, 180.0f, 0.0f);

                Assert::AreEqual(0.0f, q1.X, EPSILON);
                Assert::AreEqual(1.0f, q1.Y, EPSILON);
                Assert::AreEqual(0.0f, q1.Z, EPSILON);
                Assert::AreEqual(0.0f, q1.W, EPSILON);
            }

            [Test]
            static void Quaternion_Normalize()
            {
                Quaternion q1 = Quaternion(1.0f, 1.0f, 1.0f, 1.0f);

                q1.Normalize();

                Assert::AreEqual(0.5f, q1.X, EPSILON);
                Assert::AreEqual(0.5f, q1.Y, EPSILON);
                Assert::AreEqual(0.5f, q1.Z, EPSILON);
                Assert::AreEqual(0.5f, q1.W, EPSILON);
            }

            [Test]
            static void Quaternion_MultiplicationOperator()
            {
                Quaternion q1 = Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
                Quaternion q2 = Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

                Quaternion q3 = q1 * q2;

                Assert::AreEqual(0.0f, q3.X, EPSILON);
                Assert::AreEqual(0.0f, q3.Y, EPSILON);
                Assert::AreEqual(1.0f, q3.Z, EPSILON);
                Assert::AreEqual(0.0f, q3.W, EPSILON);
            }
        };
    }
}
