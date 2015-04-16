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
            static void Inverted()
            {
                Assert::AreEqual(1.0f, Slerpy::Weight::Inverted(0.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::Inverted(0.25f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::Inverted(0.5f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::Inverted(0.75f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::Inverted(1.0f), EPSILON);
            }

            [Test]
            static void Exaggerated()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::Exaggerated(0.0f), EPSILON);
                Assert::AreEqual(0.3125f, Slerpy::Weight::Exaggerated(0.25f), EPSILON);
                Assert::AreEqual(0.625f, Slerpy::Weight::Exaggerated(0.5f), EPSILON);
                Assert::AreEqual(0.9375f, Slerpy::Weight::Exaggerated(0.75f), EPSILON);
                Assert::AreEqual(1.0625f, Slerpy::Weight::Exaggerated(0.85f), EPSILON);
                Assert::AreEqual(0.95f + 0.05625f, Slerpy::Weight::Exaggerated(0.95f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::Exaggerated(1.0f), EPSILON);
            }
        };
    }
}