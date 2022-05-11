using iXlinker.Resources;
using iXlinkerDtos;
using PlcprojFile;
using Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddExtensionFromBasePdoStructure(PdoStructViewModel actPdoStruct)
        {
            //Apply extension from dictionary
            foreach (StructureBase structureBase in StructureBasesResourceDictionary)
            {
                if (structureBase.StructureType.Equals(actPdoStruct.GetType().ToString()) && (structureBase.StructureName.Equals(actPdoStruct.Name) || structureBase.StructureName.Equals("*")))
                {
                    actPdoStruct.Extends = ValidatePlcItem.NameIncludingNamespace(structureBase.BaseStructureNamespace, structureBase.BaseStructureName);
                    break;
                }
            }

            //Apply extension for input base and output base
            if (string.IsNullOrEmpty(actPdoStruct.Extends))
            {
                bool input = false;
                bool output = false;
                string inputBaseStructPrefix = "InputBase";
                string outputBaseStructPrefix = "OutputBase";
                string inputBaseStructName = inputBaseStructPrefix + "_" + CRC32.Calculate_CRC32(inputBaseStructPrefix).ToString("X8");
                string outputBaseStructName = outputBaseStructPrefix + "_" + CRC32.Calculate_CRC32(outputBaseStructPrefix).ToString("X8");

                foreach (PdoStructMemberViewModel member in actPdoStruct.StructMembers)
                {
                    if (member.InOutMappings.Equals("Inputs"))
                    {
                        input = true;
                    }
                    if (member.InOutMappings.Equals("Outputs"))
                    {
                        output = true;
                    }
                }
                if (input && !output)
                {
                    actPdoStruct.Extends = ValidatePlcItem.NameIncludingNamespace(FindStructureBaseNamespace(inputBaseStructName), inputBaseStructName);
                }
                else if (output && !input)
                {
                    actPdoStruct.Extends = ValidatePlcItem.NameIncludingNamespace(FindStructureBaseNamespace(outputBaseStructName), outputBaseStructName);
                }
                else if(!input && !output)
                {
                }
                else
                {
                }
            }

            //Delete extension for specific types
            foreach (StructureBase structureBase in StructureBasesResourceDictionary)
            {
                if (structureBase.StructureType.Equals(actPdoStruct.GetType().ToString()) && structureBase.StructureName.Equals(actPdoStruct.Name) && structureBase.BaseStructurePrefix.Equals("!"))
                {
                    actPdoStruct.Extends = "";
                    break;
                }
            }
            if (string.IsNullOrEmpty(actPdoStruct.Extends))
            {

            }
        }
    }
}

