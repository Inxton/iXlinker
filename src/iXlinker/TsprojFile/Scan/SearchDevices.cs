using System;
using System.IO;
using System.Xml.Serialization;
using iXlinker.Utils;
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
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            GetPlcLibraries(vs);

            GetPlcStructuresInPlcLibraries(vs);

            EventLogger.Instance.Logger.Information(@"Reading IO devices in the XAE project: ""{0}""!!!", vs.TsProject.Name);
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
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
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
                            EventLogger.Instance.Logger.Information("There are no unmapped devices");
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error("No devices found.");
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                }
                else
                {
                    EventLogger.Instance.Logger.Information("No IO devices found! No outputs generated!!!");
                    Environment.Exit(0);
                }
            }
            else
            {
                EventLogger.Instance.Logger.Information("Tsproj file contains not unique device name or box name! No outputs generated!!!");
                Environment.Exit(0);
            }
            EventLogger.Instance.Logger.Information(@"PdoEntryStructures :{0}", PdoEntryStructures.Count);
            EventLogger.Instance.Logger.Information(@"PdoStructures :{0}", PdoStructures.Count);
            EventLogger.Instance.Logger.Information(@"BoxStructures :{0}", BoxStructures.Count);
            EventLogger.Instance.Logger.Information(@"DeviceStructures :{0}", DeviceStructures.Count);
            EventLogger.Instance.Logger.Information(@"TopologyStructures :{0}", TopologyStructures.Count);
        }
    }
}
