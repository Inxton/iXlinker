using System;
using System.IO;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddEthercatDevice(Solution vs, TcSmDevDef device)
        {
            DeviceViewModel deviceViewModel = FillEthercatDeviceData(device);

            if (device.Box != null)
            {
                try
                {
                    BoxViewModel boxViewModel = new BoxViewModel();
                    foreach (TcSmDevDefBox _devDefBox in device.Box)
                    {
                        TcSmBoxDef devDefBox = _devDefBox as TcSmBoxDef;
                        bool isIndependentProjectFile = _devDefBox.Name == null && _devDefBox.File != null;
                        string path = Path.Combine(vs.TsProject.FolderPathInFileSystem, @"_Config\IO", device.Name);
                        if (isIndependentProjectFile)
                        {
                            devDefBox = GetBoxFromXtiFile(path, _devDefBox);
                            try
                            {
                                ((EtherCATSlave)devDefBox.Item).PortABoxInfo = ((EtherCATSlave)_devDefBox.Item).PortABoxInfo;
                            }
                            catch (Exception ex)
                            {
                                EventLogger.Instance.Logger.Error(@"Unable to discover PortABoxInfo for box: " + devDefBox.Name
                                     + Environment.NewLine + @", in: " + path + "!!!"
                                     + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                            }
                        }
                        if (devDefBox != null)
                        {
                            if (vs.DoNotGenerateDisabled && devDefBox.DisabledSpecified && devDefBox.Disabled && devDefBox.Box != null)
                            {
                                foreach (TcSmBoxDefBox boxDefBox in devDefBox.Box)
                                {
                                    boxDefBox.DisabledSpecified = devDefBox.DisabledSpecified;
                                    boxDefBox.Disabled = devDefBox.Disabled;
                                }
                            }
                            if (!vs.DoNotGenerateDisabled || !devDefBox.DisabledSpecified || !devDefBox.Disabled)
                            {
                                boxViewModel = CreateBox(vs, device, ref deviceViewModel, devDefBox, "TIID" + tmpLevelSeparator + deviceViewModel.Name);
                                deviceViewModel.Boxes.Add(boxViewModel);

                                deviceViewModel.MapableObjects.Add(boxViewModel.MapableObjectGrouped);

                                deviceViewModel.NumberOfSubBoxes++;
                                deviceViewModel.TotalNumberOfBoxes = deviceViewModel.TotalNumberOfBoxes + boxViewModel.TotalNumberOfBoxes + 1;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
            }
            PdoViewModel InfoData = GetDeviceInfoDataAsOneStructure(device, deviceViewModel);
            PdoViewModel SyncUnits = GetDeviceSyncUnitsAsOneStructure(device, deviceViewModel);
            PdoViewModel Inputs = GetDeviceInputsAsOneStructure(device, deviceViewModel, vs.MaxEthercatFrameIndex);
            PdoViewModel Outputs = GetDeviceOutputsAsOneStructure(device, deviceViewModel, vs.MaxEthercatFrameIndex);

            deviceViewModel.MapableObjects.Add(SyncUnits.MapableObject);
            deviceViewModel.MapableObjects.Add(Inputs.MapableObject);
            deviceViewModel.MapableObjects.Add(Outputs.MapableObject);
            deviceViewModel.MapableObjects.Add(InfoData.MapableObject);

            deviceViewModel.MapableObjectGrouped = GetAllMapableObjectsAsOneStructure(deviceViewModel, deviceViewModel.MapableObjects);

            Devices.Add(deviceViewModel);

            TotalNumberOfDevices++;
        }
    }
}
