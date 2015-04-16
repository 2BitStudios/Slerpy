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
            static void Clamp()
            {
                Assert::AreEqual(0.0f, Slerpy::Interpolate::Clamp(0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Interpolate::Clamp(0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::Clamp(1.0f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::Clamp(1.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::Clamp(2.0f, 1.0f), EPSILON);
            }

            [Test]
            static void PingPong()
            {
                Assert::AreEqual(0.0f, Slerpy::Interpolate::PingPong(0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Interpolate::PingPong(0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Interpolate::PingPong(1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Interpolate::PingPong(1.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::PingPong(2.0f, 1.0f), EPSILON);
            }

            [Test]
            static void Repeat()
            {
                Assert::AreEqual(0.0f, Slerpy::Interpolate::Repeat(0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Interpolate::Repeat(0.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Interpolate::Repeat(1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Interpolate::Repeat(1.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Interpolate::Repeat(1.75f, 1.0f), EPSILON);
            }
        };
    }
}