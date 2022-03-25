using iXlinker.Utils;
using System;
using System.IO;
using System.Xml.Serialization;
using TwincatXmlSchemas.TcSmProject;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        private static string GetPlcprojFromXae(string tsProjFilePath)
        {
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
                        EventLogger.Instance.Logger.Information(@"PLC project: ""{0}"" found in the XAE project: ""{1}"" will be used!!!", tsProjFilePath, plcProjFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            return plcProjFilePath;
        }
    }
}
