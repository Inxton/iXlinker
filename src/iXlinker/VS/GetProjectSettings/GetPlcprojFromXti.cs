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
        private static void GetPlcprojFromXti(PlcProject plcProject)
        {
            XmlSerializer xtiSerializer = new XmlSerializer(typeof(TcSmItem));
            StreamReader xtiReader = new StreamReader(plcProject.Xti.CompletePathInFileSystem);
             
            try
            {
                TcSmItem Xti = (TcSmItem)xtiSerializer.Deserialize(xtiReader);
                TcSmItemTypeProject project = (TcSmItemTypeProject)Xti.Items[0];
                plcProject.Plcproj.Name = !string.IsNullOrEmpty(project.Name) ? project.Name.ToString() : "";
                string plcProjFilePath = !string.IsNullOrEmpty(project.PrjFilePath) ? project.PrjFilePath.ToString() : "";
                plcProjFilePath = !string.IsNullOrEmpty(plcProjFilePath) ? plcProjFilePath.Replace("..\\", "") : "";

                if(!string.IsNullOrEmpty(plcProject.Xti.FolderPathInFileSystem) && !string.IsNullOrEmpty(plcProjFilePath))
                {
                    string tsProjFolder = Directory.GetParent(Directory.GetParent(plcProject.Xti.FolderPathInFileSystem).FullName).FullName;
                    plcProject.Plcproj.CompletePathInFileSystem = Path.Combine(tsProjFolder , plcProjFilePath);
                    plcProject.Plcproj.FolderPathInFileSystem = new DirectoryInfo(plcProject.Plcproj.CompletePathInFileSystem).Parent.ToString();
                    plcProject.Plcproj.FileNameInFileSystem = new DirectoryInfo(plcProject.Plcproj.CompletePathInFileSystem).Name.ToString();
                    plcProject.Plcproj.Name = plcProject.Plcproj.FileNameInFileSystem.Replace(".plcproj", "");
                    plcProject.Plcproj.Path = plcProject.Plcproj.FolderPathInFileSystem.Replace(tsProjFolder + "\\", "");
                    plcProject.Plcproj.CompleteName = plcProject.Plcproj.Path + "\\" + plcProject.Plcproj.FileNameInFileSystem;
                }

                xtiReader.Close();
            }
            catch (Exception ex)
            {
                xtiReader.Close();
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
        }
    }
}
