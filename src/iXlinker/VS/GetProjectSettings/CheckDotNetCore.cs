using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        internal static bool CheckDotNetCore()
        {
            Version minVersion = new Version(minDotNetCoreSupportedVersionIncluded);
            Version maxVersion = new Version(maxDotNetCoreSupportedVersionExcluded);

            bool dotNetCoreOK = false;
            try
            {
                using (Process p = new Process())
                {
                    string cmd = "dotnet --list-runtimes";
                    string cmdOutput="";
                    p.StartInfo = new ProcessStartInfo("cmd.exe")
                    {
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                    };

                    p.Start();
                    p.StandardInput.Write(cmd + p.StandardInput.NewLine);
                    while (!cmdOutput.Contains(cmd))
                    {
                        cmdOutput = p.StandardOutput.ReadLine();
                    }
                    do
                    {
                        cmdOutput = p.StandardOutput.ReadLine();
                        if (cmdOutput.Contains(dotnetcore))
                        {
                            string version = cmdOutput.Substring(cmdOutput.IndexOf(dotnetcore, StringComparison.Ordinal) + dotnetcore.Length + 1);
                            if (version.Contains("["))
                            {
                                 
                                version = version.Substring(0, version.LastIndexOf("[", StringComparison.Ordinal));
                                if(Version.TryParse(version, out Version outVersion))
                                {
                                    if(outVersion >= minVersion && outVersion < maxVersion)
                                    {
                                        dotNetCoreOK = true;
                                        break;
                                    }
                                }
                            }
                        }
                    } while (cmdOutput.Length>1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (!dotNetCoreOK)
            {
                Console.WriteLine(@"Unable to find .NetCore runtime installed. Versions supported: <{0},{1})!!!", minDotNetCoreSupportedVersionIncluded.ToString(), maxDotNetCoreSupportedVersionExcluded.ToString());
                Console.WriteLine("Press any key to close the application!!!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            return dotNetCoreOK;
        }
    }
}


