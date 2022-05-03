using iXlinker.Resources;
using iXlinkerDtos;
using Utils;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddExtensionFromBasePdoStructure(PdoStructViewModel actPdoStruct)
        {
            foreach (StructureBase structureBase in StructureBasesResourceDictionary)
            {
                if (structureBase.StructureType.Equals(actPdoStruct.GetType().ToString()) && (structureBase.StructureName.Equals(actPdoStruct.Name) || structureBase.StructureName.Equals("*")))
                {
                    actPdoStruct.Extends = ValidatePlcItem.NameIncludingNamespace(structureBase.BaseStructureNamespace, structureBase.BaseStructureName);
                    break;
                }
            }
        }
    }
}

