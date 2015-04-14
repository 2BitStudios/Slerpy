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
                AxisWeightings axisWeightings(
                    Weighting(2.0f, 1.0f),
                    Weighting(0.0f, 0.0f),
                    Weighting(0.0f, 0.0f));

                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetShake(0.0f, axisWeightings).Position.X, EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Transformation::OffsetShake(0.25f, axisWeightings).Position.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetShake(0.5f, axisWeightings).Position.X, EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Transformation::OffsetShake(0.75f, axisWeightings).Position.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetShake(1.0f, axisWeightings).Position.X, EPSILON);
            }
            
            [Test]
            static void OffsetTwist()
            {
                AxisWeightings axisWeightings(
                    Weighting(2.0f, 180.0f),
                    Weighting(0.0f, 0.0f),
                    Weighting(0.0f, 0.0f));

                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetTwist(0.0f, axisWeightings).Rotation.X, EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Transformation::OffsetTwist(0.25f, axisWeightings).Rotation.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetTwist(0.5f, axisWeightings).Rotation.X, EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Transformation::OffsetTwist(0.75f, axisWeightings).Rotation.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetTwist(1.0f, axisWeightings).Rotation.X, EPSILON);
            }

            [Test]
            static void OffsetThrob()
            {
                AxisWeightings axisWeightings(
                    Weighting(2.0f, 1.0f),
                    Weighting(0.0f, 0.0f),
                    Weighting(0.0f, 0.0f));

                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetThrob(0.0f, axisWeightings).Scale.X, EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Transformation::OffsetThrob(0.25f, axisWeightings).Scale.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetThrob(0.5f, axisWeightings).Scale.X, EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Transformation::OffsetThrob(0.75f, axisWeightings).Scale.X, EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Transformation::OffsetThrob(1.0f, axisWeightings).Scale.X, EPSILON);
            }
        };
    }
}