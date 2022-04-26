using System;
using iXlinkerDtos;
using System.Collections.Generic;
using System.IO;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ValidatePdoEntryStructMemberNamesUniqueness(ref PdoEntryStructViewModel actStruct)
        {
            List<string> structEntryNames = new List<string>();

            int sameNameIndex = 1;
            if (actStruct.StructMembers != null)
            {
                foreach (PdoEntryStructMemberViewModel structMember in actStruct.StructMembers)
                {
                    if (structEntryNames.Contains(structMember.NameA))
                    {
                        sameNameIndex = 1;
                        string previousName = structMember.NameA;
                        string newName = structMember.NameA + "_" + sameNameIndex.ToString();
                        while (structEntryNames.Contains(newName))
                        {
                            sameNameIndex++;
                            newName = structMember.NameA + "_" + sameNameIndex.ToString();
                        }
                        EventLogger.Instance.Logger.Information("Not unique pdo entry struct member name {0} found in the structure name {1}, in the box type {2}!!!", structMember.NameA, actStruct.Prefix, actStruct.BoxOrderCode);
                        EventLogger.Instance.Logger.Information(@"\t Renamed to from ""{0}"" to ""{1}"" !!!", previousName,newName);
                        structMember.NameA = newName;
                        structMember.NameB = newName;
                        structEntryNames.Add(structMember.NameA);
                    }
                    else
                    {
                        structEntryNames.Add(structMember.NameA);
                    }
                }
            }
        }
    }
}
