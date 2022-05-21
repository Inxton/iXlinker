using System;
using System.Diagnostics;
using System.Windows;

namespace iXlinkerExt
{
    partial class Prerequisites
    {
        internal static bool CheckDotNetCore()
        {
            bool dotNetCoreOK = false;

            Version minVersion = new Version(Constants.minDotNetCoreSupportedVersionIncluded);
            Version maxVersion = new Version(Constants.maxDotNetCoreSupportedVersionExcluded);

            try
            {
                using (Process p = new Process())
                {
                    string cmd = "dotnet --list-runtimes";
                    string cmdOutput = "";
                    string dotnetcore = Constants.dotnetcore;
                    p.StartInfo = new ProcessStartInfo("cmd.exe")
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
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
                            string version = cmdOutput.Substring(cmdOutput.IndexOf(dotnetcore) + dotnetcore.Length + 1);
                            if (version.Contains("["))
                            {

                                version = version.Substring(0, version.LastIndexOf("["));
                                if (Version.TryParse(version, out Version outVersion))
                                {
                                    if (outVersion >= minVersion && outVersion < maxVersion)
                                    {
                                        dotNetCoreOK = true;
                                        break;
                                    }
                                }
                            }
                        }
                    } while (cmdOutput.Length > 1);
                    p.CloseMainWindow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "iXlinker", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!dotNetCoreOK)
            {
                MessageBox.Show("Unable to find supported version of the .NetCore runtime installed." + Environment.NewLine + "Installed version needs to be higher or equal to: {" + minVersion.ToString() + "}" + Environment.NewLine + "and lower then:{" + maxVersion.ToString() + "}!!!", "iXlinker", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return dotNetCoreOK;
        }

    }
}
