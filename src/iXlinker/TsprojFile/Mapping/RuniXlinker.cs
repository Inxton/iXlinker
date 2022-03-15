using iXlinkerDtos;
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
        public void RuniXlinker(string tsProjFilePath, string activeTargetPlatform, string plcProjFilePath,bool doNotGenerateDisabled, string devenvPath, ushort maxEthercatFrameIndex)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //VS.CheckDotNetCore();

            //Get details like paths, platform etc.
            Solution vs = VS.GetXaeProjectDetails(tsProjFilePath, activeTargetPlatform, plcProjFilePath, doNotGenerateDisabled, devenvPath, maxEthercatFrameIndex);

            //Search all devices and their boxes in the Twincat project.
            SearchDevices(vs);
            //Generate all structures and mappings           
            GenerateStructures(vs);
            //Build XAE project
            VS.BuildXaeProjectUsingCli(vs);
            //Generate mappings and write them into the .tsproj file.
            GenerateMappingsToTsProj(vs);

            sw.Stop();
            Console.WriteLine("Complete process {0} ms!!!", sw.ElapsedMilliseconds);
            //Console.ReadLine();
        }
    }
}
