using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_EL6xxx
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
        public void Beckhoff_EL6001_0000_0020_01()
        {
            //EL6001 default 22 data bytee (as array)
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6001_0000_0020_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(101)]
        public void Beckhoff_EL6001_0000_0020_02()
        {
            //EL6001 extended 50 data words (as array)
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6001_0000_0020_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(102)]
        public void Beckhoff_EL6001_0000_0020_03()
        {
            //EL6001 legacy 22 data bytes (as array)
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6001_0000_0020_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(103)]
        public void Beckhoff_EL6001_0000_0020_04()
        {
            //EL6001 legacy 3 data bytes (as array)
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6001_0000_0020_04");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(104)]
        public void Beckhoff_EL6001_0000_0020_05()
        {
            //EL6001 legacy 5 data bytes (as array)
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6001_0000_0020_05");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(110)]
        public void Beckhoff_EL6002_0000_0019_01()
        {
            //EL6002 default 22 data bytes (as array) both channel 
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6002_0000_0019_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(111)]
        public void Beckhoff_EL6002_0000_0019_02()
        {
            //EL6002 legacy 22 data bytes (as array) both channel 
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6002_0000_0019_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(112)]
        public void Beckhoff_EL6002_0000_0019_03()
        {
            //EL6002 different configuration for channel 1 and 2
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6002_0000_0019_03");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(120)]
        public void Beckhoff_EL6224_0000_0021_01()
        {
            //EL6224 IOlink master without any IO link slaves
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6224_0000_0021_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }

        [Test, Order(121)]
        public void Beckhoff_EL6224_0000_0021_02()
        {
            //EL6224 IOlink master with 4 BALLUFF BIS M-458-045-001-07-S4
            TestsCommon.Arrange("Beckhoff_EL6xxx\\Beckhoff_EL6224_0000_0021_02");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

