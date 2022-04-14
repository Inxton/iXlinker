using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;
using iXlinker.TsprojFile.Mapping;
using System.Threading;
using System.Diagnostics;
using System;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        public void RuniXlinker(string tsProjFilePath, string activeTargetPlatform, string plcProjFilePath,bool doNotGenerateDisabled, string devenvPath, ushort maxEthercatFrameIndex)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            VS.CheckDotNetCore();

            //Get details like paths, platform etc.
            Solution vs = VS.GetXaeProjectDetails(tsProjFilePath, activeTargetPlatform, plcProjFilePath, doNotGenerateDisabled, devenvPath, maxEthercatFrameIndex);

            //Search all devices and their boxes in the Twincat project.
            SearchDevices(vs);

            //Environment.Exit(0);

            //Generate all structures and mappings           
            GenerateStructures(vs);
            //Build XAE project
            VS.BuildXaeProjectUsingCli(vs);
            //Generate mappings and write them into the .tsproj file.
            GenerateMappingsToTsProj(vs);

            sw.Stop();
            EventLogger.Instance.Logger.Information("Complete process {0} ms!!!", sw.ElapsedMilliseconds);
        }
    }
}
