using System;
using iXlinkerDtos;
using System.Collections.Generic;
using System.IO;
using iXlinker.Utils;

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
                        EventLogger.Instance.Logger.Information("Not unique device struct member name {0} found in the structure name {1}, in the device type {2}!!!", structMember.Name, actStruct.Prefix, actStruct.BoxOrderCode);

                        structMember.Name = structMember.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        EventLogger.Instance.Logger.Information("\t Renamed to {0}!!!", structMember.Name);
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
