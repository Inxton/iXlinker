using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using iXlinker.Utils;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckDeviceAndBoxNamesUniqueness(Solution vs, TcSmProjectProjectIO Io)
        {
            string deviceListFolder = AppDomain.CurrentDomain.BaseDirectory + "DeviceLists";
            string deviceList = AppDomain.CurrentDomain.BaseDirectory + "Devices.txt";
            if (Directory.Exists(deviceListFolder))
            {
                string[] files = Directory.GetFiles(deviceListFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                string[] dirs = Directory.GetDirectories(deviceListFolder);
                foreach (string dir in dirs)
                {
                    Directory.Delete(dir, true);
                }
            }
            else
            {
                Directory.CreateDirectory(deviceListFolder);
            }
            if (File.Exists(deviceList))
            {
                File.Delete(deviceList);
            }

            List<string> devNames = new List<string>();
            ObservableCollection<BoxDetails> deviceDetailsList = new ObservableCollection<BoxDetails>();

            bool ret = true;
            if (Io!=null && Io.Items != null)
            {
                foreach (TcSmProjectProjectIODevice d in Io.Items)
                {
                    ObservableCollection<BoxDetails> boxDetailsList =  new ObservableCollection<BoxDetails>();
                    List<string> BoxNames = new List<string>();
                    TcSmDevDef dev = d as TcSmDevDef;
                    bool isIndependentProjectFile = d.Name == null && d.File != null;
                    string folderName = vs.TsProject.FolderPathInFileSystem;
                    string _fileName = vs.TsProject.FileNameInFileSystem;
                    if (isIndependentProjectFile)
                    {
                        dev = GetDeviceFromXtiFile(vs, d);
                        _fileName = Path.Combine(folderName, @"_Config\IO", d.File);

                    }
                    BoxDetails deviceDetails = new BoxDetails() { Name = dev.Name, IoTreePath = dev.Name, IsIndependentProjectFile = isIndependentProjectFile, FileName = _fileName, FileExists = File.Exists(_fileName), IsDisabled = dev.DisabledSpecified && dev.DisabledSpecified };
                    deviceDetailsList.Add(deviceDetails);

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
                                if (!CheckDevDefNameUniqueness(vs.TsProject.CompletePathInFileSystem, dev.Name , box, ref BoxNames, ref boxDetailsList))
                                {
                                    ret = false;
                                }
                            }
                        }
                    }

                    using StreamWriter _file = new(Path.Combine(deviceListFolder,dev.Name + ".txt"));
                    _file.WriteLine("Name;IoTreePath;IsIndependentProjectFile;FileName;FileExists;IsDisabled");
                    foreach (BoxDetails boxDetails in boxDetailsList)
                    {
                        _file.WriteLine(boxDetails.Name + ";" + boxDetails.IoTreePath + ";" + boxDetails.IsIndependentProjectFile + ";" + boxDetails.FileName + ";" + boxDetails.FileExists + ";" + boxDetails.IsDisabled);
                    }
                }

                using StreamWriter file = new(deviceList);
                file.WriteLine("Name;IoTreePath;IsIndependentProjectFile;FileName;FileExists;IsDisabled");
                foreach (BoxDetails deviceDetails in deviceDetailsList)
                {
                    file.WriteLine(deviceDetails.Name + ";" + deviceDetails.IoTreePath +";" + deviceDetails.IsIndependentProjectFile + ";" + deviceDetails.FileName + ";" + deviceDetails.FileExists + ";" + deviceDetails.IsDisabled);
                }
            }
            return ret;
        }
        private bool CheckDevDefNameUniqueness(string fileName, string path, TcSmDevDefBox _box, ref List<string> boxNames, ref ObservableCollection<BoxDetails> boxDetailsList)
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

            BoxDetails boxDetails = new BoxDetails() { Name = box.Name, IoTreePath = path + "." + box.Name, IsIndependentProjectFile = isIndependentProjectFile, FileName = _fileName, FileExists = File.Exists(_fileName), IsDisabled = box.DisabledSpecified && box.DisabledSpecified };
            boxDetailsList.Add(boxDetails);

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
                        if (!CheckBoxDefNameUniqueness(_fileName, box.Name, path + "." + box.Name, subbox, ref boxNames, ref boxDetailsList))
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }
        private bool CheckBoxDefNameUniqueness(string fileName, string parentBoxName, string path, TcSmBoxDefBox _box, ref List<string> boxNames, ref ObservableCollection<BoxDetails> boxDetailsList)
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

            BoxDetails boxDetails = new BoxDetails() { Name = box.Name, IoTreePath = path + "." + box.Name, IsIndependentProjectFile = isIndependentProjectFile, FileName = _fileName, FileExists = File.Exists(_fileName), IsDisabled = box.DisabledSpecified && box.DisabledSpecified };
            boxDetailsList.Add(boxDetails);

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
                        if (!CheckBoxDefNameUniqueness(_fileName, box.Name, path + "." + box.Name, subbox, ref boxNames, ref boxDetailsList))
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
