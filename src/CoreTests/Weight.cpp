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
            static void FromValueInRange()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Clamp, 0.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Clamp, 0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Clamp, 1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Clamp, 1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::Clamp, 2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::Clamp, 2.5f, 0.5f, 2.0f), EPSILON);
                
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -4.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -3.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, -0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 0.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 3.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 4.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 4.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::PingPong, 5.0f, 0.5f, 2.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -4.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -3.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, -0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 0.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Repeat, 3.5f, 0.5f, 2.0f), EPSILON);

                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -4.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -3.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, -0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 0.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 3.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.3333f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 4.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-0.6666f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 4.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::FromValueInRange(WrapType::Cycle, 5.0f, 0.5f, 2.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 0.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 0.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 1.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 1.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 2.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.6666f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 2.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.3333f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 3.0f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 3.5f, 0.5f, 2.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 4.5f, 0.5f, 2.0f), EPSILON);
            }

            static void FromValueInRange_Metadata_Validate(WeightMetadata^ weightMetadata, int targetWrapCount, bool targetIsOnUpwardCurve)
            {
                Assert::AreEqual(targetWrapCount, weightMetadata->WrapCount);
                Assert::AreEqual(targetIsOnUpwardCurve, weightMetadata->IsOnUpwardCurve);
            }

            [Test]
            static void FromValueInRange_Metadata()
            {
                WeightMetadata^ weightMetadata = gcnew WeightMetadata();

                // CLAMP
                Slerpy::Weight::FromValueInRange(WrapType::Clamp, -2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Clamp, -1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Clamp, 0.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Clamp, 1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Clamp, 2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, false);

                Slerpy::Weight::FromValueInRange(WrapType::Clamp, 4.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, false);

                // PINGPONG
                Slerpy::Weight::FromValueInRange(WrapType::PingPong, -5.5f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -2, true);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, -3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -1, true);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, -2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, -1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, 0.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, 1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, 3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, 4.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, true);

                Slerpy::Weight::FromValueInRange(WrapType::PingPong, 6.5f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 2, true);

                // REPEAT
                Slerpy::Weight::FromValueInRange(WrapType::Repeat, -3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -2, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, -2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -1, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, -1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -1, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, 0.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, 1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, 2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, 3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, true);

                Slerpy::Weight::FromValueInRange(WrapType::Repeat, 4.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 2, true);

                // CYCLE
                Slerpy::Weight::FromValueInRange(WrapType::Cycle, -11.5f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -2, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, -6.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, -1, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, -5.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, -3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, -1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 0.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 5.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 6.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 7.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, true);

                Slerpy::Weight::FromValueInRange(WrapType::Cycle, 12.5f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 2, true);

                // MIRRORCLAMP
                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, -2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, -1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 0.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 1.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, true);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 2.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 3.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 0, false);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 4.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, false);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 5.5f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, false);

                Slerpy::Weight::FromValueInRange(WrapType::MirrorClamp, 7.0f, 0.5f, 2.0f, weightMetadata);
                FromValueInRange_Metadata_Validate(weightMetadata, 1, false);
            }

            [Test]
            static void FromTime()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Clamp, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(WrapType::Clamp, 0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(WrapType::Clamp, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(WrapType::Clamp, 1.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(WrapType::Clamp, 2.0f, 1.0f), EPSILON);

                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(WrapType::PingPong, -1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(WrapType::PingPong, -0.75f, 1.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromTime(WrapType::PingPong, -0.25f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::PingPong, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(WrapType::PingPong, 0.75f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(WrapType::PingPong, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(WrapType::PingPong, 1.25f, 1.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromTime(WrapType::PingPong, 1.75f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::PingPong, 2.0f, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Repeat, -1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromTime(WrapType::Repeat, -0.75f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(WrapType::Repeat, -0.25f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Repeat, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(WrapType::Repeat, 0.75f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Repeat, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromTime(WrapType::Repeat, 1.25f, 1.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromTime(WrapType::Repeat, 1.75f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Repeat, 2.0f, 1.0f), EPSILON);

                Assert::AreEqual(-1.0f, Slerpy::Weight::FromTime(WrapType::Cycle, -1.0f, 1.0f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::FromTime(WrapType::Cycle, -0.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Cycle, 0.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(WrapType::Cycle, 0.5f, 1.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromTime(WrapType::Cycle, 1.0f, 1.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromTime(WrapType::Cycle, 1.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Cycle, 2.0f, 1.0f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::FromTime(WrapType::Cycle, 2.5f, 1.0f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::FromTime(WrapType::Cycle, 3.0f, 1.0f), EPSILON);
                Assert::AreEqual(-0.5f, Slerpy::Weight::FromTime(WrapType::Cycle, 3.5f, 1.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromTime(WrapType::Cycle, 4.0f, 1.0f), EPSILON);
            }

            [Test]
            static void FromAngle()
            {
                Assert::AreEqual(0.75f, Slerpy::Weight::FromAngle(-270.0f, 135.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromAngle(-225.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromAngle(-180.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromAngle(-135.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromAngle(-90.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromAngle(-45.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromAngle(0.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromAngle(45.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromAngle(90.0f, 135.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromAngle(135.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromAngle(180.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromAngle(225.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromAngle(270.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.0f, Slerpy::Weight::FromAngle(315.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::FromAngle(360.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::FromAngle(405.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromAngle(450.0f, 135.0f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::FromAngle(495.0f, 135.0f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::FromAngle(540.0f, 135.0f), EPSILON);
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
                Assert::AreEqual(1.075f, Slerpy::Weight::WithType(WeightType::Elastic, 0.7f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Elastic, 0.8f), EPSILON);
                Assert::AreEqual(0.9749f, Slerpy::Weight::WithType(WeightType::Elastic, 0.9f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Elastic, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Elastic, 0.0f), EPSILON);
                Assert::AreEqual(-0.4166f, Slerpy::Weight::WithType(WeightType::Elastic, -0.25f), EPSILON);
                Assert::AreEqual(-0.8333f, Slerpy::Weight::WithType(WeightType::Elastic, -0.5f), EPSILON);
                Assert::AreEqual(-1.075f, Slerpy::Weight::WithType(WeightType::Elastic, -0.7f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Elastic, -0.8f), EPSILON);
                Assert::AreEqual(-0.9749f, Slerpy::Weight::WithType(WeightType::Elastic, -0.9f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Elastic, -1.0f), EPSILON);
            }

            [Test]
            static void Bounce()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Bounce, 0.0f), EPSILON);
                Assert::AreEqual(0.375f, Slerpy::Weight::WithType(WeightType::Bounce, 0.15f), EPSILON);
                Assert::AreEqual(0.625f, Slerpy::Weight::WithType(WeightType::Bounce, 0.25f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Bounce, 0.4f), EPSILON);
                Assert::AreEqual(0.85f, Slerpy::Weight::WithType(WeightType::Bounce, 0.45f), EPSILON);
                Assert::AreEqual(0.7401f, Slerpy::Weight::WithType(WeightType::Bounce, 0.5f), EPSILON);
                Assert::AreEqual(0.7f, Slerpy::Weight::WithType(WeightType::Bounce, 0.55f), EPSILON);
                Assert::AreEqual(0.7401f, Slerpy::Weight::WithType(WeightType::Bounce, 0.6f), EPSILON);
                Assert::AreEqual(0.85f, Slerpy::Weight::WithType(WeightType::Bounce, 0.65f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Bounce, 0.7f), EPSILON);
                Assert::AreEqual(0.8829f, Slerpy::Weight::WithType(WeightType::Bounce, 0.75f), EPSILON);
                Assert::AreEqual(0.8535f, Slerpy::Weight::WithType(WeightType::Bounce, 0.8f), EPSILON);
                Assert::AreEqual(0.9339f, Slerpy::Weight::WithType(WeightType::Bounce, 0.85f), EPSILON);
                Assert::AreEqual(0.942f, Slerpy::Weight::WithType(WeightType::Bounce, 0.9f), EPSILON);
                Assert::AreEqual(0.9047f, Slerpy::Weight::WithType(WeightType::Bounce, 0.95f), EPSILON);
                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::Bounce, 1.0f), EPSILON);

                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::Bounce, 0.0f), EPSILON);
                Assert::AreEqual(-0.375f, Slerpy::Weight::WithType(WeightType::Bounce, -0.15f), EPSILON);
                Assert::AreEqual(-0.625f, Slerpy::Weight::WithType(WeightType::Bounce, -0.25f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Bounce, -0.4f), EPSILON);
                Assert::AreEqual(-0.85f, Slerpy::Weight::WithType(WeightType::Bounce, -0.45f), EPSILON);
                Assert::AreEqual(-0.7401f, Slerpy::Weight::WithType(WeightType::Bounce, -0.5f), EPSILON);
                Assert::AreEqual(-0.7f, Slerpy::Weight::WithType(WeightType::Bounce, -0.55f), EPSILON);
                Assert::AreEqual(-0.7401f, Slerpy::Weight::WithType(WeightType::Bounce, -0.6f), EPSILON);
                Assert::AreEqual(-0.85f, Slerpy::Weight::WithType(WeightType::Bounce, -0.65f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Bounce, -0.7f), EPSILON);
                Assert::AreEqual(-0.8829f, Slerpy::Weight::WithType(WeightType::Bounce, -0.75f), EPSILON);
                Assert::AreEqual(-0.8535f, Slerpy::Weight::WithType(WeightType::Bounce, -0.8f), EPSILON);
                Assert::AreEqual(-0.9339f, Slerpy::Weight::WithType(WeightType::Bounce, -0.85f), EPSILON);
                Assert::AreEqual(-0.942f, Slerpy::Weight::WithType(WeightType::Bounce, -0.9f), EPSILON);
                Assert::AreEqual(-0.9047f, Slerpy::Weight::WithType(WeightType::Bounce, -0.95f), EPSILON);
                Assert::AreEqual(-1.0f, Slerpy::Weight::WithType(WeightType::Bounce, -1.0f), EPSILON);
            }

            [Test]
            static void OneMinus()
            {
                Assert::AreEqual(0.0f, Slerpy::Weight::WithType(WeightType::OneMinus, 1.0f), EPSILON);
                Assert::AreEqual(0.25f, Slerpy::Weight::WithType(WeightType::OneMinus, 0.75f), EPSILON);
                Assert::AreEqual(0.5f, Slerpy::Weight::WithType(WeightType::OneMinus, 0.5f), EPSILON);
                Assert::AreEqual(0.75f, Slerpy::Weight::WithType(WeightType::OneMinus, 0.25f), EPSILON);

                Assert::AreEqual(1.0f, Slerpy::Weight::WithType(WeightType::OneMinus, 0.0f), EPSILON);

                Assert::AreEqual(1.25f, Slerpy::Weight::WithType(WeightType::OneMinus, -0.25f), EPSILON);
                Assert::AreEqual(1.5f, Slerpy::Weight::WithType(WeightType::OneMinus, -0.5f), EPSILON);
                Assert::AreEqual(1.75f, Slerpy::Weight::WithType(WeightType::OneMinus, -0.75f), EPSILON);
                Assert::AreEqual(2.0f, Slerpy::Weight::WithType(WeightType::OneMinus, -1.0f), EPSILON);
            }
        };
    }
}