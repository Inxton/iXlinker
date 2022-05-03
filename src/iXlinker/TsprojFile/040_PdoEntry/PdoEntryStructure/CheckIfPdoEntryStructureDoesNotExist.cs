using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfPdoEntryStructureDoesNotExist(PdoEntryStructViewModel actPdoEntryStruct)
        {
            bool ret = true;
            actPdoEntryStruct.NumberOfUses = 1;
            foreach (PdoEntryStructViewModel structVM in PdoEntryStructures)
            {
                if (actPdoEntryStruct.Name.Equals(structVM.Name) && actPdoEntryStruct.Crc32.Equals(structVM.Crc32) && actPdoEntryStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    structVM.NumberOfUses++;
                    break;
                }

            }
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (actPdoEntryStruct.Name.Equals(plcStruct.Name))
                {
                    actPdoEntryStruct.TypeNamespace = plcStruct.Namespace;
                    ret = false;
                    plcStruct.NumberOfUses++;
                    break;
                }
            }
            return ret;
        }
    }
}
