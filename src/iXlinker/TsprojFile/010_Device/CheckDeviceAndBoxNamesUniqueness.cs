using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using iXlinker.Utils;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckDeviceAndBoxNamesUniqueness(Solution vs, TcSmProjectProjectIO Io)
        {
            List<string> devNames = new List<string>();

            bool ret = true;
            if (Io!=null && Io.Items != null)
            {
                foreach (TcSmProjectProjectIODevice d in Io.Items)
                {
                    List<string> TsProjBoxNames = new List<string>();
                    if (!d.DisabledSpecified || !d.DisabledSpecified)
                    {
                        string devName = d.Name != null ? d.Name : d.File != null ? d.File.ToString().Replace(".xti",""): "";
                        bool isIndependentProjectFile = d.Name == null && d.File != null;
                        if (devNames.Contains(devName))
                        {
                            EventLogger.Instance.Logger.Error("Not unique device name: {0} found in the XAE project file: {1}!!!", devName , vs.TsProject.CompletePathInFileSystem);
                            ret = false;
                        }
                        else
                        {
                            devNames.Add(devName);
                        }
                        if (!isIndependentProjectFile && d.Box != null)
                        {
                            foreach (TcSmDevDefBox box in d.Box)
                            {
                                if (!CheckDevDefNameUniqueness(vs.TsProject.CompletePathInFileSystem, devName + "." + box.Name, box, ref TsProjBoxNames))
                                {
                                    ret = false;
                                }
                            }
                        }
                        if (isIndependentProjectFile)
                        {
                            string fileName = vs.IndependentIoDevices.Where(c => c.Name.Equals(devName)).Single().CompletePathInFileSystem;

                            if (fileName != null)
                            {
                                if (File.Exists(fileName))
                                {
                                    XmlSerializer serializer = new XmlSerializer(typeof(TcSmItem));
                                    StreamReader reader = new StreamReader(fileName);
                                    List<string> XtiProjBoxNames = new List<string>();

                                    try
                                    {
                                        TcSmItem Xti = (TcSmItem)serializer.Deserialize(reader);
                                        TcSmDevDef device = (TcSmDevDef)Xti.Items[0];
                                        if (device.Box != null)
                                        {
                                            foreach (TcSmDevDefBox box in device.Box)
                                            {
                                                if (!CheckDevDefNameUniqueness(fileName, devName + "." + box.Name, box, ref XtiProjBoxNames))
                                                {
                                                    ret = false;
                                                }
                                            }
                                        }

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
                                         + Environment.NewLine + @"File: "+ fileName + " not found!!!"
                                         + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                                }
                            }
                            else
                            {
                                EventLogger.Instance.Logger.Error(@"Unable to discover complete path to xti file of the device: " + devName
                                     + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                            }
                        }
                    }
                }
            }
            return ret;
        }
        private bool CheckDevDefNameUniqueness(string fileName, string path, TcSmDevDefBox box, ref List<string> boxNames)
        {
            bool ret = true;
            if (!box.DisabledSpecified || !box.Disabled)
            {

                if (boxNames.Contains(box.Name))
                {
                    EventLogger.Instance.Logger.Error("Not unique box name: {0} found in: {1} in the XAE project file: {2}!!!", box.Name, path, fileName);
                    ret = false;
                }
                else
                {
                    boxNames.Add(box.Name);
                }
                if (box.Box != null)
                {
                    foreach (TcSmBoxDefBox subbox in box.Box)
                    {
                        if (!CheckBoxDefNameUniqueness(fileName, path + "." + subbox.Name, subbox, ref boxNames))
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }

        private bool CheckBoxDefNameUniqueness(string fileName, string path, TcSmBoxDefBox box, ref List<string> boxNames)
        {
            bool ret = true;
            if (!box.DisabledSpecified || !box.Disabled)
            {

                if (boxNames.Contains(box.Name))
                {
                    EventLogger.Instance.Logger.Error("Not unique box name: {0} found in: {1} in the XAE project file: {2}!!!", box.Name, path, fileName);
                    ret = false;
                }
                else
                {
                    boxNames.Add(box.Name);
                }
                if (box.Box != null)
                {
                    foreach (TcSmBoxDefBox subbox in box.Box)
                    {
                        if (!CheckBoxDefNameUniqueness(fileName, path + "." + subbox.Name, subbox, ref boxNames))
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
