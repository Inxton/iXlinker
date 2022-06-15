using iXlinkerExt.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TwincatXmlSchemas.TcSmProject;

namespace iXlinkerExt
{
    partial class SolutionDetails
    {
        private static PlcProjectViewModel GetPlcprojFromXti(string tsProjFilePath, string xtiFilePath)
        {
            PlcProjectViewModel plcProj = new PlcProjectViewModel();
            string tsProjFolderPath = new DirectoryInfo(tsProjFilePath).Parent.FullName.ToString();
            XmlSerializer xtiSerializer = new XmlSerializer(typeof(TcSmItem));
            StreamReader xtiReader = new StreamReader(xtiFilePath);

            try
            {
                TcSmItem Xti = (TcSmItem)xtiSerializer.Deserialize(xtiReader);
                TcSmItemTypeProject plcProject = (TcSmItemTypeProject)Xti.Items[0];
                plcProj.Name = !string.IsNullOrEmpty(plcProject.Name) ? plcProject.Name.ToString() : "";
                string plcProjFilePath = !string.IsNullOrEmpty(plcProject.PrjFilePath) ? plcProject.PrjFilePath.ToString() : "";
                plcProjFilePath = !string.IsNullOrEmpty(plcProjFilePath) ? plcProjFilePath.Replace("..\\", "") : "";

                if (!string.IsNullOrEmpty(plcProjFilePath))
                {
                    plcProj.CompletePathInFileSystem = Path.Combine(tsProjFolderPath, plcProjFilePath);
                    plcProj.FolderPathInFileSystem = new DirectoryInfo(plcProj.CompletePathInFileSystem).Parent.ToString();
                    plcProj.FileNameInFileSystem = new DirectoryInfo(plcProj.CompletePathInFileSystem).Name.ToString();
                    plcProj.Name = plcProj.FileNameInFileSystem.Replace(".plcproj", "");
                    plcProj.UniqueName = plcProj.CompletePathInFileSystem.Replace(tsProjFolderPath + "\\","");
                    plcProj.IsIndependent = true;
                    plcProj.GUID = Guid.Parse(plcProject.GUID);
                }

                xtiReader.Close();   
            }
            catch (Exception ex)
            {
                xtiReader.Close();
            }
            return plcProj;
        }
    }
}
