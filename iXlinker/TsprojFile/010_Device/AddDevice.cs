using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddDevice(TcSmProjectProjectIODevice device)
        {
            DeviceTypes device_type = (DeviceTypes)device.DevType;

            switch (device_type)
            {
                case DeviceTypes.IODEVICETYPE_ETHERCATPROT:
                    AddEthercatDevice(device);
                    break;
                default:
                    break;
            }
        }
    }
}
