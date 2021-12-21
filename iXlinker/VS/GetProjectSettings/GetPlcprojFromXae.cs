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
                    Console.WriteLine(@"No PLC project found in the XAE project:  ""{0}""!!!", tsProjFilePath);
                    Console.WriteLine("Press any key to close the application!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                if (tc != null && tc.Project != null && tc.Project.Plc != null && tc.Project.Plc.Project != null)
                {
                    if (tc.Project.Plc.Project.Length > 1)
                    {
                        Console.WriteLine(@"Multiple PLC projects found in the XAE project:  ""{0}""!!!", tsProjFilePath);
                        Console.WriteLine(@"Restart the application with the exact PLC project path and file name specified!!!");
                        Console.WriteLine("Press any key to close the application!!!");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }

                    if (tc.Project.Plc.Project.Length == 1 && tc.Project.Plc.Project[0] != null && !string.IsNullOrEmpty(tc.Project.Plc.Project[0].PrjFilePath))
                    {
                        string tsProjFolder = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\"));
                        plcProjFilePath = tsProjFolder + "\\" + tc.Project.Plc.Project[0].PrjFilePath;
                        Console.WriteLine(@"PLC project: ""{0}"" found in the XAE project: ""{1}"" will be used!!!", tsProjFilePath, plcProjFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                Console.ReadLine();
            }

            return plcProjFilePath;
        }
    }
}
