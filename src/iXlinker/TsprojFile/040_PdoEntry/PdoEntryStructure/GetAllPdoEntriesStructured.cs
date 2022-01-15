using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using System.Collections.Generic;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoEntryViewModel> GetAllPdoEntriesStructured(EtherCATSlavePdo pdo, PdoViewModel pdoViewModel, ObservableCollection<PdoEntryViewModel> pdoEntriesUnstructured)
        {
            ObservableCollection<PdoEntryViewModel> pdoEntriesStructured = new ObservableCollection<PdoEntryViewModel>();
            if (pdo.Entry != null && pdoEntriesUnstructured != null && pdoEntriesUnstructured.Count > 0)
            {
                uint reserveIndex = 1;
                PdoEntryStructViewModel actPdoEntryStruct = new PdoEntryStructViewModel();

                int pdoEntriesUnstructuredCount = pdoEntriesUnstructured.Count;

                for (int podEntryIndex = 0; podEntryIndex < pdoEntriesUnstructuredCount; podEntryIndex++)
                {
                    PdoEntryViewModel pdoEntryUnstructured = pdoEntriesUnstructured[podEntryIndex];
                    PdoEntryViewModel nextPdoEntryUnstructured = new PdoEntryViewModel();
                    string pdoEntryUnstructuredPrefix = null;
                    string nextPdoEntryUnstructuredPrefix = null;

                    bool structOpen = false;

                    if (podEntryIndex < pdoEntriesUnstructuredCount - 1)
                    {
                        nextPdoEntryUnstructured = pdoEntriesUnstructured[podEntryIndex + 1];
                        //In case of pdo entry does not contain name, for example in case of EL3201 7bit the gap between Status.Error and Status.TxPDO State, name is taken from actPdoEntryStruct.Prefix
                        if (string.IsNullOrEmpty(nextPdoEntryUnstructured.Name) && actPdoEntryStruct != null && actPdoEntryStruct.Prefix != null)
                        {
                            nextPdoEntryUnstructured.Name = actPdoEntryStruct.Prefix + "__";
                        }
                    }
                    //In case of pdo entry does not contain name, for example in case of EL3201 7bit the gap between Status.Error and Status.TxPDO State, name is taken from actPdoEntryStruct.Prefix
                    if (string.IsNullOrEmpty(pdoEntryUnstructured.Name) && actPdoEntryStruct != null && actPdoEntryStruct.Prefix != null)
                    {
                        pdoEntryUnstructured.Name = actPdoEntryStruct.Prefix + "__";
                    }
                    //Check if some pdo entries is structured
                    if (pdoEntryUnstructured.Name != null && pdoEntryUnstructured.Name.Contains("__"))
                    {
                        if (actPdoEntryStruct == null || actPdoEntryStruct.Prefix == null && actPdoEntryStruct.Id == null)   //First member of the structure
                        {
                            actPdoEntryStruct = new PdoEntryStructViewModel() { Prefix = ValidatePlcItem.Name(pdoEntryUnstructured.Name.Substring(0, pdoEntryUnstructured.Name.IndexOf("__", StringComparison.Ordinal))), Id = "", BoxOrderCode = pdoEntryUnstructured.BoxOrderCode };
                            structOpen = true;
                        }
                        if (structOpen && actPdoEntryStruct.PdoEntryVarA == null && pdoEntryUnstructured.VarA != null)
                            actPdoEntryStruct.PdoEntryVarA = ValidatePlcItem.Name(pdoEntryUnstructured.VarB.Substring(0, pdoEntryUnstructured.VarB.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal)));
                        if (structOpen && actPdoEntryStruct.PdoEntryVarB == null && pdoEntryUnstructured.VarB != null)
                            actPdoEntryStruct.PdoEntryVarB = pdoEntryUnstructured.VarB.Substring(0, pdoEntryUnstructured.VarB.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));

                        //Get pdo entry name, if empty bites used, generate unique name with attribute hide
                        string pdoEntryName = ValidatePlcItem.Name(pdoEntryUnstructured.Name.Substring(actPdoEntryStruct.Prefix.Length + 2));
                        List<string> pdoEntryAttributes = new List<string>();
                        if (pdoEntryName == "" || pdoEntryUnstructured.Type_GUID !=null)
                        {
                            pdoEntryAttributes.Add(attributeHide);

                            pdoEntryName = "reserve_" + reserveIndex.ToString();

                            reserveIndex++;
                        }
                        else
                        {
                            pdoEntryAttributes.Add("{attribute addProperty Name \"" + pdoEntryName + "\"}");
                        }

                        string pdoEntryType = pdoEntryUnstructured.Type_Value;

                        //"Replacement" BITx by ARRAY [0..x-1] of BIT
                        if (!pdoEntryType.Contains("ARRAY") && pdoEntryType.Contains("BIT") && pdoEntryType.Length > 3)
                        {
                            uint hiIndex = Convert.ToUInt32(pdoEntryType.Substring(3, pdoEntryType.Length - 3)) - 1;
                            pdoEntryType = "ARRAY [0.." + hiIndex.ToString() + "] OF BIT";
                        }
                        //For example terminal EL5101-0011 ProductCode="#x13ed3052" RevisionNo="#x0010000b" contains TxPdo "ENC TxPdo 1 Samples Counter value" with index "x1a10" with <Name>Counter value__ARRAY [0]</Name> and < DataType > UDINT </ DataType >
                        //In such a case "Counter_value" will be the structure instance name and "Counter_value_0" will be the member of this structure
                        if (pdoEntryName.ToUpper().Contains("ARRAY"))
                        {
                            string s = pdoEntryName.ToUpper().Replace("ARRAY", "").Replace("[", "").Replace("]", "").Replace(".", "").Replace(" ", "");
                            pdoEntryName = actPdoEntryStruct.Prefix + "_" + s;
                        }

                        pdoEntryName = ValidatePlcItem.Name(pdoEntryName);

                        //"Replacement" one pdo entry of ARRAY type with several entries of the base type
                        if (pdoEntryType.ToUpper().Contains("ARRAY"))
                        {
                            string[] separators = { "ARRAY", " ", "[", "]", "..", "OF" };
                            string[] typeValueSeparated = pdoEntryType.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            int loIndex = Convert.ToInt32(typeValueSeparated[0]);
                            int hiIndex = Convert.ToInt32(typeValueSeparated[1]);
                            string baseType = typeValueSeparated[2];

                            if (hiIndex > loIndex)
                            {
                                for (int i = loIndex; i <= hiIndex; i++)
                                {
                                    PdoEntryStructMemberViewModel member = new PdoEntryStructMemberViewModel();
                                    member.Attributes = pdoEntryAttributes;
                                    member.Name = pdoEntryName + "_" + baseType + "_" + i.ToString();
                                    member.BoxOrderCode = pdoEntryUnstructured.BoxOrderCode;
                                    member.Type_Value = baseType;
                                    if (pdoEntryUnstructured.InOut == "1")
                                    {
                                        member.InOutPlcProj = "AT %Q*";
                                        member.InOutMappings = "Outputs";
                                        pdoViewModel.InOutPlcProj = "AT %Q*";
                                        pdoViewModel.InOutMappings = "Outputs";
                                    }
                                    else
                                    {
                                    }
                                    member.OwnerBname = pdoEntryUnstructured.OwnerBname;
                                    member.SizeInBites = PlcBaseTypes.GetSizeInBites(baseType);
                                    member.SizeInBytes = PlcBaseTypes.GetSizeInBytes(baseType);
                                    member.Index = pdoEntryUnstructured.Index;
                                    member.IndexNumber = pdoEntryUnstructured.IndexNumber;
                                    member.SubIndex = pdoEntryUnstructured.SubIndex;
                                    member.SubIndexNumber = pdoEntryUnstructured.SubIndexNumber;
                                    actPdoEntryStruct.StructMembers.Add(member);
                                    if (!PdoEntryIsHidden(pdoEntryAttributes))
                                    {
                                        actPdoEntryStruct.Id = actPdoEntryStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                                    }
                                    actPdoEntryStruct.SizeInBites = actPdoEntryStruct.SizeInBites + member.SizeInBites;
                                    actPdoEntryStruct.SizeInBytes = actPdoEntryStruct.SizeInBytes + member.SizeInBytes;
                                }
                            }
                            else
                            {
                                PdoEntryStructMemberViewModel member = new PdoEntryStructMemberViewModel();
                                member.Attributes = pdoEntryAttributes;
                                member.Name = ValidatePlcItem.Name(pdoEntryName);
                                member.BoxOrderCode = pdoEntryUnstructured.BoxOrderCode;
                                member.Type_Value = baseType;
                                if (pdoEntryUnstructured.InOut == "1")
                                {
                                    member.InOutPlcProj = "AT %Q*";
                                    member.InOutMappings = "Outputs";
                                    pdoViewModel.InOutPlcProj = "AT %Q*";
                                    pdoViewModel.InOutMappings = "Outputs";
                                }
                                else
                                {
                                    member.InOutPlcProj = "AT %I*";
                                    member.InOutMappings = "Inputs";
                                    pdoViewModel.InOutPlcProj = "AT %I*";
                                    pdoViewModel.InOutMappings = "Inputs";
                                }
                                member.OwnerBname = pdoEntryUnstructured.OwnerBname;
                                member.SizeInBites = PlcBaseTypes.GetSizeInBites(baseType);
                                member.SizeInBytes = PlcBaseTypes.GetSizeInBytes(baseType);
                                member.Index = pdoEntryUnstructured.Index;
                                member.IndexNumber = pdoEntryUnstructured.IndexNumber;
                                member.SubIndex = pdoEntryUnstructured.SubIndex;
                                member.SubIndexNumber = pdoEntryUnstructured.SubIndexNumber;
                                actPdoEntryStruct.StructMembers.Add(member);
                                if (!PdoEntryIsHidden(pdoEntryAttributes))
                                {
                                    actPdoEntryStruct.Id = actPdoEntryStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                                }
                                actPdoEntryStruct.SizeInBites = actPdoEntryStruct.SizeInBites + member.SizeInBites;
                                actPdoEntryStruct.SizeInBytes = actPdoEntryStruct.SizeInBytes + member.SizeInBytes;
                            }
                        }
                        else
                        {
                            PdoEntryStructMemberViewModel member = new PdoEntryStructMemberViewModel();
                            member.Attributes = pdoEntryAttributes;
                            member.Name = pdoEntryName;
                            member.BoxOrderCode = pdoEntryUnstructured.BoxOrderCode;
                            member.Type_Value = pdoEntryUnstructured.Type_Value;
                            if (pdoEntryUnstructured.InOut == "1")
                            {
                                member.InOutPlcProj = "AT %Q*";
                                member.InOutMappings = "Outputs";
                                pdoViewModel.InOutPlcProj = "AT %Q*";
                                pdoViewModel.InOutMappings = "Outputs";
                            }
                            else
                            {
                                member.InOutPlcProj = "AT %I*";
                                member.InOutMappings = "Inputs";
                                pdoViewModel.InOutPlcProj = "AT %I*";
                                pdoViewModel.InOutMappings = "Inputs";
                            }
                            member.OwnerBname = pdoEntryUnstructured.OwnerBname;
                            member.SizeInBites = PlcBaseTypes.GetSizeInBites(pdoEntryUnstructured.Type_Value);
                            member.SizeInBytes = PlcBaseTypes.GetSizeInBytes(pdoEntryUnstructured.Type_Value);
                            member.Index = pdoEntryUnstructured.Index;
                            member.IndexNumber = pdoEntryUnstructured.IndexNumber;
                            member.SubIndex = pdoEntryUnstructured.SubIndex;
                            member.SubIndexNumber = pdoEntryUnstructured.SubIndexNumber;
                            actPdoEntryStruct.StructMembers.Add(member);
                            if (!PdoEntryIsHidden(pdoEntryAttributes))
                            {
                                actPdoEntryStruct.Id = actPdoEntryStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                            }
                            actPdoEntryStruct.SizeInBites = actPdoEntryStruct.SizeInBites + member.SizeInBites;
                            actPdoEntryStruct.SizeInBytes = actPdoEntryStruct.SizeInBytes + member.SizeInBytes;
                        }
                        if (pdoEntryUnstructured.Name != null && pdoEntryUnstructured.Name.Contains("__"))
                        {
                            pdoEntryUnstructuredPrefix = pdoEntryUnstructured.Name.Split('_')[0];
                        }

                        if (nextPdoEntryUnstructured.Name != null && nextPdoEntryUnstructured.Name.Contains("__"))
                        {
                            nextPdoEntryUnstructuredPrefix = nextPdoEntryUnstructured.Name.Split('_')[0];
                        }
                        bool isLastEntryOfTheCompletePDO = podEntryIndex == pdoEntriesUnstructuredCount - 1; //this pdo entry is the last member of the complete pdo object
                        bool nextEntryIsNotStructured = nextPdoEntryUnstructured.Name != null && !nextPdoEntryUnstructured.Name.Contains("__");//this pdo entry is the last member of this structure, as the next member of the pdo object is not structured
                        bool nextEntryIsMemberOfTheDifferentStructure = pdoEntryUnstructuredPrefix != null && nextPdoEntryUnstructuredPrefix != null && pdoEntryUnstructuredPrefix != nextPdoEntryUnstructuredPrefix;//this pdo entry is the last member of this structure, as the next member of the pdo object is the member of the new structure
                        if (isLastEntryOfTheCompletePDO || nextEntryIsNotStructured || nextEntryIsMemberOfTheDifferentStructure)
                        {
                            ValidatePdoEntryStructMemberNamesUniqueness(ref actPdoEntryStruct);

                            bool isArray = ReplacePdoEntryStructureByArrayIfPossible(ref actPdoEntryStruct);
                            if (isArray)
                            {
                                PdoEntryStructMemberViewModel firstStructMember = actPdoEntryStruct.StructMembers.FirstOrDefault();
                                PdoEntryViewModel pdoEntryArray = new PdoEntryViewModel() { Name = ValidatePlcItem.Name(actPdoEntryStruct.Prefix), Type_Value = firstStructMember.Type_Value, OwnerBname = firstStructMember.OwnerBname, InOut = pdoEntryUnstructured.InOut, VarB = actPdoEntryStruct.PdoEntryVarB, VarA = actPdoEntryStruct.PdoEntryVarA, BoxOrderCode = pdoViewModel.BoxOrderCode, Index = firstStructMember.Index, IndexNumber = firstStructMember.IndexNumber, SubIndex = firstStructMember.SubIndex, SubIndexNumber = firstStructMember.SubIndexNumber, SizeInBites = actPdoEntryStruct.SizeInBites, SizeInBytes = actPdoEntryStruct.SizeInBytes };
                                pdoEntryArray.SizeInBites = actPdoEntryStruct.SizeInBites;
                                pdoEntryArray.SizeInBytes = actPdoEntryStruct.SizeInBytes;
                                pdoEntriesStructured.Add(pdoEntryArray);
                                //delete actPdoEntryStruct
                                actPdoEntryStruct = null;
                                structOpen = false;
                            }
                            else
                            {
                                //Calculate CRC of the actPdoEntryStruct.Id
                                actPdoEntryStruct.Crc32 = CRC32.Calculate_CRC32(actPdoEntryStruct.Id);
                                actPdoEntryStruct.Name = ValidatePlcItem.StructurePrefix(actPdoEntryStruct.Prefix) + "_" + actPdoEntryStruct.Crc32.ToString("X8");
                                //Check if such an structure exists
                                if (CheckIfPdoEntryStructureDoesNotExist(actPdoEntryStruct))
                                {
                                    //if not add to the structure list
                                    PdoEntryStructures.Add(actPdoEntryStruct);
                                }
                                //create pdo entry of the structured type
                                PdoEntryStructMemberViewModel firstStructMember = actPdoEntryStruct.StructMembers.FirstOrDefault();
                                PdoEntryViewModel pdoEntryStructured = new PdoEntryViewModel() { Name = ValidatePlcItem.Name(actPdoEntryStruct.Prefix), Type_Value = actPdoEntryStruct.Name, OwnerBname = pdoEntryUnstructured.OwnerBname, InOut = pdoEntryUnstructured.InOut, VarB = actPdoEntryStruct.PdoEntryVarB, VarA = actPdoEntryStruct.PdoEntryVarA, BoxOrderCode = pdoViewModel.BoxOrderCode, Index = firstStructMember.Index, IndexNumber = firstStructMember.IndexNumber, SubIndex = firstStructMember.SubIndex, SubIndexNumber = firstStructMember.SubIndexNumber, SizeInBites = actPdoEntryStruct.SizeInBites, SizeInBytes = actPdoEntryStruct.SizeInBytes };
                                //add to the return value list
                                pdoEntryStructured.SizeInBites = actPdoEntryStruct.SizeInBites;
                                pdoEntryStructured.SizeInBytes = actPdoEntryStruct.SizeInBytes;
                                pdoEntriesStructured.Add(pdoEntryStructured);
                                //delete actPdoEntryStruct
                                actPdoEntryStruct = null;
                                structOpen = false;
                            }
                        }
                    }

                    else
                    {
                        //if (string.IsNullOrEmpty(pdoEntryUnstructured.Name) && pdoEntryUnstructured.Type_GUID != null)
                        //{
                        //    pdoEntryUnstructured.Name = "reserve_" + reserveIndex.ToString();
                        //    reserveIndex++;
                        //}
                        if (pdoEntryUnstructured.Name != null)
                        {
                            pdoEntryUnstructured.Name = ValidatePlcItem.Name(pdoEntryUnstructured.Name);
                            pdoEntryUnstructured.Type_Value = ValidatePlcItem.Type(pdoEntryUnstructured.Type_Value);
                            //if (pdoEntryUnstructured.Index != null || (pdoEntryUnstructured.Name.Contains("reserve_") && pdoEntryUnstructured.Type_GUID != null))
                            if (pdoEntryUnstructured.Index != null )
                            {
                                pdoEntryUnstructured.SizeInBites = PlcBaseTypes.GetSizeInBites(pdoEntryUnstructured.Type_Value);
                                pdoEntryUnstructured.SizeInBytes = PlcBaseTypes.GetSizeInBytes(pdoEntryUnstructured.Type_Value);
                                pdoEntriesStructured.Add(pdoEntryUnstructured);
                            }
                        }
                    }
                }
            }
            else
            {
                pdoEntriesStructured = null;
            }
            if (pdoEntriesStructured != null && pdoEntriesStructured.Count == 0)
            {
                pdoEntriesStructured = null;
            }
            return pdoEntriesStructured;
        }
    }
}
