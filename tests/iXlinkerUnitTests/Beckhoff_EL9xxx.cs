using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_EL9xxx
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
        public void Beckhoff_EL1809_0000_0018_01()
        {
            TestsCommon.Arrange("Beckhoff_EL1xxx\\Beckhoff_EL1809_0000_0018_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }


    }
}

