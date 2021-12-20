using PlcprojFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Utils;
using ViewModels;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GroupPdosIntoArrayIfPossible(ref BoxStructViewModel actBoxStruct, ref MapableObject mapableObject)
        {
            BoxStructViewModel ret = new BoxStructViewModel();
            
            int membersCount = actBoxStruct.StructMembers.Count;
            if (membersCount > 1)
            {
                List<BoxStructMemberViewModel> membersUngroupped = actBoxStruct.StructMembers.ToList();
                List<BoxStructMemberViewModel> membersGroupped = new List<BoxStructMemberViewModel>();
                List<MapableItem> mapableItems = mapableObject.MapableItems.ToList(); ;
                List<string> arrayTypes = new List<string>();
                int itemsCount = mapableObject.MapableItems.Count;
                int itemsModified = 0;
                bool firstItemAlreadyModified = false;
                uint sameNameIndex = 0;
                for (int i = 0; i < membersCount; i++) 
                {
                    string firstMemberPrefix = null;
                    string firstMemberSuffix = null;
                    int firstIndex = 0;
                    int arrayIndex = 0;
                    bool firstIndexIsNumber = false;
                    if (membersUngroupped[i].Name.Contains("_"))
                    {
                        firstMemberPrefix = membersUngroupped[i].Name.Substring(0, membersUngroupped[i].Name.LastIndexOf("_"));
                        firstIndexIsNumber = Int32.TryParse(membersUngroupped[i].Name.Substring(membersUngroupped[i].Name.LastIndexOf("_") + 1), out firstIndex);
                        if (arrayTypes.Contains(firstMemberPrefix))
                        {
                            sameNameIndex++;
                            firstMemberSuffix = "_" + sameNameIndex.ToString();
                            firstItemAlreadyModified = false;
                        }
                        else
                        {
                            sameNameIndex = 0;
                            firstMemberSuffix = "";
                            arrayTypes.Add(firstMemberPrefix);
                        }
                    }
                    int j = i + 1;
                    while (j < membersCount && !string.IsNullOrEmpty(firstMemberPrefix) && firstIndexIsNumber &&
                        membersUngroupped[i].Type_Value == membersUngroupped[j].Type_Value &&
                        membersUngroupped[j].Name.Contains("_") &&
                        membersUngroupped[j].Name.Substring(0, membersUngroupped[j].Name.LastIndexOf("_")) == firstMemberPrefix)
                    {
                        if (!firstItemAlreadyModified)
                        {
                            for (int k = itemsModified; k < itemsCount; k++)
                            {
                                arrayIndex = 0;
                                if (mapableItems[k].VarA.StartsWith(firstMemberPrefix + "_" + firstIndex.ToString()))
                                {
                                    //mapableItems[k].VarA = ValidatePlcItem.Link(mapableItems[k].VarA.Replace(firstMemberPrefix + "_" + firstIndex.ToString(), firstMemberPrefix + firstMemberSuffix + "[" + firstIndex.ToString() + "]"));
                                    mapableItems[k].VarA = ValidatePlcItem.Link(mapableItems[k].VarA.Replace(firstMemberPrefix + "_" + firstIndex.ToString(), firstMemberPrefix + firstMemberSuffix + "[" + arrayIndex.ToString() + "]"));
                                    itemsModified++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            firstItemAlreadyModified = true;
                            arrayIndex++;
                        }
                        string actMemberPrefix = null;
                        string actMemberSuffix = firstMemberSuffix;
                        bool actIndexIsNumber = false;
                        int actIndex = 0;
                        if (membersUngroupped[j].Name.Contains("_"))
                        {
                            actMemberPrefix = membersUngroupped[j].Name.Substring(0, membersUngroupped[j].Name.LastIndexOf("_"));
                            actIndexIsNumber = Int32.TryParse(membersUngroupped[j].Name.Substring(membersUngroupped[j].Name.LastIndexOf("_") + 1), out actIndex);
                        }

                        if (!string.IsNullOrEmpty(actMemberPrefix) && actIndexIsNumber)
                        {
                            for (int k = itemsModified; k < itemsCount; k++)
                            {
                                if (mapableItems[k].VarA.StartsWith(actMemberPrefix + "_" + actIndex.ToString()))
                                {
                                    //mapableItems[k].VarA = ValidatePlcItem.Link(mapableItems[k].VarA.Replace(actMemberPrefix + "_" + actIndex.ToString(), actMemberPrefix + actMemberSuffix + "[" + actIndex.ToString() + "]"));
                                    mapableItems[k].VarA = ValidatePlcItem.Link(mapableItems[k].VarA.Replace(actMemberPrefix + "_" + actIndex.ToString(), actMemberPrefix + actMemberSuffix + "[" + arrayIndex.ToString() + "]"));
                                    itemsModified++;
                                    arrayIndex++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        j++;
                    }
                    if (j - i > 1)
                    {
                        Int32.TryParse(membersUngroupped[i].Name.Substring(membersUngroupped[i].Name.LastIndexOf("_") + 1), out firstIndex);
                        Int32.TryParse(membersUngroupped[j - 1].Name.Substring(membersUngroupped[j - 1].Name.LastIndexOf("_") + 1), out int lastIndex);
                        //string arrayType = "ARRAY[" + firstIndex.ToString() + ".." + lastIndex.ToString() + "] OF " + membersUngroupped[i].Type_Value;
                        string arrayType = "ARRAY[0.."+ (arrayIndex -1).ToString() + "] OF " + membersUngroupped[i].Type_Value;
                        BoxStructMemberViewModel arrayMember = new BoxStructMemberViewModel();
                        arrayMember.Attributes.Add("{attribute addProperty Name \"" + firstMemberPrefix + firstMemberSuffix + "\"}");
                        arrayMember.Name = ValidatePlcItem.Name(firstMemberPrefix + firstMemberSuffix);
                        arrayMember.BoxOrderCode = membersUngroupped[i].BoxOrderCode;
                        arrayMember.Type_Value = arrayType;
                        arrayMember.InOutPlcProj = membersUngroupped[i].InOutPlcProj;
                        arrayMember.InOutMappings = membersUngroupped[i].InOutMappings;
                        arrayMember.OwnerBname = membersUngroupped[i].OwnerBname;
                        arrayMember.SizeInBites = membersUngroupped[i].SizeInBites * (uint)(lastIndex - firstIndex + 1);
                        arrayMember.SizeInBytes = membersUngroupped[i].SizeInBytes * (uint)(lastIndex - firstIndex + 1);
                        arrayMember.Index = membersUngroupped[i].Index;
                        arrayMember.IndexNumber = membersUngroupped[i].IndexNumber;
                        membersGroupped.Add(arrayMember);
                        firstItemAlreadyModified = false;
                        i = j - 1;
                    }
                    else
                    {
                        membersGroupped.Add(membersUngroupped[i]);
                        for (int k = itemsModified; k < itemsCount; k++)
                        {
                            if (mapableItems[k].VarA.StartsWith(membersUngroupped[i].Name))
                            {
                                itemsModified++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                ret.StructMembers = new ObservableCollection<BoxStructMemberViewModel>();

                foreach (BoxStructMemberViewModel member in membersGroupped)
                {
                    if (string.IsNullOrEmpty(ret.BoxOrderCode))
                    {
                        ret.BoxOrderCode = member.BoxOrderCode;
                    }
                    ret.StructMembers.Add(member);
                    ret.Id = ret.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                    ret.SizeInBites = ret.SizeInBites + member.SizeInBites;
                    ret.SizeInBytes = ret.SizeInBytes + member.SizeInBytes;
                }

                ret.Crc32 = CRC32.Calculate_CRC32(ret.Id);
                ret.Prefix = actBoxStruct.Prefix;
                ret.Name = ValidatePlcItem.Name(ret.Prefix + "_" + actBoxStruct.Crc32.ToString("X8"));
                mapableObject.MapableItems = new ObservableCollection<MapableItem>();
                foreach(MapableItem mapableItem in mapableItems)
                {
                    mapableObject.MapableItems.Add(mapableItem);
                }
            }
            else
            {
                ret = actBoxStruct;
            }
            actBoxStruct = ret;
        }
    }
}
