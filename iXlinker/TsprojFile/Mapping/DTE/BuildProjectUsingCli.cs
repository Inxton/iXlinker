using System;
using System.Diagnostics;
using System.Threading;
using ViewModels;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        public static void BuildProjectUsingCli(VisualStudioDTEViewModel vs , TcXaeObject tcXaeObject )
        {
            string startMessage = null;
            string endMessage = null;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = vs.Devenv;

            Stopwatch sw = new Stopwatch();
            switch (tcXaeObject)
            {
                case TcXaeObject.PLC_project:
                    startMessage = @"Building PLC project: """ + vs.PlcProject.Details.CompletePathInFileSystem + @""" !!!";
                    endMessage = @"PLC project: """ + vs.PlcProject.Details.CompletePathInFileSystem + @""" built";
                    startInfo.Arguments = @"/useenv """ + vs.PlcProject.Details.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.PlcProject.Details.FolderPathInFileSystem + @"\\build.txt""";
                    break;
                case TcXaeObject.XAE_project:
                    startMessage = @"Building XAE project: """ + vs.TsProject.Details.CompletePathInFileSystem + @""" !!!";
                    endMessage = @"XAE project: """ + vs.TsProject.Details.CompletePathInFileSystem + @""" built";
                    startInfo.Arguments = @"/useenv """ + vs.TsProject.Details.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.TsProject.Details.FolderPathInFileSystem + @"\\build.txt""";
                    break;
                case TcXaeObject.Solution:
                    startMessage = @"Building solution: """ + vs.Sln.CompletePathInFileSystem + @""" !!!";
                    endMessage = @"Solution: """ + vs.Sln.CompletePathInFileSystem + @""" built";
                    startInfo.Arguments = @"/useenv """ + vs.Sln.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.Sln.FolderPathInFileSystem + @"\\build.txt""";
                    break;
                default:
                    startMessage = @"Building solution: """ + vs.Sln.CompletePathInFileSystem + @""" !!!";
                    endMessage = @"Solution: """ + vs.Sln.CompletePathInFileSystem + @""" built";
                    startInfo.Arguments = @"/useenv """ + vs.Sln.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.Sln.FolderPathInFileSystem + @"\\build.txt""";
                    break;
            }
            process.StartInfo = startInfo;
            Console.WriteLine(@"Starting process Filename: ""{0}"", Arguments: ""{1}""", startInfo.FileName, startInfo.Arguments);
            Console.Write(startMessage);
            sw.Start();
            process.Start();
            process.WaitForExit();
            sw.Stop();
            Console.Write(endMessage);
            Console.WriteLine(" in {0} ms!!!", sw.ElapsedMilliseconds);
           }
    }
}
