using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_EL3xxx
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestsCommon.OneTimeSetup();
        }

        [SetUp]
        public void Setup()
        {
            TestsCommon.Setup();
        }

        [Test, Order(100)]
        public void Beckhoff_EL3164_0000_0020_01()
        {
            //EL3164 all channels Standard-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3164_0000_0020_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(101)]
        public void Beckhoff_EL3164_0000_0020_02()
        {
            //EL3164 all channels Compact-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3164_0000_0020_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(102)]
        public void Beckhoff_EL3164_0000_0020_03()
        {
            //EL3164 mixed channel configuration
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3164_0000_0020_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

