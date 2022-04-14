
using NUnit.Framework;

namespace iXlinkerUnitTests
{
    public class Beckhoff_AMP8xxx
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
        public void Beckhoff_AMP8000_0030_0103_0103_01()
        {
            //There are eight PDO entry items with the same name "Std Input"
            //in the module "Device (Safety)", PDO "Standard Inputdata" PDO entry struct "FSOE"
            //This items are renamed to Std_Input,Std_Input_1,..Std_Input_7 and groupped into the structure "FSOE"
            TestsCommon.Arrange("Beckhoff_AMP8xxx\\Beckhoff_AMP8000_0030_0103_0103_01");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
        }
    }
}

