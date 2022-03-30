using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;

namespace iXlinkerIntegrationTests
{
    public class Integration
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var executingAssemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var executingAssemblyFolder = executingAssemblyPath.Directory.FullName;
            TestFolderPath = Path.GetFullPath(@"..\..\..\..\test_projects\", executingAssemblyFolder);
        }

        public string TestFolderPath { get; private set; }

        [Test]
        public void CompareWithExpected()
        {
            var actualDirectory = @$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves";

            if (Directory.Exists(actualDirectory))
            {
                Directory.Delete(actualDirectory, true);
            }

            Assert.IsFalse(Directory.Exists(actualDirectory));

            CopyFilesRecursively(@$"{TestFolderPath}\tabularasa\", $@"{TestFolderPath}\actual\");

            var TsProjFilePath = @$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj";
            var ActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            var PlcProjFilePath = @$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj";
            var DoNotGenerateDisabled = true;
            var DevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";
            var MaxEthercatFrameIndex = 25;

            var args = new string[] { $"-t", $"{TsProjFilePath}", "-p", $"{ActiveTargetPlatform}", "-c",  $"{PlcProjFilePath}", "-g", $"{DoNotGenerateDisabled}", "-d", $"{DevenvPath}", "-n", $"{MaxEthercatFrameIndex}" };

            iXlinker.Program.Main(args);

            Assert.IsTrue(AreFileContentsEqual(TsProjFilePath, @$"{TestFolderPath}\expected\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves.tsproj"));
            Assert.IsTrue(AreFileContentsEqual(PlcProjFilePath, @$"{TestFolderPath}\expected\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\Untitled1.plcproj"));

            var expectedDutFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\expected\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\DUTs\IO\").ToList();
            var actualDutFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\DUTs\IO\").ToList();

            for (int i = 0; i < expectedDutFiles.Count(); i++)
            {
                Assert.IsTrue(AreFileContentsEqual(expectedDutFiles[i], actualDutFiles[i]));
            }

            var expectedGvlFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\expected\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\GVLs\").ToList();
            var actualGvlFiles = Directory.EnumerateFiles(@$"{TestFolderPath}\actual\All_Beckhoff_ECATslaves\All_Beckhoff_ECATslaves\Untitled1\GVLs\").ToList();

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