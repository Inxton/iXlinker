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
        private TcSmBoxDef GetBoxFromXtiFile(string path, TcSmDevDefBox _box)
        {
            TcSmBoxDef box = new TcSmDevDefBox();

            string boxName = _box.Name != null ? _box.Name : _box.File != null ? _box.File.ToString().Replace(".xti", "") : "";
            bool isIndependentProjectFile = _box.Name == null && _box.File != null;

            if (isIndependentProjectFile)
            {
                string fileName = Path.Combine(path, _box.File);

                if (fileName != null)
                {
                    if (File.Exists(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TcSmItem));
                        StreamReader reader = new StreamReader(fileName);

                        try
                        {
                            TcSmItem Xti = (TcSmItem)serializer.Deserialize(reader);
                            box = (TcSmBoxDef)Xti.Items[0];
                            box.Name = boxName;
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
                        EventLogger.Instance.Logger.Error(@"Unable to find xti file for the box: " + boxName + "!!!"
                             + Environment.NewLine + @"File: " + fileName + " not found!!!"
                             + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                }
                else
                {
                    EventLogger.Instance.Logger.Error(@"Unable to discover complete path to xti file of the box: " + boxName
                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            else
            {
                box = _box as TcSmBoxDef;
            }
            return box;
        }
        private TcSmBoxDef GetBoxFromXtiFile(string path, TcSmBoxDefBox _box)
        {
            TcSmBoxDef box = new TcSmDevDefBox();

            string boxName = _box.Name != null ? _box.Name : _box.File != null ? _box.File.ToString().Replace(".xti", "") : "";
            bool isIndependentProjectFile = _box.Name == null && _box.File != null;

            if (isIndependentProjectFile)
            {
                string fileName = Path.Combine(path, _box.File);
                if (fileName != null)
                {
                    if (File.Exists(fileName))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TcSmItem));
                        StreamReader reader = new StreamReader(fileName);

                        try
                        {
                            TcSmItem Xti = (TcSmItem)serializer.Deserialize(reader);
                            box = (TcSmBoxDef)Xti.Items[0];
                            box.Name = boxName;
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
                        EventLogger.Instance.Logger.Error(@"Unable to find xti file for the box: " + boxName + "!!!"
                             + Environment.NewLine + @"File: " + fileName + " not found!!!"
                             + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                }
                else
                {
                    EventLogger.Instance.Logger.Error(@"Unable to discover complete path to xti file of the box: " + boxName
                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            else
            {
                box = _box as TcSmBoxDef;
            }
            return box;
        }
    }
}
