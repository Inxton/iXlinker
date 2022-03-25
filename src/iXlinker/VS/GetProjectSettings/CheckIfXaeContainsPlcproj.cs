using iXlinker.Utils;
using System;
using System.IO;
using System.Xml.Serialization;
using TwincatXmlSchemas.TcSmProject;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        private static bool CheckIfXaeContainsPlcproj(string tsProjFilePath, string plcProjFilePath)
        {
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
                        if (plcProj.PrjFilePath.Equals(plcProjName))
                        {
                            ret = true;
                            break;
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
