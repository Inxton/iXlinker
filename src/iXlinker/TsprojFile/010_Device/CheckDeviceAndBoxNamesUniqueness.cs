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
                    TcSmDevDef dev = GetDeviceFromXtiFile(vs, d);
                    List<string> TsProjBoxNames = new List<string>();
                    if (!dev.DisabledSpecified || !dev.DisabledSpecified)
                    {
                        if (devNames.Contains(dev.Name))
                        {
                            EventLogger.Instance.Logger.Error("Not unique device name: {0} found in the XAE project file: {1}!!!", dev.Name, vs.TsProject.CompletePathInFileSystem);
                            ret = false;
                        }
                        else
                        {
                            devNames.Add(dev.Name);
                        }
                        if ( dev.Box != null)
                        {
                            foreach (TcSmDevDefBox box in dev.Box)
                            {
                                if (!CheckDevDefNameUniqueness(vs.TsProject.CompletePathInFileSystem, dev.Name , box, ref TsProjBoxNames))
                                {
                                    ret = false;
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    bool isIndependent = d.Name == null && d.File != null;
                    //    string fileName = isIndependent ? Path.Combine(Directory.GetParent(vs.TsProject.CompletePathInFileSystem).FullName.ToString() , @"_Config\IO",d.File) : vs.TsProject.CompletePathInFileSystem;
                    //    EventLogger.Instance.Logger.Information("Disabled device: {0} found in the XAE project file: {1}!!!", dev.Name, fileName);
                    //}
                }
            }
            return ret;
        }
        private bool CheckDevDefNameUniqueness(string fileName, string path, TcSmDevDefBox _box, ref List<string> boxNames)
        {
            bool ret = true;
            TcSmBoxDef box = _box as TcSmBoxDef;
            bool isIndependentProjectFile = _box.Name == null && _box.File != null;
            string folderName = Directory.GetParent(fileName).FullName.ToString();
            string _fileName = fileName;
            if (isIndependentProjectFile)
            {
                if (path == null)
                {
                    EventLogger.Instance.Logger.Error(@"Unable to find xti file: " + _box.File
                         + Environment.NewLine + @"Input variable 'path' in method: " + System.Reflection.MethodBase.GetCurrentMethod().Name + " is null!!!"
                         + Environment.NewLine + @"Device will be ignored!!!");
                    
                }
                else
                {
                    box = GetBoxFromXtiFile(Path.Combine(folderName, @"_Config\IO", path), _box);
                    _fileName = Path.Combine(folderName, @"_Config\IO", path, _box.File);
                }
            }

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
                        if (!CheckBoxDefNameUniqueness(_fileName, box.Name, path + "." + box.Name, subbox, ref boxNames))
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }
        private bool CheckBoxDefNameUniqueness(string fileName, string parentBoxName, string path, TcSmBoxDefBox _box, ref List<string> boxNames)
        {
            bool ret = true;

            TcSmBoxDef box = _box as TcSmBoxDef;
            bool isIndependentProjectFile = _box.Name == null && _box.File != null;
            string folderName = Directory.GetParent(fileName).FullName.ToString();
            string _fileName = fileName;
            if (isIndependentProjectFile)
            {
                box = GetBoxFromXtiFile(Path.Combine(folderName, parentBoxName), _box);
                _fileName = Path.Combine(folderName, parentBoxName, _box.File);
            }




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
                        if (!CheckBoxDefNameUniqueness(_fileName, box.Name, path + "." + box.Name, subbox, ref boxNames))
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
