using System;
using System.Diagnostics;
using System.Threading;
using ViewModels;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        public static void BuildProjectUsingCli(SolutionViewModel vs , TcXaeObject tcXaeObject )
        {
            string startMessage = null;
            string endMessage = null;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = vs.DevenvPath;

            Stopwatch sw = new Stopwatch();
            switch (tcXaeObject)
            {
                case TcXaeObject.PLC_project:
                    startMessage = @"Building PLC project: """ + vs.PlcProject.CompletePathInFileSystem + @""" !!!";
                    endMessage = @"PLC project: """ + vs.PlcProject.CompletePathInFileSystem + @""" built";
                    startInfo.Arguments = @"/useenv """ + vs.PlcProject.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.PlcProject.FolderPathInFileSystem + @"\\build.txt""";
                    break;
                case TcXaeObject.XAE_project:
                    startMessage = @"Building XAE project: """ + vs.TsProject.CompletePathInFileSystem + @""" !!!";
                    endMessage = @"XAE project: """ + vs.TsProject.CompletePathInFileSystem + @""" built";
                    startInfo.Arguments = @"/useenv """ + vs.TsProject.CompletePathInFileSystem + @""" /build """ + vs.ActiveTargetPlatform + @""" /Out """ + vs.TsProject.FolderPathInFileSystem + @"\\build.txt""";
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
