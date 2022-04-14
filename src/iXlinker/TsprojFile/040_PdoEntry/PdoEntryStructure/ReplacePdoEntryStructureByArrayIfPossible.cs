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

            if (actPdoEntryStruct.StructMembers.Count > 1)
            {
                PdoEntryStructMemberViewModel firstMember = actPdoEntryStruct.StructMembers.FirstOrDefault();
                PdoEntryStructMemberViewModel prevMember = null;
                string firstMemberNamePrefix = null;
                if (firstMember.NameA.Contains("_"))
                {
                    firstMemberNamePrefix = firstMember.NameA.Substring(0, firstMember.NameA.LastIndexOf("_", StringComparison.Ordinal));
                }
                string firstMemberArrayIndex = null;
                if (firstMember.NameA.Contains("_"))
                {
                    firstMemberArrayIndex = firstMember.NameA.Substring(firstMember.NameA.LastIndexOf("_", StringComparison.Ordinal) + 1);
                }
                int firstIndex = 0;
                int lastIndex = 0;

                if (firstMember.Type_Value.Equals("BIT") || firstMemberNamePrefix == null || firstMemberArrayIndex == null || !Int32.TryParse(firstMemberArrayIndex, out firstIndex))
                {
                    isArray = false;
                }
                else
                {
                    prevMember = firstMember;
                    foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
                    {
                        string memberNamePrefix = null;
                        if (member.NameA.Contains("_"))
                        {
                            memberNamePrefix = member.NameA.Substring(0, member.NameA.LastIndexOf("_", StringComparison.Ordinal));
                        }
                        if (memberNamePrefix != firstMemberNamePrefix || member.Type_Value != firstMember.Type_Value || member.Index != firstMember.Index || (member.SubIndex != firstMember.SubIndex && member.SubIndexNumber != prevMember.SubIndexNumber + 1))
                        {
                            isArray = false;
                            break;
                        }
                        prevMember = member;
                    }
                    string lastMemberArrayIndex = null;
                    if (prevMember.NameA.Contains("_"))
                    {
                        lastMemberArrayIndex = prevMember.NameA.Substring(prevMember.NameA.LastIndexOf("_", StringComparison.Ordinal) + 1);
                    }

                    if (lastMemberArrayIndex == null || !Int32.TryParse(lastMemberArrayIndex, out lastIndex))
                    {
                        isArray = false;
                    }
                    else
                    {
                        if (Math.Abs(lastIndex - firstIndex) + 1 != actPdoEntryStruct.StructMembers.Count)
                        {
                            isArray = false;
                        }
                    }
                }
                if (isArray)
                {
                    lastIndex = lastIndex - firstIndex;
                    firstIndex = 0;
                    actPdoEntryStruct.StructMembers = null;
                    firstMember.Type_Value = "ARRAY [" + firstIndex.ToString() + ".." + lastIndex.ToString() + "] OF " + firstMember.Type_Value;
                    firstMember.TypeNamespace = firstMember.TypeNamespace;
                }
            }
            else
            {
                isArray = false;
            }
            return isArray;
        }
    }
}
