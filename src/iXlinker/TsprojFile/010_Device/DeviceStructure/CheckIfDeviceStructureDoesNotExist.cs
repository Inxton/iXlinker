using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfDeviceStructureDoesNotExist(DeviceStructViewModel actDevStruct)
        {
            bool ret = true;
            actDevStruct.NumberOfUses = 1;
            foreach (DeviceStructViewModel structVM in DeviceStructures)
            {
                if (actDevStruct.Name.Equals(structVM.Name) && actDevStruct.Crc32.Equals(structVM.Crc32) && actDevStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    structVM.NumberOfUses++;
                    break;
                }

            }
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (actDevStruct.Name.Equals(plcStruct.Name))
                {
                    actDevStruct.TypeNamespace = plcStruct.Namespace;
                    ret = false;
                    plcStruct.NumberOfUses++;
                    break;
                }
            }
            return ret;
        }
    }
}
