using iXlinker.Resources;
using iXlinkerDtos;
using Utils;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddExtensionFromBaseBoxStructure(BoxStructViewModel actBoxStruct)
        {
            foreach (StructureBase structureBase in StructureBasesResourceDictionary)
            {
                if (structureBase.StructureType.Equals(actBoxStruct.GetType().ToString()) && (structureBase.StructureName.Equals(actBoxStruct.Name) || structureBase.StructureName.Equals("*")))
                {
                    actBoxStruct.Extends = ValidatePlcItem.NameIncludingNamespace(structureBase.BaseStructureNamespace, structureBase.BaseStructureName);
                    break;
                }
            }
        }
    }
}

