using System;
using System.IO;
using TsprojFile.Scan;
using System.Collections.Generic;
using iXlinker.Utils;

namespace iXlinkerDtos
{
    public class MainViewModel : ScanTcProjFile
    {
        public MainViewModel(string[] args)
        {
            int i = args.Length;

            string tsProjFilePath = null;
            string activeTargetPlatform = null;
            string plcProjFilePath = null;
            bool doNotGenerateDisabled = true;
            string devenvPath = null;
            ushort defaultMaxFrameIndex = 0;

            string defaultTsProjFilePath = null;
            string defaultActiveTargetPlatform = null;
            string defaultPlcProjFilePath = null;
            bool defaultDoNotGenerateDisabled;
            string defaultDevenvPath = null;
            ushort maxFrameIndex = 0;

            defaultTsProjFilePath = @"C:\x_tmp\TwinCAT Project4\TwinCAT Project4\TwinCAT Project4.tsproj";
            defaultActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            defaultPlcProjFilePath = @"C:\x_tmp\TwinCAT Project4\TwinCAT Project4\Untitled1\Untitled1.plcproj";
            defaultDoNotGenerateDisabled = true;
            defaultDevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";
            defaultMaxFrameIndex = 0;

            if (i == 0)
            {
                EventLogger.Instance.Logger.Information("No argument passed, default projectpath is going to be used.");

                List<string> list = new List<string>();
                list.Add(defaultTsProjFilePath);
                list.Add(defaultActiveTargetPlatform);
                list.Add(defaultPlcProjFilePath);
                list.Add(defaultDoNotGenerateDisabled.ToString());
                list.Add(defaultDevenvPath);
                list.Add(defaultMaxFrameIndex.ToString());
                String[] str = list.ToArray();
                args = str;
            }

            if (args != null)
            {
                i = args.Length;
                if (i == 1)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = "";
                    plcProjFilePath = "";
                    doNotGenerateDisabled = true;
                    devenvPath = "";
                    maxFrameIndex = 0;
                }
                else if (i == 2)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = "";
                    doNotGenerateDisabled = true;
                    devenvPath = "";
                    maxFrameIndex = 0;
                }
                else if (i == 3)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = true;
                    devenvPath = "";
                    maxFrameIndex = 0;
                }
                else if (i == 4)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = !args[3].ToLower().Contains("false");
                    devenvPath = "";
                    maxFrameIndex = 0;
                }
                else if (i == 5)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = !args[3].ToLower().Contains("false");
                    devenvPath = args[4];
                    maxFrameIndex = 0;
                }
                else if (i == 6)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = !args[3].ToLower().Contains("false");
                    devenvPath = args[4];
                    ushort.TryParse(args[5],out maxFrameIndex);
                }
                if (i>=1 && i<=6)
                {
                    if (File.Exists(tsProjFilePath))
                    {
                        EventLogger.Instance.Logger.Information("Opening file :" + tsProjFilePath);

                        RuniXlinker(tsProjFilePath, activeTargetPlatform, plcProjFilePath, doNotGenerateDisabled, devenvPath, maxFrameIndex);

                        EventLogger.Instance.Logger.Information("Done");
                    }
                    else
                    {
                        EventLogger.Instance.Logger.Information(@"File ""{0}"" not found. Check the path and file name of the Twincat project!!!", tsProjFilePath);
                        Environment.Exit(0);
                    }
                }
            }

            else
            {
                EventLogger.Instance.Logger.Information("Invalid arguments");
            }
        }
    }
}