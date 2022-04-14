using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;
using iXlinkerDtos;
using iXlinker.TsprojFile;
using iXlinker.TsprojFile.Mapping;
using TsprojFile.Scan;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using iXlinker.Utils;
using System.Xml.Serialization;
using System;
using TwincatXmlSchemas.TcPlcObject;

namespace iXlinkerUnitTests
{
    public class TestsCommon
    {
        internal static string TestFolderPath { get; private set; }
        internal static string SourcePath { get; private set; }
        internal static DirectoryInfo expectedDir { get; private set; }
        internal static DirectoryInfo generatedDir { get; private set; }

        internal static void OneTimeSetup()
        {
            var executingAssemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var executingAssemblyFolder = executingAssemblyPath.Directory.FullName;
            TestFolderPath = Path.GetFullPath(@"..\..\..\..\test_projects\", executingAssemblyFolder);
            SourcePath = (TestFolderPath + "\\tabularasa").Replace("\\\\","\\");
            expectedDir = new DirectoryInfo((TestFolderPath + "\\expected").Replace("\\\\", "\\"));
            expectedDir.Delete(true);
            expectedDir.Create();
            generatedDir = new DirectoryInfo((TestFolderPath + "\\generated").Replace("\\\\", "\\"));
            generatedDir.Delete(true);
            generatedDir.Create();
            EventLogger.VerbosityLevel = Serilog.Events.LogEventLevel.Error;
        }

        internal static void Setup()
        {
            expectedDir.Delete(true);
            expectedDir.Create();
            generatedDir.Delete(true);
            generatedDir.Create();
        }

        internal static void OneTimeTearDown()
        {
            expectedDir.Delete(true);
            expectedDir.Create();
            generatedDir.Delete(true);
            generatedDir.Create();
        }
        
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

        internal static bool AllFilesAreEqual(string expectedFolder, string generatedFolder)
        {
            bool areEqual = true;
            EnumerationOptions enumerationOptions = new EnumerationOptions() { RecurseSubdirectories = true };
            string[] expectedFiles = Directory.GetFiles(expectedFolder, "*", enumerationOptions);
            foreach (string expectedFile in expectedFiles)
            {
                string generatedFile = expectedFile.Replace(expectedFolder,generatedFolder);
                if (File.Exists(generatedFile))
                {
                    if (!AreFilesEqual(expectedFile, generatedFile))
                    {
                        if (!AreFileContentsEqual(expectedFile, generatedFile))
                        {
                            areEqual = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine(@"Generated file ""{0}"" not exists.", generatedFile.Replace(generatedFolder,".."));
                    areEqual = false;
                }
            }
            return areEqual;
        }
        internal static bool AllFilesAreEqual()
        {
            return AllFilesAreEqual(expectedDir.FullName, generatedDir.FullName);
        }
        private static bool AreFilesEqual(string path1, string path2) => File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
        private static bool AreFileContentsEqual(string pathExpected, string pathGenerated)
        {
            bool areEqual = true;
            string[] expected = File.ReadAllLines(pathExpected);
            string[] generated = File.ReadAllLines(pathGenerated);

            if (pathExpected.EndsWith(".TcDUT"))
            {
                if (!AreTcDUTFilesEqual(pathExpected, pathGenerated))
                {
                    areEqual = false;
                }
            }
            else if (expected.Length == generated.Length)
            {
                for (int row = 0; row < expected.Length; row++)
                {
                    if (!expected[row].Equals(generated[row]))
                    {
                        int column = GetFirstDiffIndex(expected[row], generated[row]);
                        Console.WriteLine(@"File content ""{0}"" differs. Row: {1} , column: {2}.", pathGenerated.Replace(generatedDir.FullName,".."), row,column);
                        Console.WriteLine(@"Expected: ""{0}""", expected[row]);
                        Console.WriteLine(@"Generated: ""{0}""", generated[row]);
                        areEqual = false;
                    }
                }
            }
            else
            {
                Console.WriteLine(@"Unexpected length of file: ""{0}""!!!", pathGenerated.Replace(generatedDir.FullName, ".."));
                areEqual = false;
            }
            return areEqual;
        }
        private static bool AreTcDUTFilesEqual(string pathExpected, string pathGenerated)
        {
            bool areEqual = true;
            TcPlcObjectBaseDeclType dutExpected = GetTcDUTContent(pathExpected);
            TcPlcObjectBaseDeclType dutGenerated = GetTcDUTContent(pathGenerated);
            if (dutExpected ==null || dutGenerated == null)
            {
                areEqual = false;
            }
            if (dutExpected.Name != dutGenerated.Name)
            {
                Console.WriteLine(@"Different structure names found inside file: ""{0}""", pathGenerated.Replace(generatedDir.FullName, ".."));
                Console.WriteLine(@"Expected: ""{0}""", dutExpected.Name);
                Console.WriteLine(@"Generated: ""{0}""", dutGenerated.Name);
                areEqual = false;
            }

            if (dutExpected.Declaration != dutGenerated.Declaration)
            {
                string[] expected = GetAllRelevantTcDutDelarationLines(dutExpected.Declaration).ToArray();
                string[] generated = GetAllRelevantTcDutDelarationLines(dutGenerated.Declaration).ToArray();
                if (expected.Length != generated.Length)
                {
                    Console.WriteLine(@"Different number of the declaration rows in the file: ""{0}""", pathGenerated.Replace(generatedDir.FullName, ".."));
                    Console.WriteLine(@"Expected: ""{0}""", expected.Length);
                    Console.WriteLine(@"Generated: ""{0}""", generated.Length);
                    areEqual = false;
                }
                for (int item = 0; item < expected.Length; item++)
                {
                    if (!expected[item].Equals(generated[item]))
                    {
                        int column = GetFirstDiffIndex(expected[item], generated[item]);
                        Console.WriteLine(@"File: ""{0}"", declaration part of the structure: {1} differs. Item: {2} , column: {3}.", pathGenerated.Replace(generatedDir.FullName, ".."), dutExpected.Name, item, column);
                        Console.WriteLine(@"Expected: ""{0}""", expected[item]);
                        Console.WriteLine(@"Generated: ""{0}""", generated[item]);
                        areEqual = false;
                    }
                }

            }

            return areEqual;
        }
        private static int GetFirstDiffIndex(string str1, string str2)
        {
            if (str1 == null || str2 == null) return -1;

            int length = str1.Length;

            for (int index = 0; index < length; index++)
            {
                if (str1[index] != str2[index])
                {
                    return index;
                }
            }

            return -1;
        }
        private static List<string> GetAllRelevantTcDutDelarationLines(string declarationField)
        {
            string[] exludedPrefixes = {
                "{attribute 'GeneratedUsingTerminal: ",
                "{attribute addProperty BoxType",
                "{attribute addProperty Id",
                "{attribute addProperty CRC",
                };

            List<string> declarationFiltered = new List<string>();

            string[] declarationItems = declarationField.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string item in declarationItems)
            {
                bool included = true;
                foreach (string excluded in exludedPrefixes)
                {
                    if (item.StartsWith(excluded))
                    {
                        included = false;
                        break;
                    }
                }
                if(included) declarationFiltered.Add(item);
            }
            return declarationFiltered;
        }
        private static TcPlcObjectBaseDeclType GetTcDUTContent(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TcPlcObject));
            StreamReader reader = new StreamReader(path);
            TcPlcObject tcDUT = new TcPlcObject();
            TcPlcObjectBaseDeclType baseDeclType = new TcPlcObjectBaseDeclType();
            try
            {
                tcDUT = (TcPlcObject)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Unable to retrieve: ""{0}""", path);
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                tcDUT = null;
                reader.Close();
            }

            try
            {
                baseDeclType = (TcPlcObjectBaseDeclType)tcDUT.Item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Unable to cast declaration part of the structure: ""{0}"" in the file:""{1}""", tcDUT.Item, path);
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                baseDeclType = null;
            }

