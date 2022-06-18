using System;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private DeviceViewModel FillEthercatDeviceData(TcSmDevDef device)
        {
            DeviceViewModel deviceViewModel = new DeviceViewModel();

            deviceViewModel.SyncUnits = GetAllSyncUnits(device);

            DeviceTypes device_type = (DeviceTypes)device.DevType;

            string device_name = "???";
            try
            {
                if (device.RemoteName != null)
                {
                    device_name = device.RemoteName;
                }
                else if (device.Name != null)
                {
                    device_name = device.Name;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                return deviceViewModel;
            }

            int device_id = 0;
            try
            {
                if (device.Id != null)
                {
                    device_id = int.Parse(device.Id);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                return deviceViewModel;
            }

            string device_amsnetid = "";
            try
            {
                if (device.AmsNetId != null)
                {
                    device_amsnetid = device.AmsNetId;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                return deviceViewModel;
            }

            int device_amsport = 0;
            try
            {
                if (device.AmsPort != null)
                {
                    device_amsport = (int)device.AmsPort;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                return deviceViewModel;
            }

            deviceViewModel.Type = device_type;
            deviceViewModel.Name = device_name;
            deviceViewModel.Id = device_id;
            deviceViewModel.AmsNetId = device_amsnetid;
            deviceViewModel.AmsPort = device_amsport;
            deviceViewModel.TotalNumberOfBoxes = 0;
            deviceViewModel.NumberOfSubBoxes = 0;

            return deviceViewModel;
        }
    }
}
