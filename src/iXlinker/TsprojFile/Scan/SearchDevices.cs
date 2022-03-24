using System;
using System.IO;
using System.Xml.Serialization;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void SearchDevices(Solution vs)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
            StreamReader reader = new StreamReader(vs.TsProject.CompletePathInFileSystem);

            try
            {
                ClearLists();

                Tc = (TcSmProject)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                Console.ReadLine();
            }

            GetPlcLibraries(vs);

            GetPlcStructuresInPlcLibraries(vs);

            Console.WriteLine(@"Reading IO devices in the XAE project: ""{0}""!!!", vs.TsProject.Name);
            TcSmProjectProjectPlcProject plcProj = new TcSmProjectProjectPlcProject();
            for (int i = 0; i < Tc.Project.Plc.Project.Length; i++)
            {
                string dtePlcProjName = vs.PlcProject.Name;
                string tcPlcProjName = Tc.Project.Plc.Project[i].Name;
                if (dtePlcProjName.Equals(tcPlcProjName))
                {
                    plcProj = Tc.Project.Plc.Project[i];
                    break;
                }
            }

            OwnerAPlcName = "TIPC" + tmpLevelSeparator + plcProj.Name + tmpLevelSeparator + plcProj.Instance[0].Name;
            try
            {
                if(plcProj.Instance[0].Contexts != null) Context = plcProj.Instance[0].Contexts[0].Name;
                else Context = "invalid_context";
                if (Context == "Default")
                    Context = "PlcTask";
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                Context = "invalid_context";
            }

            GetAllTasks();

            if (CheckDeviceAndBoxNamesUniqueness(Tc.Project.Io))
            {
                if (Tc.Project.Io != null && Tc.Project.Io.Items != null)
                {
                    try
                    {
                        foreach (TcSmProjectProjectIODevice device in Tc.Project.Io.Items)
                        {
                            if (!vs.DoNotGenerateDisabled || !device.DisabledSpecified || !device.Disabled)
                            {
                                AddDevice(vs, device);
                            }
                        }

                        if (TotalNumberOfDevices == 0 && TotalNumberOfBoxes == 0)
                        {
                            Console.WriteLine("There are no unmapped devices");
                            Console.ReadLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("No devices found.");
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("No IO devices found! No outputs generated!!!");
                    Console.WriteLine("Press any key to close the application!!!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Tsproj file contains not unique device name or box name! No outputs generated!!!");
                Console.WriteLine("Press any key to close the application!!!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            Console.WriteLine(@"PdoEntryStructures :{0}", PdoEntryStructures.Count);
            Console.WriteLine(@"PdoStructures :{0}", PdoStructures.Count);
            Console.WriteLine(@"BoxStructures :{0}", BoxStructures.Count);
            Console.WriteLine(@"DeviceStructures :{0}", DeviceStructures.Count);
            Console.WriteLine(@"TopologyStructures :{0}", TopologyStructures.Count);
        }
    }
}
