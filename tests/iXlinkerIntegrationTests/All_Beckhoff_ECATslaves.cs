using iXlinker.Utils;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;

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
            expectedDir.Delete(true);
            expectedDir.Create();
            generatedDir = new DirectoryInfo((TestFolderPath + "\\generated").Replace("\\\\", "\\"));
            generatedDir.Delete(true);
            generatedDir.Create();
            EventLogger.VerbosityLevel = Serilog.Events.LogEventLevel.Information;
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            expectedDir.Delete(true);
            expectedDir.Create();
            generatedDir.Delete(true);
            generatedDir.Create();
        }


        [Test]
        public void CompareWithExpected()
        {
            //var generatedDirectory = @$"{TestFolderPath}\generated\All_Beckhoff_ECATslaves";

            //if (Directory.Exists(generatedDirectory))
            //{
            //    Directory.Delete(generatedDirectory, true);
            //}

            //Assert.IsFalse(Directory.Exists(generatedDirectory));

            //CopyFilesRecursively(@$"{TestFolderPath}\tabularasa\", $@"{TestFolderPath}\actual\");


            //var TsProjFilePath = @$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj";
            //var ActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            //var PlcProjFilePath = @$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj";
            //var DoNotGenerateDisabled = true;
            //var DevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";
            //var MaxEthercatFrameIndex = 25;

            //CopyFilesRecursively(@$"{SourcePath}\All_Beckhoff_ECATslaves", generatedDir.FullName);

            //var TsProjFilePath = @$"{generatedDir.FullName}\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj";
            //var ActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            //var PlcProjFilePath = @$"{generatedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj";
            //var DoNotGenerateDisabled = true;
            //var DevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";
            //var MaxEthercatFrameIndex = 25;


            //var args = new string[] { $"-t", $"{TsProjFilePath}", "-p", $"{ActiveTargetPlatform}", "-c",  $"{PlcProjFilePath}", "-g", $"{DoNotGenerateDisabled}", "-d", $"{DevenvPath}", "-n", $"{MaxEthercatFrameIndex}" };

            //iXlinker.Program.Main(args);

            //Assert.IsTrue(AreFileContentsEqual(TsProjFilePath, @$"{expectedDir.FullName}\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj"));
            //Assert.IsTrue(AreFileContentsEqual(PlcProjFilePath, @$"{expectedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj"));

            //var expectedDutFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\expected\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\DUTs\IO\").ToList();
            //var generatedDutFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\generated\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\DUTs\IO\").ToList();

            //for (int i = 0; i < expectedDutFiles.Count(); i++)
            //{
            //    Assert.IsTrue(AreFileContentsEqual(expectedDutFiles[i], generatedDutFiles[i]));
            //}

            //var expectedGvlFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\expected\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\GVLs\").ToList();
            //var actualGvlFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\generated\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\GVLs\").ToList();

            //for (int i = 0; i < expectedDutFiles.Count(); i++)
            //{
            //    Assert.IsTrue(AreFileContentsEqual(expectedGvlFiles[i], actualGvlFiles[i]));
            //}

            CopyFilesRecursively(@$"{SourcePath}\All_Beckhoff_ECATslaves", expectedDir.FullName);
            CopyFilesRecursively(@$"{SourcePath}\All_Beckhoff_ECATslaves", generatedDir.FullName);

            var TsProjFilePath = @$"{generatedDir.FullName}\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj";
            var ActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            var PlcProjFilePath = @$"{generatedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj";
            var DoNotGenerateDisabled = true;
            var DevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";
            var MaxEthercatFrameIndex = 25;


            var args = new string[] { $"-t", $"{TsProjFilePath}", "-p", $"{ActiveTargetPlatform}", "-c", $"{PlcProjFilePath}", "-g", $"{DoNotGenerateDisabled}", "-d", $"{DevenvPath}", "-n", $"{MaxEthercatFrameIndex}" };

            iXlinker.Program.Main(args);

            Assert.IsTrue(AreFileContentsEqual(TsProjFilePath, @$"{expectedDir.FullName}\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj"));
            Assert.IsTrue(AreFileContentsEqual(PlcProjFilePath, @$"{expectedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj"));

            var expectedDutFiles = Directory.EnumerateFiles(@$"{expectedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\DUTs\IO\").ToList();
            var generatedDutFiles = Directory.EnumerateFiles(@$"{generatedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\DUTs\IO\").ToList();

            for (int i = 0; i < expectedDutFiles.Count(); i++)
            {
                Assert.IsTrue(AreFileContentsEqual(expectedDutFiles[i], generatedDutFiles[i]));
            }

            var expectedGvlFiles = Directory.EnumerateFiles(@$"{expectedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\GVLs\").ToList();
            var actualGvlFiles = Directory.EnumerateFiles(@$"{generatedDir.FullName}\All_Beckhoff_ECATslaves\Untitled1\GVLs\").ToList();

            for (int i = 0; i < expectedDutFiles.Count(); i++)
            {
                Assert.IsTrue(AreFileContentsEqual(expectedGvlFiles[i], actualGvlFiles[i]));
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
    }
}