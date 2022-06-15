using iXlinker.Utils;
using iXlinkerDtos;
using System;
using System.IO;
using System.Xml.Serialization;
using TwincatXmlSchemas.TcSmProject;


namespace iXlinker.TsprojFile.Mapping
{
    internal partial class VS
    {
        private static void GetPlcprojFromXae(Solution vs)
        {
            string tsProjFilePath = vs.TsProject.CompletePathInFileSystem;

            PlcProject plcProject = new PlcProject();

            string plcProjFilePath = null;

            XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
            StreamReader reader = new StreamReader(tsProjFilePath);


            try
            {
                TcSmProject tc = (TcSmProject)serializer.Deserialize(reader);
                reader.Close();
                if (tc != null && tc.Project != null && tc.Project.Plc == null)
                {
                    EventLogger.Instance.Logger.Information(@"No PLC project found in the XAE project:  ""{0}""!!!", tsProjFilePath);
                    Environment.Exit(0);
                }
                if (tc != null && tc.Project != null && tc.Project.Plc != null && tc.Project.Plc.Project != null)
                {
                    if (tc.Project.Plc.Project.Length > 1)
                    {
                        EventLogger.Instance.Logger.Information(@"Multiple PLC projects found in the XAE project:  ""{0}""!!!", tsProjFilePath);
                        EventLogger.Instance.Logger.Information(@"Restart the application with the exact PLC project path and file name specified!!!");
                        Environment.Exit(0);
                    }
                    
                    if (tc.Project.Plc.Project.Length == 1 && tc.Project.Plc.Project[0] != null && !string.IsNullOrEmpty(tc.Project.Plc.Project[0].PrjFilePath))
                    {
                        string tsProjFolder = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\", StringComparison.Ordinal));
                        plcProjFilePath = tsProjFolder + "\\" + tc.Project.Plc.Project[0].PrjFilePath;
                        EventLogger.Instance.Logger.Information(@"PLC project: ""{0}"" found in the XAE project: ""{1}"" will be used!!!", plcProjFilePath, tsProjFilePath);
                        plcProject.Plcproj.CompletePathInFileSystem = plcProjFilePath;
                        plcProject.Plcproj.FolderPathInFileSystem = new DirectoryInfo(plcProjFilePath).Parent.ToString();
                        plcProject.Plcproj.FileNameInFileSystem = new DirectoryInfo(plcProjFilePath).Name.ToString();
                        plcProject.Plcproj.Name = plcProject.Plcproj.FileNameInFileSystem.Replace(".plcproj", "");
                        plcProject.Plcproj.Path = plcProject.Plcproj.FolderPathInFileSystem.Replace(vs.TsProject.FolderPathInFileSystem + "\\", "");
                        plcProject.Plcproj.CompleteName = plcProject.Plcproj.Path + "\\" + plcProject.Plcproj.FileNameInFileSystem;
                        plcProject.IsIndependent = false;
                        EventLogger.Instance.Logger.Information(@"PLC project: ""{0}"" found in the XAE project: ""{1}"" will be used!!!", plcProjFilePath , tsProjFilePath);
                    }

                    else if (tc.Project.Plc.Project.Length == 1 && tc.Project.Plc.Project[0] != null && string.IsNullOrEmpty(tc.Project.Plc.Project[0].PrjFilePath) && !string.IsNullOrEmpty(tc.Project.Plc.Project[0].File))
                    {
                        string tsProjFolder = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\", StringComparison.Ordinal));

                        plcProject.IsIndependent = true;
                        plcProject.Xti.FileNameInFileSystem = tc.Project.Plc.Project[0].File;
                        plcProject.Xti.FolderPathInFileSystem = tsProjFolder + "\\_Config\\PLC";
                        plcProject.Xti.CompletePathInFileSystem = plcProject.Xti.FolderPathInFileSystem + "\\" + plcProject.Xti.FileNameInFileSystem;
                        plcProject.Xti.Name = plcProject.Xti.FileNameInFileSystem.Replace(".xti", "");
                        plcProject.Xti.Path = plcProject.Xti.FolderPathInFileSystem.Replace(vs.TsProject.FolderPathInFileSystem + "\\", "");
                        plcProject.Xti.CompleteName = plcProject.Xti.Path + "\\" + plcProject.Xti.FileNameInFileSystem;

                        GetPlcprojFromXti(plcProject);

                        EventLogger.Instance.Logger.Information(@"PLC project: ""{0}"" found in the XAE project: ""{1}"" will be used!!!", plcProject.Plcproj.CompletePathInFileSystem, tsProjFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            vs.PlcProject = plcProject;
        }
    }
}
