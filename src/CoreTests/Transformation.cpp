namespace Slerpy
{
    namespace Tests
    {
        using namespace NUnit::Framework;

        [TestFixture]
        public ref class Transformation
        {
        private:
            static float const EPSILON = 0.0001f;

        public:
            [Test]
            static void OffsetShake()
            {
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetShake(0.0f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Position.X, EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Transformation::OffsetShake(0.25f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Position.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetShake(0.5f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Position.X, EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Transformation::OffsetShake(0.75f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Position.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetShake(1.0f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Position.X, EPSILON);
            }
            
            [Test]
            static void OffsetTwist()
            {
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetTwist(0.0f, 2.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.0f).Rotation.X, EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Transformation::OffsetTwist(0.25f, 2.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.0f).Rotation.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetTwist(0.5f, 2.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.0f).Rotation.X, EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Transformation::OffsetTwist(0.75f, 2.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.0f).Rotation.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetTwist(1.0f, 2.0f, 0.0f, 0.0f, 180.0f, 0.0f, 0.0f).Rotation.X, EPSILON);
            }

            [Test]
            static void OffsetThrob()
            {
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetThrob(0.0f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Scale.X, EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Transformation::OffsetThrob(0.25f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Scale.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetThrob(0.5f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Scale.X, EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Transformation::OffsetThrob(0.75f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Scale.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetThrob(1.0f, 2.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f).Scale.X, EPSILON);
            }
        };
    }
}