#define MATH_PI (float)Math::PI

namespace Slerpy
{
    namespace Tests
    {
        using namespace System;

        using namespace NUnit::Framework;

        [TestFixture]
        public ref class Interpolate
        {
        private:
            static float const EPSILON = 0.0001f;

        public:
            [Test]
            static void LinearStandard()
            {
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Standard, 1.0f, 3.0f, -1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Standard, 1.0f, 3.0f, 0.0f), EPSILON);
                Assert::AreEqual(2.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Standard, 1.0f, 3.0f, 0.5f), EPSILON);
                Assert::AreEqual(3.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Standard, 1.0f, 3.0f, 1.0f), EPSILON);
                Assert::AreEqual(5.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Standard, 1.0f, 3.0f, 2.0f), EPSILON);
            }

            [Test]
            static void LinearClamped()
            {
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Clamped, 1.0f, 3.0f, -2.0f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Clamped, 1.0f, 3.0f, -1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Clamped, 1.0f, 3.0f, 0.0f), EPSILON);
                Assert::AreEqual(2.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Clamped, 1.0f, 3.0f, 0.5f), EPSILON);
                Assert::AreEqual(3.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Clamped, 1.0f, 3.0f, 1.0f), EPSILON);
                Assert::AreEqual(3.0f, Slerpy::Interpolate::LinearWithType(InterpolateType::Clamped, 1.0f, 3.0f, 2.0f), EPSILON);
            }

            [Test]
            static void SphericalStandard()
            {
                Assert::AreEqual(1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, -1.0f, MATH_PI), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.0f, MATH_PI), EPSILON);
                Assert::AreEqual(-1.0227f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.1f, MATH_PI), EPSILON);
                Assert::AreEqual(-0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.25f, MATH_PI), EPSILON);
                Assert::AreEqual(-0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.4f, MATH_PI), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.5f, MATH_PI), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.6f, MATH_PI), EPSILON);
                Assert::AreEqual(0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.75f, MATH_PI), EPSILON);
                Assert::AreEqual(0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 0.9f, MATH_PI), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 1.0f, MATH_PI), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Standard, -1.0f, 1.0f, 2.0f, MATH_PI), EPSILON);
            }

            [Test]
            static void SphericalClamped()
            {
                Assert::AreEqual(1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -2.0f, MATH_PI), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -1.0f, MATH_PI), EPSILON);
                Assert::AreEqual(1.0227f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.9f, MATH_PI), EPSILON);
                Assert::AreEqual(0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.75f, MATH_PI), EPSILON);
                Assert::AreEqual(0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.6f, MATH_PI), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.5f, MATH_PI), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.4f, MATH_PI), EPSILON);
                Assert::AreEqual(-0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.25f, MATH_PI), EPSILON);
                Assert::AreEqual(-1.0227f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, -0.1f, MATH_PI), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.0f, MATH_PI), EPSILON);
                Assert::AreEqual(-1.0227f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.1f, MATH_PI), EPSILON);
                Assert::AreEqual(-0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.25f, MATH_PI), EPSILON);
                Assert::AreEqual(-0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.4f, MATH_PI), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.5f, MATH_PI), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.6f, MATH_PI), EPSILON);
                Assert::AreEqual(0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.75f, MATH_PI), EPSILON);
                Assert::AreEqual(0.6818f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 0.9f, MATH_PI), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 1.0f, MATH_PI), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::SphericalWithType(InterpolateType::Clamped, -1.0f, 1.0f, 2.0f, MATH_PI), EPSILON);
            }
        };
    }
}

#undef MATH_PI