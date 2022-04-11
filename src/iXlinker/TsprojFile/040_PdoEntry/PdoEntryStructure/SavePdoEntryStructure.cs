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
            uint sizeInBites = 0;
            double sizeInBytes = 0;

            foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
            {
                member.Name = ValidatePlcItem.Name(member.NameUnmodified);
                List<string> memberAttributes = new List<string>();
                memberAttributes.Add("{attribute addProperty Name \"" + member.Name + "\"}");
                member.Attributes = memberAttributes;
                Id = Id + member.Name + member.TypeNamespace + member.Type_Value + member.SizeInBites + member.SizeInBytes;
                sizeInBites = sizeInBites + member.SizeInBites;
                sizeInBytes = sizeInBytes + member.SizeInBytes;
            }
            actPdoEntryStruct.Id = Id;
            actPdoEntryStruct.SizeInBites = sizeInBites;
            actPdoEntryStruct.SizeInBytes = sizeInBytes;
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
