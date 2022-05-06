using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfPdoStructureDoesNotExist(PdoStructViewModel actPdoStruct)
        {
            bool ret = true;
            actPdoStruct.NumberOfUses = 1;
            foreach (PdoStructViewModel structVM in PdoStructures)
            {
                if (actPdoStruct.Name.Equals(structVM.Name) && actPdoStruct.Crc32.Equals(structVM.Crc32) && actPdoStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    structVM.NumberOfUses++;
                    break;
                }

            }
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (actPdoStruct.Name.Equals(plcStruct.Name))
                {
                    actPdoStruct.TypeNamespace = plcStruct.Namespace;
                    ret = false;
                    plcStruct.NumberOfUses++;
                    break;
                }
            }
            return ret;
        }
    }
}
