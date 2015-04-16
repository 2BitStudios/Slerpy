namespace Slerpy
{
    namespace Tests
    {
        using namespace NUnit::Framework;

        [TestFixture]
        public ref class Weight
        {
        private:
            static float const EPSILON = 0.0001f;

        public:
            [Test]
            static void Linear()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::Linear(0.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::Linear(0.25f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::Linear(0.5f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::Linear(0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::Linear(1.0f), EPSILON);
            }

            [Test]
            static void Heavy()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::Heavy(0.0f), EPSILON);
                Assert::AreEqual(0.0625f, Slerpy::Weight::Heavy(0.25f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::Heavy(0.5f), EPSILON);
                Assert::AreEqual(0.5625f, Slerpy::Weight::Heavy(0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::Heavy(1.0f), EPSILON);
            }

            [Test]
            static void Invert()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::Invert(1.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::Invert(0.75f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::Invert(0.5f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::Invert(0.25f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::Invert(0.0f), EPSILON);
            }
        };
    }
}