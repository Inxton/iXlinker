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
        private TcSmDevDef GetDeviceFromXtiFile(Solution vs, TcSmProjectProjectIODevice d)
        {
            TcSmDevDef device = new TcSmDevDef();

            string devName = d.Name != null ? d.Name : d.File != null ? d.File.ToString().Replace(".xti", "") : "";
            bool isIndependentProjectFile = d.Name == null && d.File != null;

            if (isIndependentProjectFile)
            {
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
                            device = (TcSmDevDef)Xti.Items[0];
                            device.Name = device.RemoteName ?? devName;
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
                device = d as TcSmDevDef;
            }
            return device;
        }
    }
}
