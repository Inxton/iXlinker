using System;
using iXlinkerDtos;
using System.Collections.Generic;
using System.IO;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ValidateVarGrpVarStructMemberNamesUniqueness(ref PdoEntryStructViewModel actStruct)
        {
            List<string> structEntryNames = new List<string>();
            List<string> structEntryNamesDuplicities = new List<string>();

            int sameNameIndex = 1;
            if (actStruct.StructMembers != null)
            {
                foreach (PdoEntryStructMemberViewModel structMember in actStruct.StructMembers)
                {
                    if (structEntryNames.Contains(structMember.NameA))
                    {
                        if (!structEntryNamesDuplicities.Contains(structMember.NameA))
                        {
                            sameNameIndex = 1;
                            structEntryNamesDuplicities.Add(structMember.NameA);
                        }
                        EventLogger.Instance.Logger.Information("Not unique variable struct member name {0} found in the structure name {1}, in the box type {2}!!!", structMember.NameA, actStruct.Name, actStruct.BoxOrderCode);
                        if (exportDuplicities)
                        {
                            using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\VarGrpVarStructMemberNamesDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", actStruct.BoxOrderCode, actStruct.Prefix, structMember.NameA);
                            }
                        }
                        structMember.NameA = structMember.NameA + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        EventLogger.Instance.Logger.Information("\t Renamed to {0}!!!", structMember.NameA);
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
