using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_EL4xxx
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
        public void Beckhoff_EL4004_0000_0020_01()
        {
            TestsCommon.Arrange("Beckhoff_EL4xxx\\Beckhoff_EL4004_0000_0020_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(101)]
        public void Beckhoff_EL4024_0000_0021_01()
        {
            TestsCommon.Arrange("Beckhoff_EL4xxx\\Beckhoff_EL4024_0000_0021_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

