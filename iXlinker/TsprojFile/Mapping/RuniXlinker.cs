using ViewModels;
using TwincatXmlSchemas.TcPlcProj;
using iXlinker.TsprojFile.Mapping;
using System.Threading;
//using iXlinker.Utils;
using System.Diagnostics;
using System;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        public void RuniXlinker(string tsProjFilePath, string activeTargetPlatform, string plcProjFilePath,bool doNotGenerateDisabled, string devenvPath)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Get details like paths, platform etc.
            SolutionViewModel vs = VS.GetXaeProjectDetails(tsProjFilePath, activeTargetPlatform, plcProjFilePath, doNotGenerateDisabled, devenvPath);

            //Search all devices and their boxes in the Twincat project.
            SearchDevices(vs);
            //Generate all structures and mappings           
            GenerateStructures(vs);
            //Build XAE project
            VS.BuildProjectUsingCli(vs, VS.TcXaeObject.XAE_project);
            //Generate mappings and write them into the .tsproj file.
            GenerateMappingsToTsProj(vs);
            //Build XAE project
            //VS.BuildProjectUsingCli(vs, VS.TcXaeObject.XAE_project);

            sw.Stop();
            Console.WriteLine("Complete process {0} ms!!!", sw.ElapsedMilliseconds);
            //Console.ReadLine();
        }
    }
}