            return baseDeclType;
        }

        internal static void Arrange(string patternPath)
        {
            CopyFilesRecursively(SourcePath + "\\" + patternPath, expectedDir.FullName);
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\Ts.tsproj"));
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\Plc.plcproj"));
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\PlcTask.TcTTO"));
            File.Copy(expectedDir.FullName + "\\Ts.tsproj", generatedDir.FullName + "\\Ts.tsproj");
            File.Copy(expectedDir.FullName + "\\Plc.plcproj", generatedDir.FullName + "\\Plc.plcproj");
            File.Copy(expectedDir.FullName + "\\PlcTask.TcTTO", generatedDir.FullName + "\\PlcTask.TcTTO");
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\Ts.tsproj"));
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\Plc.plcproj"));
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\PlcTask.TcTTO"));
        }
        internal static void Act()
        {
            string tsProjFilePath = generatedDir.FullName + "\\Ts.tsproj";
            string plcProjFilePath = generatedDir.FullName + "\\Plc.plcproj";
            string activeTargetPlatform = "Release|TwinCAT RT(x64)";
            string devenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";

            Solution vs = VS.GetXaeProjectDetails(tsProjFilePath, activeTargetPlatform, plcProjFilePath, true, devenvPath, 0);
            ScanTcProjFile tcProj = new ScanTcProjFile();
            tcProj.SearchDevices(vs);
            tcProj.GenerateStructures(vs);
            tcProj.GenerateMappingsToTsProj(vs);
        }
    }
}

