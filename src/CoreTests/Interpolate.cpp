namespace Slerpy
{
    namespace Tests
    {
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
        };
    }
}