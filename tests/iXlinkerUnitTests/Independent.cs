using NUnit.Framework;
using iXlinkerDtos;
using iXlinkerTestHelper;
using System.IO;
using TwincatXmlSchemas.TcPlcObject;

namespace iXlinkerUnitTests
{
    public class Independent
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
        public void IndependentProjectFile()
        {
            TestsCommon.Arrange("Independent\\IndependentProjectFile");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\_Config\PLC\Plc.xti"));
        }

        [Test, Order(101)]
        public void DependentProjectFile()
        {
            TestsCommon.Arrange("Independent\\DependentProjectFile");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\_Config\PLC\Plc.xti"));

        }
    }
}