using iXlinker.Resources;
using iXlinkerDtos;
using Utils;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ReplacePdoStructureNameByItsReplacement(PdoStructViewModel actPdoStruct)
        {
            foreach (PdoStructureDuplicate pdoStructureDuplicate in PdoStructureDuplicates)
            {
                if (pdoStructureDuplicate.OriginalName.Equals(actPdoStruct.Name))
                {
                    actPdoStruct.Name = pdoStructureDuplicate.ReplacementName;
                    break;
                }
            }
        }
    }
}

