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
                if (boxStructure.StartsWith("EK"))
                {
                    Assert.AreEqual("EtcSlaveBoxBase_77A0E4A7", TestsCommon.GetTypeFromWhichDtuExtends(boxStructure));
                }
                else if (boxStructure.StartsWith("EL"))
                {
                    Assert.AreEqual("EtcSlaveTerminalBase_947E5A46", TestsCommon.GetTypeFromWhichDtuExtends(boxStructure));
                }
            }
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\InputBase_8311D824.TcDUT"));
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\OutputBase_10CEE7DE.TcDUT"));
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\EtcSlaveBase_82538BE2.TcDUT"));
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\EtcSlaveBoxBase_77A0E4A7.TcDUT"));
            Assert.IsTrue(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\EtcSlaveTerminalBase_947E5A46.TcDUT"));
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
                if (boxStructure.StartsWith("EK"))
                {
                    Assert.AreEqual("TcoIo.EtcSlaveBoxBase_77A0E4A7", TestsCommon.GetTypeFromWhichDtuExtends(boxStructure));
                }
                else if (boxStructure.StartsWith("EL"))
                {
                    Assert.AreEqual("TcoIo.EtcSlaveTerminalBase_947E5A46", TestsCommon.GetTypeFromWhichDtuExtends(boxStructure));
                }
            }
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\InputBase_8311D824.TcDUT"));
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\OutputBase_10CEE7DE.TcDUT"));
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\EtcSlaveBase_82538BE2.TcDUT"));
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\EtcSlaveBoxBase_77A0E4A7.TcDUT"));
            Assert.IsFalse(File.Exists(TestsCommon.generatedDir.FullName + @"\DUTs\IO\Base\EtcSlaveTerminalBase_947E5A46.TcDUT"));
        }
    }
}