using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_EL2xxx
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
        public void Beckhoff_EL2014_0000_0017_01()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2014_0000_0017_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(101)]
        public void Beckhoff_EL2014_0000_0017_02()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2014_0000_0017_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(102)]
        public void Beckhoff_EL2014_0000_0017_03()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2014_0000_0017_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(103)]
        public void Beckhoff_EL2032_0000_0018_01()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2032_0000_0018_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(104)]
        public void Beckhoff_EL2034_0000_0018_01()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2034_0000_0018_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(105)]
        public void Beckhoff_EL2044_0000_0016_01()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2044_0000_0016_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(106)]
        public void Beckhoff_EL2044_0000_0016_02()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2044_0000_0016_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(107)]
        public void Beckhoff_EL2044_0000_0016_03()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2044_0000_0016_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(108)]
        public void Beckhoff_EL2809_0000_0018_01()
        {
            TestsCommon.Arrange("Beckhoff_EL2xxx\\Beckhoff_EL2809_0000_0018_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

