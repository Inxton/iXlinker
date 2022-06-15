using System;
using System.IO;
using iXlinker.Utils;
using iXlinkerDtos;

namespace iXlinker.TsprojFile.Mapping
{
    internal partial class VS
    {
        public static Solution GetXaeProjectDetails(string tsProjFilePath, string activeTargetPlatform, string plcProjFilePath, bool doNotGenerateDisabled, string devenvPath, ushort maxEthercatFrameIndex)
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
                    EventLogger.Instance.Logger.Information(@"No Active Target Platform is defined! The default one: ""{0}"" is going to be used!!!", defaultTargetPlatform);
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
                        EventLogger.Instance.Logger.Information(@"Invalid Active Target Platform: ""{0}"" is defined! The default one: ""{1}"" is going to be used!!!", activeTargetPlatform, defaultTargetPlatform);
                        vs.ActiveTargetPlatform = defaultTargetPlatform;
                    }
                }

                //Plcproj details
                if (string.IsNullOrEmpty(plcProjFilePath))
                {
                    EventLogger.Instance.Logger.Information(@"No PLC project name is defined! If the XAE contains only one PLC project, this one is used.!!!");
                    GetPlcprojFromXae(vs);
                    plcProjFilePath = vs.PlcProject.Plcproj.CompletePathInFileSystem;
                }

                if (File.Exists(plcProjFilePath))
                {
                    if (vs.PlcProject == null)
                    {
                        vs.PlcProject = new PlcProject();
                        vs.PlcProject.Plcproj.CompletePathInFileSystem = plcProjFilePath;
                        vs.PlcProject.Plcproj.FolderPathInFileSystem = plcProjFilePath.Substring(0, plcProjFilePath.LastIndexOf("\\", StringComparison.Ordinal));
                        vs.PlcProject.Plcproj.FileNameInFileSystem = plcProjFilePath.Substring(plcProjFilePath.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        vs.PlcProject.Plcproj.Name = vs.PlcProject.Plcproj.FileNameInFileSystem.Replace(".plcproj", "");
                        vs.PlcProject.Plcproj.Path = vs.PlcProject.Plcproj.FolderPathInFileSystem.Replace(vs.TsProject.FolderPathInFileSystem + "\\", "");
                        vs.PlcProject.Plcproj.CompleteName = vs.PlcProject.Plcproj.Path + "\\" + vs.PlcProject.Plcproj.FileNameInFileSystem;
                    }
                    if (CheckIfXaeContainsPlcproj(tsProjFilePath, vs.PlcProject))
                    {
                        vs.GvlExported = new ProjectItem() { Name = "GVL_iXlinker" };
                        vs.GvlExported.Path = "GVLs";
                        vs.GvlExported.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.GvlExported.Path;
                        vs.GvlExported.CompleteName = vs.PlcProject.Plcproj.CompleteName + "\\" + vs.GvlExported.Path + "\\" + vs.GvlExported.Name;
                        vs.GvlExported.FileNameInFileSystem = vs.GvlExported.Name + ".TcGVL";
                        vs.GvlExported.CompletePathInFileSystem = vs.GvlExported.FolderPathInFileSystem + "\\" + vs.GvlExported.FileNameInFileSystem;
                        TcModel.NameOfTheExportedGVL = vs.GvlExported.Name;

                        vs.DutsIo = new ProjectItem() { Name = "DUTs_IO_folder" };
                        vs.DutsIo.Path = "DUTs\\IO";
                        vs.DutsIo.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIo.Path;

                        vs.DutsIoPdoEntry = new ProjectItem() { Name = "DUTs_IO_PdoEntry_folder" };
                        vs.DutsIoPdoEntry.Path = vs.DutsIo.Path + "\\PdoEntries";
                        vs.DutsIoPdoEntry.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIoPdoEntry.Path;

                        vs.DutsIoPdo = new ProjectItem() { Name = "DUTs_IO_Pdo_folder" };
                        vs.DutsIoPdo.Path = vs.DutsIo.Path + "\\PDOs";
                        vs.DutsIoPdo.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIoPdo.Path;

                        vs.DutsIoBox = new ProjectItem() { Name = "DUTs_IO_Box_folder" };
                        vs.DutsIoBox.Path = vs.DutsIo.Path + "\\Boxes";
                        vs.DutsIoBox.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIoBox.Path;

                        vs.DutsIoDevice = new ProjectItem() { Name = "DUTs_IO_Device_folder" };
                        vs.DutsIoDevice.Path = vs.DutsIo.Path + "\\Devices";
                        vs.DutsIoDevice.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIoDevice.Path;

                        vs.DutsIoTopology = new ProjectItem() { Name = "DUTs_IO_Topology_folder" };
                        vs.DutsIoTopology.Path = vs.DutsIo.Path + "\\Topology";
                        vs.DutsIoTopology.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIoTopology.Path;

                        vs.DutsIoBase = new ProjectItem() { Name = "DUTs_Base_folder" };
                        vs.DutsIoBase.Path = vs.DutsIo.Path + "\\Base";
                        vs.DutsIoBase.FolderPathInFileSystem = vs.PlcProject.Plcproj.FolderPathInFileSystem + "\\" + vs.DutsIoBase.Path;
                    }
                }
                else
                {
                    EventLogger.Instance.Logger.Information(@"File ""{0}"" not found. Check the path and file name of the PLC project!!!", plcProjFilePath);
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
                        EventLogger.Instance.Logger.Information(@"Unable to discover Visual Studio installed. Versions supported: <{0},{1})!!!", minVsSupportedVersionIncluded.ToString(), maxVsSupportedVersionExcluded.ToString());
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
                    EventLogger.Instance.Logger.Information(@"Unable to find file: ""{0}""!!!", devenvPath);
                    Environment.Exit(0);
                }
                //MaxEthercatFrameIndex 
                vs.MaxEthercatFrameIndex = maxEthercatFrameIndex;

                //Get list of all PLC library repositories
                vs.PlcLibRepositories = GetPlcLibraryRepositories();

            }
            else
            {
                EventLogger.Instance.Logger.Information(@"File ""{0}"" not found. Check the path and file name of the XAE project!!!", tsProjFilePath);
                Environment.Exit(0);
            }
            return vs;
        }
    }
}