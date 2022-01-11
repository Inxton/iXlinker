using System;
using System.Linq;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool ReplacePdoEntryStructureByArrayIfPossible(ref PdoEntryStructViewModel actPdoEntryStruct)
        {
            bool isArray = true;

            if (actPdoEntryStruct.StructMembers.Count > 0)
            {
                PdoEntryStructMemberViewModel firstMember = actPdoEntryStruct.StructMembers.FirstOrDefault();
                PdoEntryStructMemberViewModel prevMember = null;
                string firstMemberNamePrefix = null;
                if (firstMember.Name.Contains("_"))
                {
                    firstMemberNamePrefix = firstMember.Name.Substring(0, firstMember.Name.LastIndexOf("_", StringComparison.Ordinal));
                }
                string firstMemberArrayIndex = null;
                if (firstMember.Name.Contains("_"))
                {
                    firstMemberArrayIndex = firstMember.Name.Substring(firstMember.Name.LastIndexOf("_", StringComparison.Ordinal) + 1);
                }
                int firstIndex = 0;
                int lastIndex = 0;

                if (firstMemberNamePrefix == null || firstMemberArrayIndex == null || !Int32.TryParse(firstMemberArrayIndex, out firstIndex))
                {
                    isArray = false;
                }
                else
                {
                    prevMember = firstMember;
                    foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
                    {
                        string memberNamePrefix = null;
                        if (member.Name.Contains("_"))
                        {
                            memberNamePrefix = member.Name.Substring(0, member.Name.LastIndexOf("_", StringComparison.Ordinal));
                        }
                        if (memberNamePrefix != firstMemberNamePrefix || member.Type_Value != firstMember.Type_Value || member.Index != firstMember.Index || (member.SubIndex != firstMember.SubIndex && member.SubIndexNumber != prevMember.SubIndexNumber + 1))
                        {
                            isArray = false;
                            break;
                        }
                        prevMember = member;
                    }
                    string lastMemberArrayIndex = null;
                    if (prevMember.Name.Contains("_"))
                    {
                        lastMemberArrayIndex = prevMember.Name.Substring(prevMember.Name.LastIndexOf("_", StringComparison.Ordinal) + 1);
                    }

                    if (lastMemberArrayIndex == null || !Int32.TryParse(lastMemberArrayIndex, out lastIndex))
                    {
                        isArray = false;
                    }
                    else
                    {
                        if(Math.Abs(lastIndex - firstIndex) + 1 != actPdoEntryStruct.StructMembers.Count)
                        {
                            isArray = false;
                        }
                    }
                }
                if (isArray)
                {
                    actPdoEntryStruct.StructMembers = null;
                    firstMember.Type_Value = "ARRAY [" + firstIndex.ToString() + ".." + lastIndex.ToString() + "] OF " + firstMember.Type_Value;
                }
            }

            return isArray;
        }
    }
}
