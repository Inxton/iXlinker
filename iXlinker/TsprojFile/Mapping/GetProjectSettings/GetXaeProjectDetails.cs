using System;
using System.IO;
using ViewModels;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        public static VisualStudioDTEViewModel GetXaeProjectDetails(string tsProjFilePath, string activeTargetPlatform, string plcProjFilePath)
        {
            VisualStudioDTEViewModel vs = new VisualStudioDTEViewModel();

            //Tsproj details
            if (File.Exists(tsProjFilePath))
            {
                vs.TsProject = new EnvDteTsProjectViewModel();
                vs.TsProject.Details = new ProjectItemViewModel();
                vs.TsProject.Details.CompletePathInFileSystem = tsProjFilePath;
                vs.TsProject.Details.FolderPathInFileSystem = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\"));
                vs.TsProject.Details.FileNameInFileSystem = tsProjFilePath.Substring(tsProjFilePath.LastIndexOf("\\") + 1);

                //Solution
                string slnPath = GetSolutionPathFromTsprojPath(tsProjFilePath);
                if (!string.IsNullOrEmpty(slnPath))
                {
                    ProjectItemViewModel sln = new ProjectItemViewModel();
                    sln.CompletePathInFileSystem = slnPath;
                    sln.FolderPathInFileSystem = slnPath.Substring(0, slnPath.LastIndexOf("\\"));
                    sln.FileNameInFileSystem = slnPath.Substring(slnPath.LastIndexOf("\\") + 1);
                    sln.Name = sln.Name ?? slnPath.Substring(slnPath.LastIndexOf("\\") + 1).Replace(".sln", "").Replace(".slnf", "");
                    sln.Path = "";
                    sln.CompleteName = sln.Name;
                    vs.Sln = sln;
                }

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
                        vs.PlcProject = new EnvDtePlcProjectViewModel();
                        vs.PlcProject.Details = new ProjectItemViewModel();
                        vs.PlcProject.Details.CompletePathInFileSystem = plcProjFilePath;
                        vs.PlcProject.Details.FolderPathInFileSystem = plcProjFilePath.Substring(0, plcProjFilePath.LastIndexOf("\\"));
                        vs.PlcProject.Details.FileNameInFileSystem = plcProjFilePath.Substring(plcProjFilePath.LastIndexOf("\\") + 1);
                        vs.PlcProject.Details.Name = vs.PlcProject.Details.FileNameInFileSystem.Replace(".plcproj", "");
                        vs.PlcProject.Details.Path = vs.PlcProject.Details.FolderPathInFileSystem.Replace(vs.TsProject.Details.FolderPathInFileSystem + "\\", "");
                        vs.PlcProject.Details.CompleteName = vs.PlcProject.Details.Path + "\\" + vs.PlcProject.Details.FileNameInFileSystem;

                        vs.GvlExported = new ProjectItemViewModel() { Name = "GVL_iXlinker" };
                        vs.GvlExported.Path = "GVLs";
                        vs.GvlExported.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.GvlExported.Path;
                        vs.GvlExported.CompleteName = vs.PlcProject.Details.CompleteName + "\\" + vs.GvlExported.Path + "\\" + vs.GvlExported.Name;
                        vs.GvlExported.FileNameInFileSystem = vs.GvlExported.Name + ".TcGVL";
                        vs.GvlExported.CompletePathInFileSystem = vs.GvlExported.FolderPathInFileSystem + "\\" + vs.GvlExported.FileNameInFileSystem;
                        TcModel.NameOfTheExportedGVL = vs.GvlExported.Name;

                        vs.DutsIo = new ProjectItemViewModel() { Name = "DUTs_IO_folder" };
                        vs.DutsIo.Path = "DUTs\\IO";
                        vs.DutsIo.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.DutsIo.Path;

                        vs.DutsIoPdoEntry = new ProjectItemViewModel() { Name = "DUTs_IO_PdoEntry_folder" };
                        vs.DutsIoPdoEntry.Path = vs.DutsIo.Path + "\\PdoEntries";
                        vs.DutsIoPdoEntry.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.DutsIoPdoEntry.Path;

                        vs.DutsIoPdo = new ProjectItemViewModel() { Name = "DUTs_IO_Pdo_folder" };
                        vs.DutsIoPdo.Path = vs.DutsIo.Path + "\\PDOs";
                        vs.DutsIoPdo.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.DutsIoPdo.Path;

                        vs.DutsIoBox = new ProjectItemViewModel() { Name = "DUTs_IO_Box_folder" };
                        vs.DutsIoBox.Path = vs.DutsIo.Path + "\\Boxes";
                        vs.DutsIoBox.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.DutsIoBox.Path;

                        vs.DutsIoDevice = new ProjectItemViewModel() { Name = "DUTs_IO_Device_folder" };
                        vs.DutsIoDevice.Path = vs.DutsIo.Path + "\\Devices";
                        vs.DutsIoDevice.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.DutsIoDevice.Path;

                        vs.DutsIoTopology = new ProjectItemViewModel() { Name = "DUTs_IO_Topology_folder" };
                        vs.DutsIoTopology.Path = vs.DutsIo.Path + "\\Topology";
                        vs.DutsIoTopology.FolderPathInFileSystem = vs.PlcProject.Details.FolderPathInFileSystem + "\\" + vs.DutsIoTopology.Path;
                    }
                }
                else
                {
                    Console.WriteLine(@"File ""{0}"" not found. Check the path and file name of the PLC project!!!", plcProjFilePath);
                    Console.WriteLine("Press any key to close the application!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                //Devenv
                Version minSupportedVersion = new Version(16, 0, 0, 0);
                Version maxSupportedVersion = new Version(16, 65536, 65536, 65536);

                string devenPath = GetDevenvPath(minSupportedVersion, maxSupportedVersion);
                if (string.IsNullOrEmpty(devenPath))
                {
                    Console.WriteLine(@"Unable to discover Visual Studio installed. Versions supported: <{0},{1}>!!!", minSupportedVersion.ToString(), maxSupportedVersion.ToString());
                    Console.WriteLine("Press any key to close the application!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                vs.Devenv = devenPath;
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