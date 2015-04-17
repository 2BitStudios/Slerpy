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
            static void Standard()
            {
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::WithType(InterpolateType::Standard, 1.0f, 3.0f, -1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::WithType(InterpolateType::Standard, 1.0f, 3.0f, 0.0f), EPSILON);
                Assert::AreEqual(2.0f, Slerpy::Interpolate::WithType(InterpolateType::Standard, 1.0f, 3.0f, 0.5f), EPSILON);
                Assert::AreEqual(3.0f, Slerpy::Interpolate::WithType(InterpolateType::Standard, 1.0f, 3.0f, 1.0f), EPSILON);
                Assert::AreEqual(5.0f, Slerpy::Interpolate::WithType(InterpolateType::Standard, 1.0f, 3.0f, 2.0f), EPSILON);
            }

            [Test]
            static void Clamped()
            {
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::WithType(InterpolateType::Clamped, 1.0f, 3.0f, -2.0f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Interpolate::WithType(InterpolateType::Clamped, 1.0f, 3.0f, -1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::WithType(InterpolateType::Clamped, 1.0f, 3.0f, 0.0f), EPSILON);
                Assert::AreEqual(2.0f, Slerpy::Interpolate::WithType(InterpolateType::Clamped, 1.0f, 3.0f, 0.5f), EPSILON);
                Assert::AreEqual(3.0f, Slerpy::Interpolate::WithType(InterpolateType::Clamped, 1.0f, 3.0f, 1.0f), EPSILON);
                Assert::AreEqual(3.0f, Slerpy::Interpolate::WithType(InterpolateType::Clamped, 1.0f, 3.0f, 2.0f), EPSILON);
            }
        };
    }
}