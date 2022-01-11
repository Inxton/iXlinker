using System;
using iXlinkerDtos;
using System.Collections.Generic;
using System.IO;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ValidateDeviceStructMemberNamesUniqueness(ref DeviceStructViewModel actStruct)
        {
            List<string> structEntryNames = new List<string>();
            List<string> structEntryNamesDuplicities = new List<string>();
            int sameNameIndex = 1;
            if (actStruct.StructMembers != null)
            {
                foreach (DeviceStructMemberViewModel structMember in actStruct.StructMembers)
                {
                    if (structEntryNames.Contains(structMember.Name))
                    {
                        if (!structEntryNamesDuplicities.Contains(structMember.Name))
                        {
                            sameNameIndex = 1;
                            structEntryNamesDuplicities.Add(structMember.Name);
                        }
                        Console.WriteLine("Not unique device struct member name {0} found in the structure name {1}, in the device type {2}!!!", structMember.Name, actStruct.Prefix, actStruct.BoxOrderCode);
                        if (exportDuplicities)
                        {
                            using(StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\DeviceStructMemberNamesDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", actStruct.BoxOrderCode, actStruct.Prefix, structMember.Name);
                            }
                        }
                        structMember.Name = structMember.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        Console.WriteLine("\t Renamed to {0}!!!", structMember.Name);
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
