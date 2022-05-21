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
        private static List<PlcProjectViewModel> GetPlcprojsFromXae(string tsProjFilePath)
        {
            List<PlcProjectViewModel> plcProjs = new List<PlcProjectViewModel>();

            XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
            StreamReader reader = new StreamReader(tsProjFilePath);

            try
            {
                TcSmProject tc = (TcSmProject)serializer.Deserialize(reader);
                reader.Close();

                if (tc != null && tc.Project != null && tc.Project.Plc != null && tc.Project.Plc.Project != null)
                {
                    foreach (TcSmProjectProjectPlcProject plcProject in tc.Project.Plc.Project)
                    {

                        if (!string.IsNullOrEmpty(plcProject.PrjFilePath))
                        {
                            string tsProjFolder = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\"));

                            PlcProjectViewModel plcProj = new PlcProjectViewModel();

                            plcProj.CompletePathInFileSystem = tsProjFolder + "\\" + plcProject.PrjFilePath;
                            plcProj.FileNameInFileSystem = plcProj.CompletePathInFileSystem.Substring(plcProj.CompletePathInFileSystem.LastIndexOf("\\") + 1);
                            plcProj.FolderPathInFileSystem = plcProj.CompletePathInFileSystem.Substring(0, plcProj.CompletePathInFileSystem.LastIndexOf("\\"));

                            plcProj.Name = plcProject.Name;
                            plcProj.UniqueName = plcProject.PrjFilePath;

                            plcProj.GUID = Guid.Parse(plcProject.GUID);

                            plcProjs.Add(plcProj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                Console.ReadLine();
            }

            return plcProjs;
        }
    }
}
