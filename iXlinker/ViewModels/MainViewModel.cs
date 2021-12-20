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
            bool doNotGenerateDisabled; 

            tsProjFilePath = @"D:\_tmp\TwinCAT Project4\TwinCAT Project4\TwinCAT Project4.tsproj";
            activeTargetPlatform = "Release|TwinCAT RT (x64)";
            plcProjFilePath = @"D:\_tmp\TwinCAT Project4\TwinCAT Project4\Untitled1\Untitled1.plcproj";
            doNotGenerateDisabled = true;

            //tsProjFilePath = @"D:\_tmp\TwinCAT Project1\TwinCAT Project1\TwinCAT Project1.tsproj";
            //activeTargetPlatform = "Release|TwinCAT RT (x86)";
            //plcProjFilePath = @"D:\_tmp\TwinCAT Project1\TwinCAT Project1\Untitled1\Untitled1.plcproj";
            //doNotGenerateDisabled = true;


            if (i == 0)
            {
                Console.WriteLine("No argument passed, default projectpath is going to be used.");

                List<string> list = new List<string>();
                list.Add(tsProjFilePath);
                list.Add(activeTargetPlatform);
                list.Add(plcProjFilePath);
                list.Add(doNotGenerateDisabled.ToString());
                String[] str = list.ToArray();
                args = str;
            }

            if (args != null)
            {
                i = args.Length;
                if (i == 1)
                {
                    TsProjFilePath = args[0];
                    ActiveTargetPlatform = "";
                    PlcProjFilePath = "";
                    DoNotGenerateDisabled = true;
                }
                else if (i == 2)
                {
                    TsProjFilePath = args[0];
                    ActiveTargetPlatform = args[1];
                    PlcProjFilePath = "";
                    DoNotGenerateDisabled = true;
                }
                else if (i == 3)
                {
                    TsProjFilePath = args[0];
                    ActiveTargetPlatform = args[1];
                    PlcProjFilePath = args[2];
                    DoNotGenerateDisabled = true;
                }
                else if (i == 4)
                {
                    TsProjFilePath = args[0];
                    ActiveTargetPlatform = args[1];
                    PlcProjFilePath = args[2];
                    DoNotGenerateDisabled = !args[3].ToLower().Contains("false");
                }
                if (i>=1 && i<=4)
                {
                    if (File.Exists(TsProjFilePath))
                    {
                        Console.WriteLine("Opening file :" + TsProjFilePath);

                        GenerateOutputsIntoTwincatProject(TsProjFilePath, ActiveTargetPlatform, PlcProjFilePath);

                        Console.WriteLine("Done");
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine(@"File ""{0}"" not found. Check the path and file name of the Twincat project!!!", TsProjFilePath);
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