using System;
using System.IO;
using iXlinkerDtos;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        public static Solution GetXaeProjectDetails(string tsProjFilePath, string activeTargetPlatform, string plcProjFilePath, bool doNotGenerateDisabled, string devenvPath)
        {
            Solution vs = new Solution();

            //Tsproj details
            if (File.Exists(tsProjFilePath))
            {
                vs.TsProject = new TsProject();
                vs.TsProject.CompletePathInFileSystem = tsProjFilePath;
                vs.TsProject.FolderPathInFileSystem = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\", StringComparison.Ordinal));
                vs.TsProject.FileNameInFileSystem = tsProjFilePath.Substring(tsProjFilePath.LastIndexOf("\\", StringComparison.Ordinal) + 1);

                //Active target platform
                string defaultTargetPlatform = "Release|TwinCAT RT (x64)";
                if (string.IsNullOrEmpty(activeTargetPlatform))
                {
                    Console.WriteLine(@"No Active Target Platform is defined! The default one: ""{0}"" is going to be used!!!", defaultTargetPlatform);
                    vs.ActiveTargetPlatform = defaultTargetPlatform;
                }
                else
                {
                    if (ValidateActiveTargetPlatform(activeTargetPlatform))
                    {
                        vs.ActiveTargetPlatform = activeTargetPlatform;
                    }
                    else
                    {
                        Console.WriteLine(@"Invalid Active Target Platform: ""{0}"" is defined! The default one: ""{1}"" is going to be used!!!", activeTargetPlatform, defaultTargetPlatform);
                        vs.ActiveTargetPlatform = defaultTargetPlatform;
                    }
                }

                //Plcproj details
                if (string.IsNullOrEmpty(plcProjFilePath))
                {
                    Console.WriteLine(@"No PLC project name is defined! If the XAE contains only one PLC project, this one is used.!!!");
                    plcProjFilePath = GetPlcprojFromXae(tsProjFilePath);
                }

                if (File.Exists(plcProjFilePath))
                {
                    if(CheckIfXaeContainsPlcproj(tsProjFilePath, plcProjFilePath))
                    {
                        vs.PlcProject = new PlcProject();
                        vs.PlcProject.CompletePathInFileSystem = plcProjFilePath;
                        vs.PlcProject.FolderPathInFileSystem = plcProjFilePath.Substring(0, plcProjFilePath.LastIndexOf("\\", StringComparison.Ordinal));
                        vs.PlcProject.FileNameInFileSystem = plcProjFilePath.Substring(plcProjFilePath.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        vs.PlcProject.Name = vs.PlcProject.FileNameInFileSystem.Replace(".plcproj", "");
                        vs.PlcProject.Path = vs.PlcProject.FolderPathInFileSystem.Replace(vs.TsProject.FolderPathInFileSystem + "\\", "");
                        vs.PlcProject.CompleteName = vs.PlcProject.Path + "\\" + vs.PlcProject.FileNameInFileSystem;

                        vs.GvlExported = new ProjectItem() { Name = "GVL_iXlinker" };
                        vs.GvlExported.Path = "GVLs";
                        vs.GvlExported.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.GvlExported.Path;
                        vs.GvlExported.CompleteName = vs.PlcProject.CompleteName + "\\" + vs.GvlExported.Path + "\\" + vs.GvlExported.Name;
                        vs.GvlExported.FileNameInFileSystem = vs.GvlExported.Name + ".TcGVL";
                        vs.GvlExported.CompletePathInFileSystem = vs.GvlExported.FolderPathInFileSystem + "\\" + vs.GvlExported.FileNameInFileSystem;
                        TcModel.NameOfTheExportedGVL = vs.GvlExported.Name;

                        vs.DutsIo = new ProjectItem() { Name = "DUTs_IO_folder" };
                        vs.DutsIo.Path = "DUTs\\IO";
                        vs.DutsIo.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.DutsIo.Path;

                        vs.DutsIoPdoEntry = new ProjectItem() { Name = "DUTs_IO_PdoEntry_folder" };
                        vs.DutsIoPdoEntry.Path = vs.DutsIo.Path + "\\PdoEntries";
                        vs.DutsIoPdoEntry.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.DutsIoPdoEntry.Path;

                        vs.DutsIoPdo = new ProjectItem() { Name = "DUTs_IO_Pdo_folder" };
                        vs.DutsIoPdo.Path = vs.DutsIo.Path + "\\PDOs";
                        vs.DutsIoPdo.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.DutsIoPdo.Path;

                        vs.DutsIoBox = new ProjectItem() { Name = "DUTs_IO_Box_folder" };
                        vs.DutsIoBox.Path = vs.DutsIo.Path + "\\Boxes";
                        vs.DutsIoBox.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.DutsIoBox.Path;

                        vs.DutsIoDevice = new ProjectItem() { Name = "DUTs_IO_Device_folder" };
                        vs.DutsIoDevice.Path = vs.DutsIo.Path + "\\Devices";
                        vs.DutsIoDevice.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.DutsIoDevice.Path;

                        vs.DutsIoTopology = new ProjectItem() { Name = "DUTs_IO_Topology_folder" };
                        vs.DutsIoTopology.Path = vs.DutsIo.Path + "\\Topology";
                        vs.DutsIoTopology.FolderPathInFileSystem = vs.PlcProject.FolderPathInFileSystem + "\\" + vs.DutsIoTopology.Path;
                    }
                }
                else
                {
                    Console.WriteLine(@"File ""{0}"" not found. Check the path and file name of the PLC project!!!", plcProjFilePath);
                    Console.WriteLine("Press any key to close the application!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                //DoNotGenerateDisabled 
                vs.DoNotGenerateDisabled = doNotGenerateDisabled;

                //Devenv
                if (string.IsNullOrEmpty(devenvPath))
                {
                    string _devenvPath = GetDevenvPath();
                    if (string.IsNullOrEmpty(_devenvPath))
                    {
                        Console.WriteLine(@"Unable to discover Visual Studio installed. Versions supported: <{0},{1})!!!", minVsSupportedVersionIncluded.ToString(), maxVsSupportedVersionExcluded.ToString());
                        Console.WriteLine("Press any key to close the application!!!");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    vs.DevenvPath = _devenvPath;
                }
                else if(File.Exists(devenvPath))
                {
                    vs.DevenvPath = devenvPath;
                }
                else
                {
                    Console.WriteLine(@"Unable to find file: ""{0}""!!!", devenvPath);
                    Console.WriteLine("Press any key to close the application!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine(@"File ""{0}"" not found. Check the path and file name of the XAE project!!!", tsProjFilePath);
                Console.WriteLine("Press any key to close the application!!!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return vs;
        }
    }
}