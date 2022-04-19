using iXlinker.TsprojFile.Mapping;
using iXlinker.Utils;
using iXlinkerDtos;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;
using TsprojFile.Scan;

namespace iXlinkerIntegrationTests
{
    public class Integration
    {
        internal static string TestFolderPath { get; private set; }
        internal static string SourcePath { get; private set; }
        internal static DirectoryInfo expectedDir { get; private set; }
        internal static DirectoryInfo generatedDir { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var executingAssemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var executingAssemblyFolder = executingAssemblyPath.Directory.FullName;
            TestFolderPath = Path.GetFullPath(@"..\..\..\..\test_projects\", executingAssemblyFolder);
            SourcePath = (TestFolderPath + "\\tabularasa").Replace("\\\\", "\\");
            expectedDir = new DirectoryInfo((TestFolderPath + "\\expected").Replace("\\\\", "\\"));
            if (Directory.Exists(expectedDir.FullName))
            {
                expectedDir.Delete(true);
            }
            expectedDir.Create();
            generatedDir = new DirectoryInfo((TestFolderPath + "\\generated").Replace("\\\\", "\\"));
            if(Directory.Exists(generatedDir.FullName))
            {
                generatedDir.Delete(true);
            }
            generatedDir.Create();
            EventLogger.VerbosityLevel = Serilog.Events.LogEventLevel.Information;
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (Directory.Exists(expectedDir.FullName))
            {
                expectedDir.Delete(true);
            }
            if (Directory.Exists(generatedDir.FullName))
            {
                generatedDir.Delete(true);
            }
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


            Assert.IsTrue(AreFileContentsEqual(TsProjFilePath, @$"{expectedDir.FullName}\Ts\Ts.tsproj"));
            Assert.IsTrue(AreFileContentsEqual(PlcProjFilePath, @$"{expectedDir.FullName}\Ts\PLC\PLC.plcproj"));

            var expectedDutFiles = Directory.EnumerateFiles(@$"{expectedDir.FullName}\Ts\PLC\DUTs\IO\").ToList();
            var generatedDutFiles = Directory.EnumerateFiles(@$"{generatedDir.FullName}\Ts\PLC\DUTs\IO\").ToList();

            for (int i = 0; i < expectedDutFiles.Count(); i++)
            {
                Assert.IsTrue(AreFileContentsEqual(expectedDutFiles[i], generatedDutFiles[i]));
            }

            var expectedGvlFiles = Directory.EnumerateFiles(@$"{expectedDir.FullName}\Ts\PLC\GVLs\").ToList();
            var generatedGvlFiles = Directory.EnumerateFiles(@$"{generatedDir.FullName}\Ts\PLC\GVLs\").ToList();

            for (int i = 0; i < expectedDutFiles.Count(); i++)
            {
                Assert.IsTrue(AreFileContentsEqual(expectedGvlFiles[i], generatedGvlFiles[i]));
            }
        }
        public static bool AreFileContentsEqual(string path1, string path2) =>
              File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        private static void CopyTestFiles(string source)
        {
            CopyFilesRecursively(source, expectedDir.FullName);
            CopyFilesRecursively(source, generatedDir.FullName);
        }
    }
}