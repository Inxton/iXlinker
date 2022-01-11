﻿using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddEthercatDevice(Solution vs,TcSmProjectProjectIODevice device)
        {
            DeviceViewModel deviceViewModel = FillEthercatDeviceData(device);

            if (device.Box != null)
            {
                try
                {
                    BoxViewModel boxViewModel = new BoxViewModel();
                    foreach (TcSmDevDefBox devDefBox in device.Box)
                    {
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
                                boxViewModel = CreateBox(vs, device, ref deviceViewModel, devDefBox, "TIID" + tmpLevelSeparator + device.Name);
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
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
            }
            PdoViewModel InfoData = GetDeviceInfoDataAsOneStructure(device, deviceViewModel);
            PdoViewModel SyncUnits = GetDeviceSyncUnitsAsOneStructure(device, deviceViewModel);
            PdoViewModel Inputs = GetDeviceInputsAsOneStructure(device, deviceViewModel, DeviceInputFrames);
            PdoViewModel Outputs = GetDeviceOutputsAsOneStructure(device, deviceViewModel, DeviceOutputFrames);

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
