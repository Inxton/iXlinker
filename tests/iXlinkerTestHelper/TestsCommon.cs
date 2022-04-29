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
using TwincatXmlSchemas.TcSmProject;
using TwincatXmlSchemas.TcPlcProj;

namespace iXlinkerTestHelper
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
            SourcePath = (TestFolderPath + "\\tabularasa").Replace("\\\\", "\\");
            expectedDir = new DirectoryInfo((TestFolderPath + "\\expected").Replace("\\\\", "\\"));
            if (Directory.Exists(expectedDir.FullName))
            {
                expectedDir.Delete(true);
            }
            expectedDir.Create();
            generatedDir = new DirectoryInfo((TestFolderPath + "\\generated").Replace("\\\\", "\\"));
            if (Directory.Exists(generatedDir.FullName))
            {
                generatedDir.Delete(true);
            }
            generatedDir.Create();
            EventLogger.VerbosityLevel = Serilog.Events.LogEventLevel.Error;
        }

        internal static void Setup()
        {
            if (Directory.Exists(expectedDir.FullName))
            {
                expectedDir.Delete(true);
            }
            expectedDir.Create();
            if (Directory.Exists(generatedDir.FullName))
            {
                generatedDir.Delete(true);
            }
            generatedDir.Create();
        }

        internal static void OneTimeTearDown()
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

        internal static void CopyFilesRecursively(string sourcePath, string targetPath)
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

        internal static bool AllFilesAreEqual()
        {
            return AllFilesAreEqual(expectedDir.FullName, generatedDir.FullName);
        }
        internal static bool AllFilesAreEqual(string expectedFolder, string generatedFolder)
        {
            bool areEqual = true;
            EnumerationOptions enumerationOptions = new EnumerationOptions() { RecurseSubdirectories = true };
            string[] expectedFiles = Directory.GetFiles(expectedFolder, "*", enumerationOptions);
            foreach (string expectedFile in expectedFiles)
            {
                string generatedFile = expectedFile.Replace(expectedFolder, generatedFolder);
                if (File.Exists(generatedFile))
                {
                    areEqual = AreFilesEqual(expectedFile, generatedFile) ? areEqual : false;
                }
                else
                {
                    Console.WriteLine(@"Generated file ""{0}"" not exists.", generatedFile.Replace(generatedFolder, ".."));
                    areEqual = false;
                }
            }
            return areEqual;
        }
        internal static bool AreFilesEqual(string path1, string path2)
        {
            bool areEqual = false;
            if (AreFilesBytwiseEqual(path1, path2))
            {
                areEqual = true;
            }
            else if (AreFileContentsEqual(path1, path2))
            {
                areEqual = true;
            }

            return areEqual;
        }

        private static bool AreFilesBytwiseEqual(string path1, string path2) => File.ReadAllBytes(path1).SequenceEqual(File.ReadAllBytes(path2));
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
            else if (pathExpected.EndsWith(".TcGVL"))
            {
                if (!AreTcGVLFilesEqual(pathExpected, pathGenerated))
                {
                    areEqual = false;
                }
            }
            else if (pathExpected.EndsWith(".tsproj"))
            {
                if (!DevicesAndMappingsInsideTsprojFilesAreEqual(pathExpected, pathGenerated))
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
                        Console.WriteLine(@"File content ""{0}"" differs. Row: {1} , column: {2}.", pathGenerated.Replace(generatedDir.FullName, ".."), row, column);
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
            if (dutExpected == null || dutGenerated == null)
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
                string[] expected = GetAllRelevantTcDutDeclarationLines(dutExpected.Declaration).ToArray();
                string[] generated = GetAllRelevantTcDutDeclarationLines(dutGenerated.Declaration).ToArray();
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
        private static bool AreTcGVLFilesEqual(string pathExpected, string pathGenerated)
        {
            bool areEqual = true;
            TcPlcObjectBaseDeclType gvlExpected = GetTcGvlContent(pathExpected);
            TcPlcObjectBaseDeclType gvlGenerated = GetTcGvlContent(pathGenerated);
            if (gvlExpected == null || gvlGenerated == null)
            {
                areEqual = false;
            }
            if (gvlExpected.Name != gvlGenerated.Name)
            {
                Console.WriteLine(@"Different structure names found inside file: ""{0}""", pathGenerated.Replace(generatedDir.FullName, ".."));
                Console.WriteLine(@"Expected: ""{0}""", gvlExpected.Name);
                Console.WriteLine(@"Generated: ""{0}""", gvlGenerated.Name);
                areEqual = false;
            }

            if (gvlExpected.Declaration != gvlGenerated.Declaration)
            {
                string[] expected = GetAllRelevantTcGvlDeclarationLines(gvlExpected.Declaration).ToArray();
                string[] generated = GetAllRelevantTcGvlDeclarationLines(gvlGenerated.Declaration).ToArray();
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
                        Console.WriteLine(@"File: ""{0}"", declaration part of the structure: {1} differs. Item: {2} , column: {3}.", pathGenerated.Replace(generatedDir.FullName, ".."), gvlExpected.Name, item, column);
                        Console.WriteLine(@"Expected: ""{0}""", expected[item]);
                        Console.WriteLine(@"Generated: ""{0}""", generated[item]);
                        areEqual = false;
                    }
                }

            }

            return areEqual;
        }
        private static bool DevicesAndMappingsInsideTsprojFilesAreEqual(string pathExpected, string pathGenerated)
        {
            return DevicesInsideTsprojFilesAreEqual(pathExpected, pathGenerated) && MappingsInsideTsprojFilesAreEqual(pathExpected, pathGenerated);
        }
        private static bool DevicesInsideTsprojFilesAreEqual(string pathExpected, string pathGenerated)
        {
            bool areEqual = true;
            try
            {
                TcSmProject tsProjSource = new TcSmProject();
                using (StreamReader reader = new StreamReader(pathExpected))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(TcSmProject));
                    tsProjSource = (TcSmProject)deserializer.Deserialize(reader);
                }
                TcSmProject tsProjDest = new TcSmProject();
                using (StreamReader reader = new StreamReader(pathGenerated))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(TcSmProject));
                    tsProjDest = (TcSmProject)deserializer.Deserialize(reader);
                }
                MemoryStream src = new MemoryStream();
                MemoryStream dest = new MemoryStream();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TcSmProjectProjectIO));
                xmlSerializer.Serialize(src, tsProjSource.Project.Io);
                xmlSerializer.Serialize(dest, tsProjDest.Project.Io);
                if (src.Length != dest.Length)
                {
                    areEqual = false;
                }
                else if (!src.ToArray().SequenceEqual(dest.ToArray()))
                {
                    areEqual = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                areEqual = false;
            }
            return areEqual;
        }
        private static bool MappingsInsideTsprojFilesAreEqual(string pathExpected, string pathGenerated)
        {
            bool areEqual = true;
            try
            {
                TcSmProject tsProjSource = new TcSmProject();
                using (StreamReader reader = new StreamReader(pathExpected))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(TcSmProject));
                    tsProjSource = (TcSmProject)deserializer.Deserialize(reader);
                }
                TcSmProject tsProjDest = new TcSmProject();
                using (StreamReader reader = new StreamReader(pathGenerated))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(TcSmProject));
                    tsProjDest = (TcSmProject)deserializer.Deserialize(reader);
                }
                MemoryStream src = new MemoryStream();
                MemoryStream dest = new MemoryStream();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MappingsType));
                xmlSerializer.Serialize(src, tsProjSource.Mappings);
                xmlSerializer.Serialize(dest, tsProjDest.Mappings);
                if (src.Length != dest.Length)
                {
                    areEqual = false;
                }
                else if (!src.ToArray().SequenceEqual(dest.ToArray()))
                {
                    areEqual = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                areEqual = false;
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
        private static List<string> GetAllRelevantTcDutDeclarationLines(string declarationField)
        {
            string[] exludedPrefixes = {
                "{attribute 'GeneratedUsingTerminal: ",
                "{attribute addProperty BoxType",
                "{attribute addProperty Id",
                "{attribute addProperty CRC",
                };

            List<string> declarationFiltered = new List<string>();

            string[] declarationItems = declarationField.Split(new string[] { "\n"}, StringSplitOptions.None);

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
                if (included) declarationFiltered.Add(item);
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
        private static List<string> GetAllRelevantTcGvlDeclarationLines(string declarationField)
        {
            string[] exludedPrefixes = {
                "{attribute "
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
                if (included) declarationFiltered.Add(item);
            }
            return declarationFiltered;
        }
        private static TcPlcObjectBaseDeclType GetTcGvlContent(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TcPlcObject));
            StreamReader reader = new StreamReader(path);
            TcPlcObject tcGVL = new TcPlcObject();
            TcPlcObjectBaseDeclType baseDeclType = new TcPlcObjectBaseDeclType();
            try
            {
                tcGVL = (TcPlcObject)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Unable to retrieve: ""{0}""", path);
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                tcGVL = null;
                reader.Close();
            }

            try
            {
                baseDeclType = (TcPlcObjectBaseDeclType)tcGVL.Item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Unable to cast declaration part of the global variable list: ""{0}"" in the file:""{1}""", tcGVL.Item, path);
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                baseDeclType = null;
            }

            return baseDeclType;
        }
        internal static void CopyTsprojFileWithoutMappings(string source, string destination)
        {
            try
            {
                File.Copy(source, destination);
                TcSmProject tsProj = new TcSmProject();
                //Read and deserialize content of the tsProj file located on the destination path
                using (StreamReader reader = new StreamReader(destination))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(TcSmProject));
                    tsProj = (TcSmProject)deserializer.Deserialize(reader);
                }
                //By creating new mapping, all existing mappings are overwritten
                tsProj.Mappings = new MappingsType();
                //Serialize and write content of the tsProj to the file located on the destination path
                using (StreamWriter writer = new StreamWriter(destination))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
                    serializer.Serialize(writer, tsProj);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
        }
        internal static void CopyPlcprojFileWithoutGeneratedItems(string source, string destination)
        {
            try
            {
                File.Copy(source, destination);
                Project plcProj = new Project();
                //Read and deserialize content of the plcProj file located on the destination path
                using (StreamReader reader = new StreamReader(destination))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Project));
                    plcProj = (Project)serializer.Deserialize(reader);
                }
                //Remove generated items
                foreach (ProjectItemGroup item in plcProj.ItemGroup)
                {
                    if (item.Compile != null)
                    {
                        List<ProjectItemGroupCompile> itemCompileList = item.Compile.ToList();
                        itemCompileList.RemoveAll(item => (item.SubType.Equals("Code") && (item.Include.Equals(@"GVLs\GVL_iXlinker.TcGVL") || (item.Include.StartsWith(@"DUTs\IO") && item.Include.EndsWith(".TcDUT")))));
                        item.Compile = itemCompileList.ToArray();
                    }
                }
                //Serialize and write content of the tsProj to the file located on the destination path
                using (StreamWriter writer = new StreamWriter(destination))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Project));
                    serializer.Serialize(writer, plcProj);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
        }
        internal static void Arrange(string patternPath)
        {
            CopyFilesRecursively(SourcePath + "\\" + patternPath, expectedDir.FullName);
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\Ts.tsproj"));
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\Plc.plcproj"));
            Assert.IsTrue(File.Exists(expectedDir.FullName + "\\PlcTask.TcTTO"));
            CopyTsprojFileWithoutMappings(expectedDir.FullName + "\\Ts.tsproj", generatedDir.FullName + "\\Ts.tsproj");
            CopyPlcprojFileWithoutGeneratedItems(expectedDir.FullName + "\\Plc.plcproj", generatedDir.FullName + "\\Plc.plcproj");
            File.Copy(expectedDir.FullName + "\\PlcTask.TcTTO", generatedDir.FullName + "\\PlcTask.TcTTO");
            Assert.IsTrue(File.Exists(generatedDir.FullName + "\\Ts.tsproj"));
            Assert.IsTrue(File.Exists(generatedDir.FullName + "\\Plc.plcproj"));
            Assert.IsTrue(File.Exists(generatedDir.FullName + "\\PlcTask.TcTTO"));
            Assert.IsFalse(AreFileContentsEqual(expectedDir.FullName + "\\Ts.tsproj", generatedDir.FullName + "\\Ts.tsproj"));
            Assert.IsFalse(AreFileContentsEqual(expectedDir.FullName + "\\Plc.plcproj", generatedDir.FullName + "\\Plc.plcproj"));
            Assert.IsTrue(AreFileContentsEqual(expectedDir.FullName + "\\PlcTask.TcTTO", generatedDir.FullName + "\\PlcTask.TcTTO"));
        }
        internal static void Act()
        {
            string tsProjFilePath = generatedDir.FullName + "\\Ts.tsproj";
            string plcProjFilePath = generatedDir.FullName + "\\Plc.plcproj";
            string activeTargetPlatform = "Release|TwinCAT RT(x64)";

            Solution vs = VS.GetXaeProjectDetails(tsProjFilePath, activeTargetPlatform, plcProjFilePath, true, "", 0);
            ScanTcProjFile tcProj = new ScanTcProjFile();
            tcProj.SearchDevices(vs);
            tcProj.GenerateStructures(vs);
            tcProj.GenerateMappingsToTsProj(vs);
        }
        internal static string GetTypeFromWhichDtuExtends(string path)
        {
            string ret = "";
            TcPlcObjectBaseDeclType dut = GetTcDUTContent(path);
            List<string> declarationLines = GetAllRelevantTcDutDeclarationLines(dut.Declaration);
            foreach (string declarationLine in declarationLines)
            {
                if (declarationLine.StartsWith("TYPE") && declarationLine.Contains("EXTENDS"))
                {
                    ret = declarationLine.Split("EXTENDS")[1].Replace(" ","").Replace(":", "");

                    break;
                }
            }

                return ret;
        }
    }
}