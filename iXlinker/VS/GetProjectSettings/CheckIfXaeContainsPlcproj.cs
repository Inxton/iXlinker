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
                    string tsProjFolder = plcProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\"));
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
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                Console.ReadLine();
            }

            if (!ret)
            {
                Console.WriteLine(@"PLC project: ""{0}"" is not included in the XAE project:  ""{1}""!!!", plcProjFilePath, tsProjFilePath);
                Console.WriteLine("Press any key to close the application!!!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return ret;
        }
    }
}
