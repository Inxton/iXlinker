using NUnit.Framework;
using iXlinkerDtos;
using iXlinkerTestHelper;
using System.IO;
using TwincatXmlSchemas.TcPlcObject;

namespace iXlinkerUnitTests
{
    public class HwBaseStructure
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
        public void PlcProjectWithoutReferenceToTcoIo()
        {
            TestsCommon.Arrange("HwBaseStructure\\PlcProjectWithoutReferenceToTcoIo");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());
            
            string[] boxStructures = Directory.GetFiles(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Boxes");
            foreach (string boxStructure in boxStructures)
            {
                if(!boxStructure.Equals(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Boxes\" + TcModel.etcSlaveBaseStructName + ".TcDUT"))
                {
                    Assert.AreEqual(TcModel.etcSlaveBaseStructName, TestsCommon.GetTypeFromWhichDtuExtends(boxStructure));
                }
            }
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Boxes\" + TcModel.etcSlaveBaseStructName + ".TcDUT"));
        }

        [Test, Order(101)]
        public void PlcProjectWithReferenceToTcoIo()
        {
            TestsCommon.Arrange("HwBaseStructure\\PlcProjectWithReferenceToTcoIo");
            TestsCommon.Act();
            Assert.IsTrue(TestsCommon.AllFilesAreEqual());

            string[] boxStructures = Directory.GetFiles(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Boxes");
            foreach (string boxStructure in boxStructures)
            {
                Assert.AreEqual(TcModel.plcLibContainingHwBaseName + "." + TcModel.etcSlaveBaseStructName, TestsCommon.GetTypeFromWhichDtuExtends(boxStructure));
            }
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Boxes\" + TcModel.etcSlaveBaseStructName + ".TcDUT"));
        }
    }
}