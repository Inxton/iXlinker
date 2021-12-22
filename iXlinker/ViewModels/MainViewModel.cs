using System;
using System.IO;
using TsprojFile.Scan;
using System.Collections.Generic;

namespace ViewModels
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

            string defaultTsProjFilePath = null;
            string defaultActiveTargetPlatform = null;
            string defaultPlcProjFilePath = null;
            bool defaultDoNotGenerateDisabled;
            string defaultDevenvPath = null;

            defaultTsProjFilePath = @"C:\x_tmp\TwinCAT Project4\TwinCAT Project4\TwinCAT Project4.tsproj";
            defaultActiveTargetPlatform = "Release|TwinCAT RT (x64)";
            defaultPlcProjFilePath = @"C:\x_tmp\TwinCAT Project4\TwinCAT Project4\Untitled1\Untitled1.plcproj";
            defaultDoNotGenerateDisabled = true;
            defaultDevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";

            //defaultTsProjFilePath = @"C:\x_tmp\TwinCAT Project1\TwinCAT Project1\TwinCAT Project1.tsproj";
            //defaultActiveTargetPlatform = "Release|TwinCAT RT (x86)";
            //defaultPlcProjFilePath = @"C:\x_tmp\TwinCAT Project1\TwinCAT Project1\Untitled1\Untitled1.plcproj";
            //defaultDoNotGenerateDisabled = true;
            //defaultDevenvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com";


            if (i == 0)
            {
                Console.WriteLine("No argument passed, default projectpath is going to be used.");

                List<string> list = new List<string>();
                list.Add(defaultTsProjFilePath);
                list.Add(defaultActiveTargetPlatform);
                list.Add(defaultPlcProjFilePath);
                list.Add(defaultDoNotGenerateDisabled.ToString());
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
                }
                else if (i == 2)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = "";
                    doNotGenerateDisabled = true;
                    devenvPath = "";
                }
                else if (i == 3)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = true;
                    devenvPath = "";
                }
                else if (i == 4)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = !args[3].ToLower().Contains("false");
                    devenvPath = "";
                }
                else if (i == 5)
                {
                    tsProjFilePath = args[0];
                    activeTargetPlatform = args[1];
                    plcProjFilePath = args[2];
                    doNotGenerateDisabled = !args[3].ToLower().Contains("false");
                    devenvPath = args[4];
                }
                if (i>=1 && i<=5)
                {
                    if (File.Exists(tsProjFilePath))
                    {
                        Console.WriteLine("Opening file :" + tsProjFilePath);

                        RuniXlinker(tsProjFilePath, activeTargetPlatform, plcProjFilePath, doNotGenerateDisabled, devenvPath);

                        Console.WriteLine("Done");
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine(@"File ""{0}"" not found. Check the path and file name of the Twincat project!!!", tsProjFilePath);
                        Console.WriteLine("Press any key to close the application!!!");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }

            else
            {
                Console.WriteLine("Invalid arguments");
                Console.ReadLine();
            }
        }
    }
}