using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace iXlinkerExtInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o => RunVsixInstaller(o.ExtensionsInstallPath, o.VsixInstallerPath, o.VsixPath)).WithNotParsed(o => Console.WriteLine("Not parsed"));
        }

        public static void RunVsixInstaller(string extensionPath, string vsixInstaller, string vsixPath)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\extension.vsixmanifest"))
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\iXlinkerExt.vsix"))
                {
                    PackageManifest actPackageManifest = GetPackageManifest(AppDomain.CurrentDomain.BaseDirectory + @"\extension.vsixmanifest");
                    List<PackageManifest> allPackageManifest = new List<PackageManifest>();
                    List<string> manifestFiles = new List<string>(Directory.GetFiles(extensionPath, "extension.vsixmanifest", SearchOption.AllDirectories));

                    foreach (string manifestFile in manifestFiles)
                    {
                        allPackageManifest.Add(GetPackageManifest(manifestFile));
                    }

                    bool isInstalled = false;

                    Version installedVersion = new Version();
                    Version rdy2installVersion = Version.Parse(actPackageManifest.Metadata.Identity.Version);
                    foreach (PackageManifest item in allPackageManifest)
                    {
                        if (item.Metadata.Identity.Id.Equals(actPackageManifest.Metadata.Identity.Id))
                        {
                            installedVersion = Version.Parse(item.Metadata.Identity.Version);
                            if (installedVersion >= rdy2installVersion)
                            {
                                isInstalled = true;
                                Console.WriteLine("iXlinker extension already installed with Id:{0} and version: {1}", item.Metadata.Identity.Id, item.Metadata.Identity.Version);
                                break;
                            }
                        }
                    }
                    if (!isInstalled)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.CreateNoWindow = false;
                        startInfo.UseShellExecute = false;
                        startInfo.WorkingDirectory = vsixInstaller;
                        startInfo.FileName = vsixInstaller + @"\VSIXInstaller.exe";
                        startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        startInfo.Arguments = " /logFile:iXlinkerExtInstaller.log " + @"""" + vsixPath + @"""" + @"\iXlinkerExt.vsix";

                        try
                        {
                            Console.WriteLine("Installing iXlinker extension with Id:{0} and version: {1}", actPackageManifest.Metadata.Identity.Id, actPackageManifest.Metadata.Identity.Version);
                            Process exeProcess = Process.Start(startInfo);
                            exeProcess.WaitForExit();
                            Console.WriteLine("iXlinker succesfully extension installed with Id:{0} and version: {1}", actPackageManifest.Metadata.Identity.Id, actPackageManifest.Metadata.Identity.Version);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(@"Extension instalation file: {0}\iXlinkerExt.vsix not found!", AppDomain.CurrentDomain.BaseDirectory);
                }
            }
            else
            {
                Console.WriteLine(@"Extension manifest: {0}\extension.vsixmanifest not found!", AppDomain.CurrentDomain.BaseDirectory);
            }
            Console.WriteLine("iXlinkerExtInstaller:Done");
        }
        internal class CommandLineOptions
        {
            [Option('e', "extensionPath", Required = true, HelpText = "Path to the folder where vsix extensions are located.")]
            public string ExtensionsInstallPath { get { return this.extensionsInstallPath; } set { if (!string.IsNullOrEmpty(value)) { this.extensionsInstallPath = ReplaceDoubleBackslash(value); } } }

            private string extensionsInstallPath;

            [Option('i', "vsixInstallerPath", Required = true, HelpText = "Path to the folder where VSIXInstaller.exe is located.")]
            public string VsixInstallerPath { get { return this.vsixInstallerPath; } set { if (!string.IsNullOrEmpty(value)) { this.vsixInstallerPath = ReplaceDoubleBackslash(value); } } }

            private string vsixInstallerPath;

            [Option('v', "vsixPath", Required = true, HelpText = "Path to the folder where vsix extension is located.")]
            public string VsixPath { get { return this.vsixPath; } set { if (!string.IsNullOrEmpty(value)) { this.vsixPath = ReplaceDoubleBackslash(value); } } }

            private string vsixPath;
            private string ReplaceDoubleBackslash(string path)
            {
                return path.Replace("\\\\", "\\");
            }
        }

        public static PackageManifest GetPackageManifest(string manifestFile)
        {
            PackageManifest packageManifest = new PackageManifest();
            if (File.Exists(manifestFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PackageManifest));
                StreamReader reader = new StreamReader(manifestFile);

                try
                {
                    packageManifest = (PackageManifest)serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    reader.Close();
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
            }
            return packageManifest;
        }
    }
}
