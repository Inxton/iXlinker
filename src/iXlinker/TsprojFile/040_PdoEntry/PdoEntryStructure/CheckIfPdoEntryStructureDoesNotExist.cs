using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfPdoEntryStructureDoesNotExist(PdoEntryStructViewModel actPdoEntryStruct)
        {
            bool ret = true;
            foreach(PdoEntryStructViewModel structVM in PdoEntryStructures)
            {
                if (actPdoEntryStruct.Name.Equals(structVM.Name) && actPdoEntryStruct.Crc32.Equals(structVM.Crc32) && actPdoEntryStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    break;
                }

            }
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (actPdoEntryStruct.Name.Equals(plcStruct.Name))
                {
                    actPdoEntryStruct.TypeNamespace = plcStruct.Namespace;
                    ret = false;
                    break;
                }
            }
            return ret;
        }
    }
}
