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
        private void GroupPdoEntriesIntoArrayIfPossibleAndModifyMapings(ref PdoStructViewModel actPdoStruct, ref PdoViewModel pdoViewModel)
        {
            PdoStructViewModel ret = new PdoStructViewModel();

            int sMembersCount = actPdoStruct.StructMembers.Count;
            if (sMembersCount > 1)
            {
                List<PdoStructMemberViewModel> sMembersIn = actPdoStruct.StructMembers.ToList();
                List<PdoStructMemberViewModel> sMembersOut = new List<PdoStructMemberViewModel>();
                for (int i = 0; i < sMembersCount; i++)
                {
                    bool isArray = false;
                    PdoStructMemberViewModel firstItem = null;
                    string firstArrayItemPrefix = null;         //prefix of the array taken from the first member of the array for example Data_5 => Data
                    int firstArrayItemIndex = -1;               //index of the array taken from the first member of the array for example Data_5 => 5
                    bool firstArrayItemIndexIsNumber = false;   //first member of the array looks like Data_5 not like Data_ABCD
                    PdoStructMemberViewModel currentItem = null;
                    string currentArrayItemPrefix = null;       //prefix of the array taken from the member of the array for example Data_5 => Data
                    int currentArrayItemIndex = -1;             //index of the array taken from the member of the array for example Data_5 => 5
                    bool currentArrayItemIndexIsNumber = false; //first member of the array looks like Data_5 not like Data_ABCD
                    //PdoStructMemberViewModel lastItem = null;
                    //string lastArrayItemPrefix = null;          //prefix of the array taken from the last member of the array for example Data_5 => Data
                    int lastArrayItemIndex = -1;                //index of the array taken from the last member of the array for example Data_5 => 5
                    //bool lastArrayItemIndexIsNumber = false;    //first member of the array looks like Data_5 not like Data_ABCD
                    int arrayDimension = -1;
                    bool endOfStructureStillNotReached = false;
                    bool arrayItemPrefixesAreEqual = false;
                    bool itemTypesAreEqual = false;
                    bool itemIndexesAreEqual = false;

                    bool CheckNextItem = false;

                    int j = 0;

                    do
                    {
                        firstItem = isArray ? firstItem : actPdoStruct.StructMembers[i];
                        if (firstItem.Name.Contains("_") && !isArray)
                        {
                            firstArrayItemPrefix = firstItem.Name.Substring(0, firstItem.Name.LastIndexOf("_", StringComparison.Ordinal));
                            firstArrayItemIndexIsNumber = String.IsNullOrEmpty(firstArrayItemPrefix) ? false : Int32.TryParse(firstItem.Name.Replace(firstArrayItemPrefix,"").Replace("_",""), out firstArrayItemIndex);
                            arrayDimension = 1;
                            j = i + 1;
                        }
                        if (firstArrayItemIndexIsNumber)
                        {
                            endOfStructureStillNotReached = j < sMembersCount;
                            arrayItemPrefixesAreEqual = false;
                            itemTypesAreEqual = false;
                            itemIndexesAreEqual = false;
                            if (endOfStructureStillNotReached)
                            {
                                currentItem = actPdoStruct.StructMembers[j];
                                if (currentItem.Name.Contains("_") )
                                {
                                    currentArrayItemPrefix = currentItem.Name.Substring(0, currentItem.Name.LastIndexOf("_", StringComparison.Ordinal));
                                    currentArrayItemIndexIsNumber = String.IsNullOrEmpty(currentArrayItemPrefix) ? false : Int32.TryParse(currentItem.Name.Replace(currentArrayItemPrefix, "").Replace("_", ""), out currentArrayItemIndex);
                                    arrayItemPrefixesAreEqual = currentArrayItemIndexIsNumber ? currentArrayItemPrefix.Equals(firstArrayItemPrefix) : false;
                                    itemTypesAreEqual = currentArrayItemIndexIsNumber ? currentItem.Type_Value.Equals(firstItem.Type_Value) : false;
                                    itemIndexesAreEqual = currentArrayItemIndexIsNumber ? currentItem.Index.Equals(firstItem.Index) : false;
                                    isArray = !isArray ? arrayItemPrefixesAreEqual  && itemTypesAreEqual  && itemIndexesAreEqual : isArray;
                                    arrayDimension += isArray && arrayItemPrefixesAreEqual && itemTypesAreEqual && itemIndexesAreEqual ? 1 : 0;
                                    lastArrayItemIndex = isArray && arrayItemPrefixesAreEqual && itemTypesAreEqual && itemIndexesAreEqual ? currentArrayItemIndex : lastArrayItemIndex;
                                }
                            }
                        }
                        j++;
                        endOfStructureStillNotReached = j < sMembersCount;
                        CheckNextItem = isArray && endOfStructureStillNotReached && arrayItemPrefixesAreEqual && itemTypesAreEqual && itemIndexesAreEqual;

                        //Temporary control condition =>                       
                        if (CheckNextItem)
                        {
                            if (pdoViewModel.PdoEntriesStructured[i].MapableObject.MapableItems.Count > 1 || pdoViewModel.PdoEntriesStructured[j].MapableObject.MapableItems.Count > 1)
                            {
                            }
                        }
                        //<= Temporary control condition
                    }
                    while (CheckNextItem);
                    if (isArray)
                    {
                        if(lastArrayItemIndex - firstArrayItemIndex == arrayDimension - 1 )
                        {
                            //Modifying structure members =>
                            string arrayType = "ARRAY[0.." + (arrayDimension - 1 ).ToString() + "] OF " + sMembersIn[i].Type_Value;
                            PdoStructMemberViewModel arrayMember = new PdoStructMemberViewModel();
                            arrayMember.Attributes.Add("{attribute addProperty Name \"" + firstArrayItemPrefix + "\"}");
                            arrayMember.Name = ValidatePlcItem.Name(firstArrayItemPrefix);
                            arrayMember.BoxOrderCode = sMembersIn[i].BoxOrderCode;
                            arrayMember.Type_Value = arrayType;
                            arrayMember.TypeNamespace = sMembersIn[i].TypeNamespace;
                            arrayMember.InOutPlcProj = sMembersIn[i].InOutPlcProj;
                            arrayMember.InOutMappings = sMembersIn[i].InOutMappings;
                            arrayMember.OwnerBname = sMembersIn[i].OwnerBname;
                            arrayMember.Size = sMembersIn[i].Size * arrayDimension;
                            arrayMember.Index = sMembersIn[i].Index;
                            arrayMember.IndexNumber = sMembersIn[i].IndexNumber;
                            sMembersOut.Add(arrayMember);
                            // <= Modifying structure members

                            //Temporary control condition =>                       
                            if (sMembersCount != pdoViewModel.PdoEntriesStructured.Count)
                            {
                            }
                            //<= Temporary control condition
                            //Modifying mappings =>
                            currentArrayItemIndex = 0;
                            for (int k = i; k < i + arrayDimension ; k++)
                            {
                                foreach (MappableItem mappableItem in pdoViewModel.PdoEntriesStructured[k].MapableObject.MapableItems )
                                {
                                    if (mappableItem.VarA.Contains(tmpStructSeparator))
                                    {
                                        string[] items = mappableItem.VarA.Split(tmpStructSeparator);
                                        items[0] = items[0].Substring(0, items[0].LastIndexOf('_')) + "[" + currentArrayItemIndex + "]";
                                        mappableItem.VarA = items[0] + tmpStructSeparator + items[1]; 
                                    }
                                    else
                                    {
                                        mappableItem.VarA = mappableItem.VarA.Substring(0, mappableItem.VarA.LastIndexOf('_')) + "[" + currentArrayItemIndex + "]";
                                    }
                                }
                                currentArrayItemIndex++;
                            }
                            // <= Modifying mappings

                            i = isArray ? i + arrayDimension - 1 : j-1;
                            isArray = false;
                        }
                    }
                    else
                    {
                        sMembersOut.Add(sMembersIn[i]);
                    }
                }

                ret.StructMembers = new ObservableCollection<PdoStructMemberViewModel>();

                foreach (PdoStructMemberViewModel member in sMembersOut)
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