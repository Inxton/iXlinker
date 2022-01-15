using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfDeviceStructureDoesNotExist(DeviceStructViewModel actDevStruct)
        {
            bool ret = true;
            foreach(DeviceStructViewModel structVM in DeviceStructures)
            {
                if (actDevStruct.Name.Equals(structVM.Name) && actDevStruct.Crc32.Equals(structVM.Crc32) && actDevStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    break;
                }

            }
            return ret;
        }
    }
}
