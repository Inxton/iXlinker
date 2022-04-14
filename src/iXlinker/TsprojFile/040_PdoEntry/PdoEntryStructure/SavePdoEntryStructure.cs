using iXlinkerDtos;
using PlcprojFile;
using System.Collections.Generic;
using Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void SavePdoEntryStructure(ref PdoEntryStructViewModel actPdoEntryStruct)
        {
            string Id = "";
            double size = 0;

            foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
            {
                member.NameA = ValidatePlcItem.Name(member.NameA);
                List<string> memberAttributes = new List<string>();
                memberAttributes.Add("{attribute addProperty Name \"" + member.NameA + "\"}");
                member.Attributes = memberAttributes;
                Id = Id + member.NameA + member.TypeNamespace + member.Type_Value + member.Size;
                size = size + member.Size;
            }
            actPdoEntryStruct.Id = Id;
            actPdoEntryStruct.Size = size;
            actPdoEntryStruct.Crc32 = CRC32.Calculate_CRC32(Id);
            actPdoEntryStruct.Name = actPdoEntryStruct.Prefix + "_" + actPdoEntryStruct.Crc32.ToString("X8");

            //Check if such an structure exists
            if (CheckIfPdoEntryStructureDoesNotExist(actPdoEntryStruct))
            {
                //if not add to the structure list
                PdoEntryStructures.Add(actPdoEntryStruct);
            }
        }
    }
}
