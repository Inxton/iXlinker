﻿using System;
using System.IO;
using System.Linq;
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
            if (File.Exists(vs.TsProject.CompletePathInFileSystem))
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
                    EventLogger.Instance.Logger.Error(@"Unable to deserialize XAE project file: " + vs.TsProject.CompletePathInFileSystem + " !!!"
                          + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    reader.Close();
                }
            }
            else
            {
                EventLogger.Instance.Logger.Error(@"Unable to find XAE project file!!!"
                     + Environment.NewLine + @"File: " + vs.TsProject.CompletePathInFileSystem + " not found!!!"
                     + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                //Get OwnerAPlcName  and Context in case of Independent Plc project file
                if (vs.PlcProject.IsIndependent && vs.PlcProject.Xti.FileNameInFileSystem.Equals(Tc.Project.Plc.Project[i].File))
                {
                    XmlSerializer xtiSerializer = new XmlSerializer(typeof(TcSmItem));
                    StreamReader xtiReader = new StreamReader(vs.PlcProject.Xti.CompletePathInFileSystem);

                    try
                    {
                        TcSmItem Xti = (TcSmItem)xtiSerializer.Deserialize(xtiReader);
                        TcSmItemTypeProject plcProj = (TcSmItemTypeProject)Xti.Items[0];
                        xtiReader.Close();

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
                //Get OwnerAPlcName  and Context in case of dependent Plc project file
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

            if (CheckDeviceAndBoxNamesUniqueness(vs, Tc.Project.Io))
            {
                if (Tc.Project.Io != null && Tc.Project.Io.Items != null)
                {
                    try
                    {
                        foreach (TcSmProjectProjectIODevice device in Tc.Project.Io.Items)
                        {
                            bool isIndependentProjectFile = device.Name == null && device.File != null;
                            TcSmDevDef _device = new TcSmDevDef();
                            if (isIndependentProjectFile)
                            {
                                string devName = isIndependentProjectFile ? device.File.ToString().Replace(".xti", "") : "";
                                string fileName = vs.IndependentIoDevices.Where(c => c.Name.Equals(devName)).Single().CompletePathInFileSystem;

                                if (fileName != null)
                                {
                                    if (File.Exists(fileName))
                                    {
                                        XmlSerializer serializer = new XmlSerializer(typeof(TcSmItem));
                                        StreamReader reader = new StreamReader(fileName);

                                        try
                                        {
                                            TcSmItem Xti = (TcSmItem)serializer.Deserialize(reader);
                                            _device = (TcSmDevDef)Xti.Items[0];

                                        }
                                        catch (Exception ex)
                                        {
                                            EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                                        }
                                        finally
                                        {
                                            reader.Close();
                                        }
                                    }
                                    else
                                    {
                                        EventLogger.Instance.Logger.Error(@"Unable to find xti file for the device: " + devName + "!!!"
                                             + Environment.NewLine + @"File: " + fileName + " not found!!!"
                                             + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                                    }
                                }
                                else
                                {
                                    EventLogger.Instance.Logger.Error(@"Unable to discover complete path to xti file of the device: " + devName
                                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                                }
                            }
                            else
                            {
                                _device = (TcSmDevDef)device;
                            }


                            if (!vs.DoNotGenerateDisabled || !_device.DisabledSpecified || !_device.Disabled)
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
