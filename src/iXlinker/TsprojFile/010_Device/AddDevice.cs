using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddDevice(Solution vs, TcSmDevDef device)
        {
            DeviceTypes device_type = (DeviceTypes)device.DevType;

            switch (device_type)
            {
                case DeviceTypes.IODEVICETYPE_ETHERCATPROT:
                    AddEthercatDevice(vs,device);
                    break;
                default:
                    break;
            }
        }
    }
}
