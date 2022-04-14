using PlcprojFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Utils;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GroupPdoEntriesIntoArrayIfPossible(ref PdoStructViewModel actPdoStruct, ref MappableObject mapableObject)
        {
            PdoStructViewModel ret = new PdoStructViewModel();

            int membersCount = actPdoStruct.StructMembers.Count;
            if (membersCount > 1)
            {
                List<PdoStructMemberViewModel> membersUngroupped = actPdoStruct.StructMembers.ToList();
                List<PdoStructMemberViewModel> membersGroupped = new List<PdoStructMemberViewModel>();
  
                for (int i = 0; i < membersCount; i++)
                {
                    string firstMemberPrefix = null;
                    int firstIndex = 0;
                    int arrayIndex = 0;
                    bool firstIndexIsNumber = false;
                    if (membersUngroupped[i].Name.Contains("_"))
                    {
                        firstMemberPrefix = membersUngroupped[i].Name.Substring(0, membersUngroupped[i].Name.LastIndexOf("_", StringComparison.Ordinal));
                        firstIndexIsNumber = Int32.TryParse(membersUngroupped[i].Name.Substring( membersUngroupped[i].Name.LastIndexOf("_", StringComparison.Ordinal)+1), out firstIndex);
                    }
                    int j = i + 1;
                    while (j < membersCount && !string.IsNullOrEmpty(firstMemberPrefix) && firstIndexIsNumber &&
                        membersUngroupped[i].Type_Value == membersUngroupped[j].Type_Value &&
                        membersUngroupped[j].Name.Contains("_") &&
                        membersUngroupped[j].Name.Substring(0, membersUngroupped[j].Name.LastIndexOf("_", StringComparison.Ordinal)) == firstMemberPrefix)
                    {
                        if (mapableObject.MapableItems[i].VarA.Contains("_") && Int32.TryParse(mapableObject.MapableItems[i].VarA.Substring(mapableObject.MapableItems[i].VarA.LastIndexOf("_", StringComparison.Ordinal) + 1), out firstIndex))
                        {
                            arrayIndex = 0;
                            mapableObject.MapableItems[i].VarA = mapableObject.MapableItems[i].VarA.Substring(0, mapableObject.MapableItems[i].VarA.LastIndexOf("_", StringComparison.Ordinal)) + "[" + arrayIndex.ToString() + "]";
                            arrayIndex++;
                        }
                        if (mapableObject.MapableItems[j].VarA.Contains("_") && Int32.TryParse(mapableObject.MapableItems[j].VarA.Substring(mapableObject.MapableItems[j].VarA.LastIndexOf("_", StringComparison.Ordinal) + 1), out int actIndex))
                        {
                            mapableObject.MapableItems[j].VarA = mapableObject.MapableItems[j].VarA.Substring(0, mapableObject.MapableItems[j].VarA.LastIndexOf("_", StringComparison.Ordinal)) + "[" + arrayIndex.ToString() + "]";
                            arrayIndex++;
                        }
                        mapableObject.MapableItems[i].VarA = ValidatePlcItem.Link(mapableObject.MapableItems[i].VarA);
                        mapableObject.MapableItems[j].VarA = ValidatePlcItem.Link(mapableObject.MapableItems[j].VarA);
                        j++;
                    }
                    if (j - i > 1)
                    {
                        Int32.TryParse(membersUngroupped[i].Name.Substring(membersUngroupped[i].Name.LastIndexOf("_", StringComparison.Ordinal) + 1), out firstIndex);
                        Int32.TryParse(membersUngroupped[j - 1].Name.Substring(membersUngroupped[j - 1].Name.LastIndexOf("_", StringComparison.Ordinal) + 1), out int lastIndex);
                        string arrayType = "ARRAY[0 .." + (arrayIndex - 1).ToString() + "] OF " + membersUngroupped[i].Type_Value;
                        PdoStructMemberViewModel arrayMember = new PdoStructMemberViewModel();
                        arrayMember.Attributes.Add("{attribute addProperty Name \"" + firstMemberPrefix + "\"}");
                        arrayMember.Name = ValidatePlcItem.Name(firstMemberPrefix);
                        arrayMember.BoxOrderCode = membersUngroupped[i].BoxOrderCode;
                        arrayMember.Type_Value = arrayType;
                        arrayMember.TypeNamespace = membersUngroupped[i].TypeNamespace;
                        arrayMember.InOutPlcProj = membersUngroupped[i].InOutPlcProj;
                        arrayMember.InOutMappings = membersUngroupped[i].InOutMappings;
                        arrayMember.OwnerBname = membersUngroupped[i].OwnerBname;
                        arrayMember.Size = membersUngroupped[i].Size * (uint)(lastIndex - firstIndex + 1);
                        arrayMember.Index = membersUngroupped[i].Index;
                        arrayMember.IndexNumber = membersUngroupped[i].IndexNumber;
                        membersGroupped.Add(arrayMember);
                        i = j - 1;
                    }
                    else
                    {
                        membersGroupped.Add(membersUngroupped[i]);
                    }
                }

                ret.StructMembers = new ObservableCollection<PdoStructMemberViewModel>();

                foreach (PdoStructMemberViewModel member in membersGroupped)
                {
                    if (string.IsNullOrEmpty(ret.BoxOrderCode))
                    {
                        ret.BoxOrderCode = member.BoxOrderCode;
                    }
                    ret.StructMembers.Add(member);
                    ret.Id = ret.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
                    ret.Size = ret.Size + member.Size;
                }

                ret.Crc32 = CRC32.Calculate_CRC32(ret.Id);
                ret.Prefix = actPdoStruct.Prefix;
                ret.Name = ValidatePlcItem.Name(ret.Prefix + "_" + actPdoStruct.Crc32.ToString("X8"));
            }
            else
            {
                ret = actPdoStruct;
            }
            actPdoStruct = ret;
        }
    }
}
