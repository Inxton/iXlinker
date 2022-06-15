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
        internal void SearchDevices(Solution vs)
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

            //Fill the list of the base empty structures that are later extended by the required structure
            FillStructureBaseResourseDictionary();
            //Fill the list of the PLC structures that need to replaced by the replicated structures
            FillSpecialPlcTypesReplacementDictionary();
            //Fill the list of the PDO structures that need to replaced as its content is identical
            FillPdoStructuresReplacementDictionary();

            EventLogger.Instance.Logger.Information(@"Reading IO devices in the XAE project: ""{0}""!!!", vs.TsProject.Name);
            for (int i = 0; i < Tc.Project.Plc.Project.Length; i++)
            {
                if (vs.PlcProject.IsIndependent && vs.PlcProject.Xti.FileNameInFileSystem.Equals(Tc.Project.Plc.Project[i].File))
                {
                    XmlSerializer xtiSerializer = new XmlSerializer(typeof(TcSmItem));
                    StreamReader xtiReader = new StreamReader(vs.PlcProject.Xti.CompletePathInFileSystem);

                    try
                    {
                        TcSmItem Xti = (TcSmItem)xtiSerializer.Deserialize(xtiReader);
                        TcSmItemTypeProject plcProj = (TcSmItemTypeProject)Xti.Items[0];
                        xtiReader.Close();
                        //OwnerAPlcName = "TIPC" + tmpLevelSeparator + plcProj.Name + tmpLevelSeparator + plcProj.Instance[0].Name;
                        OwnerAPlcName = plcProj.Instance[0].Name;
                        try
                        {
                            if (plcProj.Instance[0].Contexts != null) Context = plcProj.Instance[0].Contexts[0].Name;
                            else Context = "invalid_context";
                            if (Context == "Default")
                                Context = "PlcTask";
                        }
                        catch (Exception ex)
                        {
                            EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                            Context = "invalid_context";
                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        xtiReader.Close();
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                }
                else if (vs.PlcProject.Plcproj.Name.Equals(Tc.Project.Plc.Project[i].Name))
                {
                    TcSmProjectProjectPlcProject plcProj = new TcSmProjectProjectPlcProject();
                    plcProj = Tc.Project.Plc.Project[i];
                    OwnerAPlcName = "TIPC" + tmpLevelSeparator + plcProj.Name + tmpLevelSeparator + plcProj.Instance[0].Name;
                    try
                    {
                        if (plcProj.Instance[0].Contexts != null) Context = plcProj.Instance[0].Contexts[0].Name;
                        else Context = "invalid_context";
                        if (Context == "Default")
                            Context = "PlcTask";
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                        Context = "invalid_context";
                    }
                    break;
                }
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
