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

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Linear, 0.0f), EPSILON);
                Assert::AreEqual(-0.25f, Slerpy::Weight::WithType(WeightType::Linear, -0.25f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::WithType(WeightType::Linear, -0.5f), EPSILON);
                Assert::AreEqual(-0.75f, Slerpy::Weight::WithType(WeightType::Linear, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Linear, -1.0f), EPSILON);
            }

            [Test]
            static void Eager()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Eager, 0.0f), EPSILON);
                Assert::AreEqual(0.4036f, Slerpy::Weight::WithType(WeightType::Eager, 0.25f), EPSILON);
                Assert::AreEqual(0.6610f, Slerpy::Weight::WithType(WeightType::Eager, 0.5f), EPSILON);
                Assert::AreEqual(0.8502f, Slerpy::Weight::WithType(WeightType::Eager, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Eager, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Eager, 0.0f), EPSILON);
                Assert::AreEqual(-0.4036f, Slerpy::Weight::WithType(WeightType::Eager, -0.25f), EPSILON);
                Assert::AreEqual(-0.6610f, Slerpy::Weight::WithType(WeightType::Eager, -0.5f), EPSILON);
                Assert::AreEqual(-0.8502f, Slerpy::Weight::WithType(WeightType::Eager, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Eager, -1.0f), EPSILON);
            }

            [Test]
            static void Heavy()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Heavy, 0.0f), EPSILON);
                Assert::AreEqual(0.0625f, Slerpy::Weight::WithType(WeightType::Heavy, 0.25f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::WithType(WeightType::Heavy, 0.5f), EPSILON);
                Assert::AreEqual(0.5625f, Slerpy::Weight::WithType(WeightType::Heavy, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Heavy, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Heavy, 0.0f), EPSILON);
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

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Inverted, 0.0f), EPSILON);
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

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Exaggerated, 0.0f), EPSILON);
                Assert::AreEqual(-0.3125f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.25f), EPSILON);
                Assert::AreEqual(-0.625f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.5f), EPSILON);
                Assert::AreEqual(-0.9375f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.75f), EPSILON);
                Assert::AreEqual(-1.0625f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.85f), EPSILON);
                Assert::AreEqual(-1.0625f, Slerpy::Weight::WithType(WeightType::Exaggerated, -0.95f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Exaggerated, -1.0f), EPSILON);
            }

            [Test]
            static void StickyLow()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::StickyLow, 0.0f), EPSILON);
                Assert::AreEqual(0.0625f, Slerpy::Weight::WithType(WeightType::StickyLow, 0.25f), EPSILON);
                Assert::AreEqual(0.375f, Slerpy::Weight::WithType(WeightType::StickyLow, 0.5f), EPSILON);
                Assert::AreEqual(0.6875f, Slerpy::Weight::WithType(WeightType::StickyLow, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::StickyLow, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::StickyLow, 0.0f), EPSILON);
                Assert::AreEqual(-0.0625f, Slerpy::Weight::WithType(WeightType::StickyLow, -0.25f), EPSILON);
                Assert::AreEqual(-0.375f, Slerpy::Weight::WithType(WeightType::StickyLow, -0.5f), EPSILON);
                Assert::AreEqual(-0.6875f, Slerpy::Weight::WithType(WeightType::StickyLow, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::StickyLow, -1.0f), EPSILON);
            }

            [Test]
            static void StickyHigh()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::StickyHigh, 0.0f), EPSILON);
                Assert::AreEqual(0.3125f, Slerpy::Weight::WithType(WeightType::StickyHigh, 0.25f), EPSILON);
                Assert::AreEqual(0.625f, Slerpy::Weight::WithType(WeightType::StickyHigh, 0.5f), EPSILON);
                Assert::AreEqual(0.9375f, Slerpy::Weight::WithType(WeightType::StickyHigh, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::StickyHigh, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::StickyHigh, 0.0f), EPSILON);
                Assert::AreEqual(-0.3125f, Slerpy::Weight::WithType(WeightType::StickyHigh, -0.25f), EPSILON);
                Assert::AreEqual(-0.625f, Slerpy::Weight::WithType(WeightType::StickyHigh, -0.5f), EPSILON);
                Assert::AreEqual(-0.9375f, Slerpy::Weight::WithType(WeightType::StickyHigh, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::StickyHigh, -1.0f), EPSILON);
            }

            [Test]
            static void Snap()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Snap, 0.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Snap, 0.25f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Snap, 0.5f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Snap, 0.75f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Snap, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Snap, 0.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Snap, -0.25f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Snap, -0.5f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Snap, -0.75f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Snap, -1.0f), EPSILON);
            }

            [Test]
            static void Elastic()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Elastic, 0.0f), EPSILON);
                Assert::AreEqual(0.4166f, Slerpy::Weight::WithType(WeightType::Elastic, 0.25f), EPSILON);
                Assert::AreEqual(0.8333f, Slerpy::Weight::WithType(WeightType::Elastic, 0.5f), EPSILON);
                Assert::AreEqual(1.0562f, Slerpy::Weight::WithType(WeightType::Elastic, 0.7f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Elastic, 0.8f), EPSILON);
                Assert::AreEqual(0.9937f, Slerpy::Weight::WithType(WeightType::Elastic, 0.9f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Elastic, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Elastic, 0.0f), EPSILON);
                Assert::AreEqual(-0.4166f, Slerpy::Weight::WithType(WeightType::Elastic, -0.25f), EPSILON);
                Assert::AreEqual(-0.8333f, Slerpy::Weight::WithType(WeightType::Elastic, -0.5f), EPSILON);
                Assert::AreEqual(-1.0562f, Slerpy::Weight::WithType(WeightType::Elastic, -0.7f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Elastic, -0.8f), EPSILON);
                Assert::AreEqual(-0.9937f, Slerpy::Weight::WithType(WeightType::Elastic, -0.9f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Elastic, -1.0f), EPSILON);
            }
        };
    }
}