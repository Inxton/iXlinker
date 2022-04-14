using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_EK11xx
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
        public void Beckhoff_EK1100_0000_0018_01()
        {
            TestsCommon.Arrange("Beckhoff_EK11xx\\Beckhoff_EK1100_0000_0018_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(101)]
        public void Beckhoff_EK1122_0000_0018_01()
        {
            TestsCommon.Arrange("Beckhoff_EK11xx\\Beckhoff_EK1122_0000_0018_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

