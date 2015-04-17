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
            static void FromTime()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::Clamp, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::Clamp, 0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(TimeWrapType::Clamp, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(TimeWrapType::Clamp, 1.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(TimeWrapType::Clamp, 2.0f, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::PingPong, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::PingPong, 0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(TimeWrapType::PingPong, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::PingPong, 1.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::PingPong, 2.0f, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::Repeat, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::Repeat, 0.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::Repeat, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::Repeat, 1.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(TimeWrapType::Repeat, 1.75f, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 1.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 2.0f, 1.0f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 2.5f, 1.0f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 3.0f, 1.0f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 3.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(TimeWrapType::Cycle, 4.0f, 1.0f), EPSILON);
            }

            [Test]
            static void Linear()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Linear, 0.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::WithType(WeightType::Linear, 0.25f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::WithType(WeightType::Linear, 0.5f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::WithType(WeightType::Linear, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Linear, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Linear, -0.0f), EPSILON);
                Assert::AreEqual(-0.25f, Slerpy::Weight::WithType(WeightType::Linear, -0.25f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::WithType(WeightType::Linear, -0.5f), EPSILON);
                Assert::AreEqual(-0.75f, Slerpy::Weight::WithType(WeightType::Linear, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Linear, -1.0f), EPSILON);
            }

            [Test]
            static void Heavy()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Heavy, 0.0f), EPSILON);
                Assert::AreEqual(0.0625f, Slerpy::Weight::WithType(WeightType::Heavy, 0.25f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::WithType(WeightType::Heavy, 0.5f), EPSILON);
                Assert::AreEqual(0.5625f, Slerpy::Weight::WithType(WeightType::Heavy, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Heavy, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Heavy, -0.0f), EPSILON);
                Assert::AreEqual(-0.0625f, Slerpy::Weight::WithType(WeightType::Heavy, -0.25f), EPSILON);
                Assert::AreEqual(-0.25f, Slerpy::Weight::WithType(WeightType::Heavy, -0.5f), EPSILON);
                Assert::AreEqual(-0.5625f, Slerpy::Weight::WithType(WeightType::Heavy, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Heavy, -1.0f), EPSILON);
            }

            [Test]
            static void Inverted()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Inverted, 0.0f), EPSILON);
                Assert::AreEqual(-0.25f, Slerpy::Weight::WithType(WeightType::Inverted, 0.25f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::WithType(WeightType::Inverted, 0.5f), EPSILON);
                Assert::AreEqual(-0.75f, Slerpy::Weight::WithType(WeightType::Inverted, 0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Inverted, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Inverted, -0.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::WithType(WeightType::Inverted, -0.25f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::WithType(WeightType::Inverted, -0.5f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::WithType(WeightType::Inverted, -0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Inverted, -1.0f), EPSILON);
            }

            [Test]
            static void Exaggerated()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.0f), EPSILON);
                Assert::AreEqual(0.3125f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.25f), EPSILON);
                Assert::AreEqual(0.625f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.5f), EPSILON);
                Assert::AreEqual(0.9375f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.75f), EPSILON);
                Assert::AreEqual(1.0625f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.85f), EPSILON);
                Assert::AreEqual(1.0625f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.95f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Exaggerated, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.0f), EPSILON);
                Assert::AreEqual(-0.3125f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.25f), EPSILON);
                Assert::AreEqual(-0.625f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.5f), EPSILON);
                Assert::AreEqual(-0.9375f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.75f), EPSILON);
                Assert::AreEqual(-1.0625f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.85f), EPSILON);
                Assert::AreEqual(-1.0625f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.95f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Exaggerated, -1.0f), EPSILON);
            }
        };
    }
}