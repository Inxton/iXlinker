using System;
using System.Diagnostics;
using System.Threading;
using iXlinker.Utils;
using iXlinkerDtos;

namespace iXlinker.TsprojFile.Mapping
{
    internal partial class VS
    {
        public static void BuildXaeProjectUsingCli(Solution vs)
        {
            string startMessage = null;
            string endMessage = null;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = vs.DevenvPath;

            Stopwatch sw = new Stopwatch();

            startMessage = @"Building XAE project: """ + vs.TsProject.CompletePathInFileSystem + @""" !!!";
            endMessage = @"XAE project: """ + vs.TsProject.CompletePathInFileSystem + @""" built";
            startInfo.Arguments = @"""" + vs.TsProject.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.TsProject.FolderPathInFileSystem + @"\\build.txt""";

            process.StartInfo = startInfo;
            EventLogger.Instance.Logger.Information(@"Starting process Filename: ""{0}"", Arguments: ""{1}""", startInfo.FileName, startInfo.Arguments);
            EventLogger.Instance.Logger.Information(startMessage);
            sw.Start();
            process.Start();
            process.WaitForExit();
            sw.Stop();
            EventLogger.Instance.Logger.Information(endMessage);
            EventLogger.Instance.Logger.Information(" in {0} ms!!!", sw.ElapsedMilliseconds);
           }
    }
}
