using iXlinker.TsprojFile.Mapping;
using iXlinker.Utils;
using iXlinkerDtos;
using iXlinkerTestHelper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using TsprojFile.Scan;
using TwincatXmlSchemas.TcPlcProj;
using TwincatXmlSchemas.TcSmProject;

namespace iXlinkerIntegrationTests
{
    public class Integration
    {
        internal static string SourcePath { get; private set; }
        internal static DirectoryInfo expectedDir { get; private set; }
        internal static DirectoryInfo generatedDir { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestsCommon.OneTimeSetup();
            SourcePath = TestsCommon.SourcePath;
            expectedDir = TestsCommon.expectedDir;
            generatedDir = TestsCommon.generatedDir;
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

        [Test, Order(101)]
        public void Beckhoff_AMIxxxx()
        {
            string TestCaseFolder = "Beckhoff_AMIxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(102)]
        public void Beckhoff_APSxxxx()
        {
            string TestCaseFolder = "Beckhoff_APSxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(103)]
        public void Beckhoff_ATxxxx()
        {
            string TestCaseFolder = "Beckhoff_ATxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(104)]
        public void Beckhoff_BKCUCX()
        {
            string TestCaseFolder = "Beckhoff_BKCUCX";
            TestRun(TestCaseFolder);
        }

        [Test, Order(105)]
        public void Beckhoff_Duplicates()
        {
            string TestCaseFolder = "Beckhoff_Duplicates";
            TestRun(TestCaseFolder);
        }

        [Test, Order(106)]
        public void Beckhoff_EJxxxx()
        {
            string TestCaseFolder = "Beckhoff_EJxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(107)]
        public void Beckhoff_EKxxxx()
        {
            string TestCaseFolder = "Beckhoff_EKxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(108)]
        public void Beckhoff_ELMxxxx()
        {
            string TestCaseFolder = "Beckhoff_ELMxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(109)]
        public void Beckhoff_ELxxxx()
        {
            string TestCaseFolder = "Beckhoff_ELxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(110)]
        public void Beckhoff_ELXxxxx()
        {
            string TestCaseFolder = "Beckhoff_ELXxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(111)]
        public void Beckhoff_EMxxxx()
        {
            string TestCaseFolder = "Beckhoff_EMxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(112)]
        public void Beckhoff_EPPxxxx()
        {
            string TestCaseFolder = "Beckhoff_EPPxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(113)]
        public void Beckhoff_EPxxxx()
        {
            string TestCaseFolder = "Beckhoff_EPxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(114)]
        public void Beckhoff_EQxxxx()
        {
            string TestCaseFolder = "Beckhoff_EQxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(115)]
        public void Beckhoff_ERxxxx()
        {
            string TestCaseFolder = "Beckhoff_ERxxxx";
            TestRun(TestCaseFolder);
        }

        [Test, Order(116)]
        public void Beckhoff_KLxxxx()
        {
            string TestCaseFolder = "Beckhoff_KLxxxx";
            TestRun(TestCaseFolder);
        }

        internal static void TestRun(string testCaseFolder)
        {
            CopyTestFiles(@$"{SourcePath}\{testCaseFolder}");
            string TsProjFilePath = @$"{generatedDir.FullName}\Ts\Ts.tsproj";
            string ActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            string PlcProjFilePath = @$"{generatedDir.FullName}\Ts\PLC\PLC.plcproj";

            Solution vs = VS.GetXaeProjectDetails(TsProjFilePath, ActiveTargetPlatform, PlcProjFilePath, true, "", 25);
            ScanTcProjFile tcProj = new ScanTcProjFile();
            tcProj.SearchDevices(vs);
            tcProj.GenerateStructures(vs);
            tcProj.GenerateMappingsToTsProj(vs);


            Assert.IsTrue(TestsCommon.AreFilesEqual(TsProjFilePath, @$"{expectedDir.FullName}\Ts\Ts.tsproj"));
            Assert.IsTrue(TestsCommon.AreFilesEqual(PlcProjFilePath, @$"{expectedDir.FullName}\Ts\PLC\PLC.plcproj"));

            Assert.IsTrue(TestsCommon.AllFilesAreEqual(@$"{expectedDir.FullName}\Ts\PLC\DUTs\IO\", @$"{generatedDir.FullName}\Ts\PLC\DUTs\IO\"));
            Assert.IsTrue(TestsCommon.AllFilesAreEqual(@$"{expectedDir.FullName}\Ts\PLC\GVLs\", @$"{generatedDir.FullName}\Ts\PLC\GVLs\"));
        }

        private static void CopyTestFiles(string source)
        {
            TestsCommon.CopyFilesRecursively(source, expectedDir.FullName);

            string str = generatedDir.FullName + @"\Ts";
            if (Directory.Exists(str))
                Directory.Delete(str,true);
            Directory.CreateDirectory(str);

            str = str + @"\PLC";
            if (Directory.Exists(str))
                Directory.Delete(str,true);
            Directory.CreateDirectory(str);

            str = @"\Ts\Ts.tsproj";
            TestsCommon.CopyTsprojFileWithoutMappings(expectedDir.FullName + str, generatedDir.FullName + str);
            Assert.IsTrue(File.Exists(generatedDir.FullName + str));
            Assert.IsFalse(TestsCommon.AreFilesEqual(expectedDir.FullName + str, generatedDir.FullName + str));

            str = @"\Ts\PLC\Plc.plcproj";
            TestsCommon.CopyPlcprojFileWithoutGeneratedItems(expectedDir.FullName + str, generatedDir.FullName + str);
            Assert.IsTrue(File.Exists(generatedDir.FullName + str));
            Assert.IsFalse(TestsCommon.AreFilesEqual(expectedDir.FullName + str, generatedDir.FullName + str));

            str = @"\Ts\PLC\PlcTask.TcTTO";
            File.Copy(expectedDir.FullName + str, generatedDir.FullName + str);
            Assert.IsTrue(File.Exists(generatedDir.FullName + str));
            Assert.IsTrue(TestsCommon.AreFilesEqual(expectedDir.FullName + str, generatedDir.FullName + str));
        }
    }
}