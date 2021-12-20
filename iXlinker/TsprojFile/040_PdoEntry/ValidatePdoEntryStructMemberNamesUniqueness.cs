using System;
using ViewModels;
using System.Collections.Generic;
using System.IO;

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
                    if (structEntryNames.Contains(structMember.Name))
                    {
                        sameNameIndex = 1;
                        string previousName = structMember.Name;
                        string newName = structMember.Name + "_" + sameNameIndex.ToString();
                        while (structEntryNames.Contains(newName))
                        {
                            sameNameIndex++;
                            newName = structMember.Name + "_" + sameNameIndex.ToString();
                        }
                        Console.WriteLine("Not unique pdo entry struct member name {0} found in the structure name {1}, in the box type {2}!!!", structMember.Name, actStruct.Prefix, actStruct.BoxOrderCode);
                        if (exportDuplicities)
                        {
                            using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\PdoEntryStructMemberNamesDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", actStruct.BoxOrderCode, actStruct.Prefix, structMember.Name);
                            }
                        }

                        Console.WriteLine(@"\t Renamed to from ""{0}"" to ""{1}"" !!!", previousName,newName);
                        structMember.Name = newName;
                        structEntryNames.Add(structMember.Name);
                    }
                    else
                    {
                        structEntryNames.Add(structMember.Name);
                    }
                }
            }
        }
    }
}
