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
        internal static bool CheckIfXaeContainsPlcproj(string tsProjFilePath, PlcProject plcProject )
        {
            string plcProjFilePath = plcProject.Plcproj.CompletePathInFileSystem;

            bool ret = false;

            XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
            StreamReader reader = new StreamReader(tsProjFilePath);

            try
            {
                TcSmProject tc = (TcSmProject)serializer.Deserialize(reader);
                reader.Close();

                if (tc != null && tc.Project != null && tc.Project.Plc != null && tc.Project.Plc.Project != null)
                {
                    string tsProjFolder = plcProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\", StringComparison.Ordinal));
                    string plcProjName = plcProjFilePath.Replace(tsProjFolder + "\\", "");

                    foreach (TcSmProjectProjectPlcProject plcProj in tc.Project.Plc.Project)
                    {
                        if (!String.IsNullOrEmpty(plcProj.PrjFilePath) && String.IsNullOrEmpty(plcProj.File))
                        {
                            if (plcProj.PrjFilePath.Equals(plcProjName))
                            {
                                ret = true;
                                break;
                            }
                        }
                        else if (String.IsNullOrEmpty(plcProj.PrjFilePath) && !String.IsNullOrEmpty(plcProj.File))
                        {
                            if (plcProject.Xti.CompletePathInFileSystem == null)
                            {
                                plcProject.IsIndependent = true;
                                plcProject.Xti.FileNameInFileSystem = plcProj.File;
                                plcProject.Xti.FolderPathInFileSystem = tsProjFolder + "\\_Config\\PLC";
                                plcProject.Xti.CompletePathInFileSystem = plcProject.Xti.FolderPathInFileSystem + "\\" + plcProject.Xti.FileNameInFileSystem;
                                plcProject.Xti.Name = plcProject.Xti.FileNameInFileSystem.Replace(".xti", "");
                                plcProject.Xti.Path = plcProject.Xti.FolderPathInFileSystem.Replace(tsProjFolder + "\\", "");
                                plcProject.Xti.CompleteName = plcProject.Xti.Path + "\\" + plcProject.Xti.FileNameInFileSystem;
                            }
                            if (plcProj.File.Equals(plcProject.Xti.FileNameInFileSystem))
                            {
                                ret = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            if (!ret)
            {
                EventLogger.Instance.Logger.Information(@"PLC project: ""{0}"" is not included in the XAE project:  ""{1}""!!!", plcProjFilePath, tsProjFilePath);
                Environment.Exit(0);
            }
            return ret;
        }
    }
}
