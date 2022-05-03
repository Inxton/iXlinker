using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfBoxStructureDoesNotExist(BoxStructViewModel actBoxStruct)
        {
            bool ret = true;
            actBoxStruct.NumberOfUses = 1;
            foreach (BoxStructViewModel structVM in BoxStructures)
            {
                if (actBoxStruct.Name.Equals(structVM.Name) && actBoxStruct.Crc32.Equals(structVM.Crc32) && actBoxStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    structVM.NumberOfUses++;
                    break;
                }
            }
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (actBoxStruct.Name.Equals(plcStruct.Name))
                {
                    actBoxStruct.TypeNamespace = plcStruct.Namespace;
                    ret = false;
                    plcStruct.NumberOfUses++;
                    break;
                }
            }
            return ret;
        }
    }
}
