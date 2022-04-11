using System;
using System.Linq;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool IsArrayOfPdoEntryStructureItem(PdoEntryStructViewModel actPdoEntryStruct, out int lowIndex, out int highIndex, out string typeValue, out string typeNamespace)
        {
            bool isArray = true;
            int _lowIndex = 0;
            int _highIndex = 0;
            PdoEntryStructMemberViewModel firstMember = actPdoEntryStruct.StructMembers.FirstOrDefault();

            if (actPdoEntryStruct.StructMembers.Count > 1)
            {
                string firstMemberNamePrefix = (firstMember.Name != null && firstMember.Name.Contains("_")) ? firstMember.Name.Substring(0, firstMember.Name.LastIndexOf("_", StringComparison.Ordinal)) : null;
                string firstMemberArrayIndex = firstMemberNamePrefix != null ? firstMemberArrayIndex = firstMember.Name.Substring(firstMember.Name.LastIndexOf("_", StringComparison.Ordinal) + 1) : null;

                if (firstMemberNamePrefix == null || firstMemberArrayIndex == null || !Int32.TryParse(firstMemberArrayIndex, out _lowIndex) || firstMember.Type_Value.Equals("BIT"))
                {
                    isArray = false;
                }
                else
                {
                    PdoEntryStructMemberViewModel prevMember = firstMember;
                    foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
                    {
                        string memberNamePrefix = (member.Name != null && member.Name.Contains("_")) ? member.Name.Substring(0, member.Name.LastIndexOf("_", StringComparison.Ordinal)) : null;
                        if (memberNamePrefix != firstMemberNamePrefix || member.Type_Value != firstMember.Type_Value || member.Index != firstMember.Index || (member.SubIndex != firstMember.SubIndex && member.SubIndexNumber != prevMember.SubIndexNumber + 1))
                        {
                            isArray = false;
                            break;
                        }
                        prevMember = member;
                    }
                    string lastMemberArrayIndex = (prevMember.Name != null && prevMember.Name.Contains("_")) ? prevMember.Name.Substring(prevMember.Name.LastIndexOf("_", StringComparison.Ordinal) + 1) : null;

                    if (lastMemberArrayIndex == null || !Int32.TryParse(lastMemberArrayIndex, out _highIndex))
                    {
                        isArray = false;
                    }
                    else
                    {
                        if (Math.Abs(_highIndex - _lowIndex) + 1 != actPdoEntryStruct.StructMembers.Count)
                        {
                            isArray = false;
                        }
                    }
                }
            }
            else
            {
                isArray = false;
            }
            lowIndex = isArray ? _lowIndex : 0;
            highIndex = isArray ? _highIndex : 0;
            typeValue = isArray ? firstMember.Type_Value : null;
            typeNamespace = isArray ? firstMember.TypeNamespace : null;
            return isArray;
        }
    }
}
