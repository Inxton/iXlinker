using System;
using System.Collections.Generic;
using System.Diagnostics;
using ViewModels;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        public static void OpenSolution(VisualStudioDTEViewModel vs)
        {
            string slnPath = vs.Sln.CompletePathInFileSystem;
            if (!string.IsNullOrEmpty(slnPath))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                startInfo.FileName = vs.Devenv;
                startInfo.Arguments = @"/useenv """ + slnPath + @"""";

                Console.WriteLine(@"Opening solution: ""{0}"" !!!", slnPath);
                Stopwatch sw = new Stopwatch();
                sw.Start();

                process.StartInfo = startInfo;
                process.Start();

                sw.Stop();
                Console.WriteLine(@"Solution: ""{0}"" open!!!", slnPath);
                Console.WriteLine(" in {0} ms!!!", sw.ElapsedMilliseconds);



            }
            else
            {
                Console.WriteLine(@"!!!Unable to find solution: ""{0}"" !!!", slnPath);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}