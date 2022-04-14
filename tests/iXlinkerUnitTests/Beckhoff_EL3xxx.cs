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

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestsCommon.OneTimeTearDown();
        }

        [Test, Order(100)]
        public void Beckhoff_EL3024_0000_0019_01()
        {
            //EL3024 all channels Standard-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3024_0000_0019_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(101)]
        public void Beckhoff_EL3024_0000_0019_02()
        {
            //EL3024 all channels Compact-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3024_0000_0019_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(102)]
        public void Beckhoff_EL3024_0000_0019_03()
        {
            //EL3024 mixed channel configuration
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3024_0000_0019_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(103)]
        public void Beckhoff_EL3064_0000_0020_01()
        {
            //EL3064 all channels Standard-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3064_0000_0020_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(104)]
        public void Beckhoff_EL3064_0000_0020_02()
        {
            //EL3064 all channels Compact-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3064_0000_0020_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(105)]
        public void Beckhoff_EL3064_0000_0020_03()
        {
            //EL3064 mixed channel configuration
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3064_0000_0020_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(106)]
        public void Beckhoff_EL3164_0000_0020_01()
        {
            //EL3164 all channels Standard-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3164_0000_0020_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(107)]
        public void Beckhoff_EL3164_0000_0020_02()
        {
            //EL3164 all channels Compact-DC-Synchron
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3164_0000_0020_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(108)]
        public void Beckhoff_EL3164_0000_0020_03()
        {
            //EL3164 mixed channel configuration
            TestsCommon.Arrange("Beckhoff_EL3xxx\\Beckhoff_EL3164_0000_0020_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

