using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;
using Utils;
using System.Collections.Generic;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoEntryViewModel> GetAllVarGrpVarsStructured(TcSmVarGrpDef varGrp, PdoViewModel pdoViewModel, ObservableCollection<PdoEntryViewModel> varsUnstructured)
        {
            ObservableCollection<PdoEntryViewModel> varsStructured = new ObservableCollection<PdoEntryViewModel>();
            uint reserveIndex = 1;

            PdoEntryStructViewModel actVarStruct = new PdoEntryStructViewModel(); 

            int varsUnstructuredCount = varsUnstructured.Count;

            for (int varIndex=0; varIndex < varsUnstructuredCount; varIndex++)
            {
                PdoEntryViewModel varUnstructured  = varsUnstructured[varIndex];
                PdoEntryViewModel nextVarUnstructured = new PdoEntryViewModel();
                string varUnstructuredPrefix = null;
                string nextVarUnstructuredPrefix = null;
 
                bool structOpen = false;

                if (varIndex < varsUnstructuredCount - 1) 
                {
                    nextVarUnstructured = varsUnstructured[varIndex + 1];
                    if (nextVarUnstructured.Name == null && actVarStruct != null && actVarStruct.Prefix != null)
                    {
                        nextVarUnstructured.Name = actVarStruct.Prefix + "__";
                    }
                }

                if (varUnstructured.Name == null && actVarStruct !=null && actVarStruct.Prefix !=null)
                {
                    varUnstructured.Name = actVarStruct.Prefix + "__";
                }
                //Check if some vars is structured
                if (varUnstructured.Name !=null && varUnstructured.Name.Contains("__"))
                {
                    if (actVarStruct == null || actVarStruct.Prefix == null && actVarStruct.Id == null)   //First member of the structure
                    {
                        actVarStruct = new PdoEntryStructViewModel() { Prefix = ValidatePlcItem.StructurePrefix(varUnstructured.Name.Substring(0, varUnstructured.Name.IndexOf("__", StringComparison.Ordinal))) , Id = "", BoxOrderCode = varUnstructured.BoxOrderCode };
                        structOpen = true;
                    }
                    if (structOpen && actVarStruct.PdoEntryVarA == null && varUnstructured.VarA != null)
                        actVarStruct.PdoEntryVarA = varUnstructured.VarB.Substring(0, varUnstructured.VarB.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));
                    if (structOpen && actVarStruct.PdoEntryVarB == null && varUnstructured.VarB != null)
                        actVarStruct.PdoEntryVarB = varUnstructured.VarB.Substring(0, varUnstructured.VarB.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));

                    //Get pdo entry name, if empty bites used, generate unique name with attribute hide
                    string pdoEntryName = ValidatePlcItem.Name(varUnstructured.Name.Substring(actVarStruct.Prefix.Length+2));
                    //string pdoEntryAttribute = "{attribute addProperty Name \"" + pdoEntryName + "\"}";
                    List<string> pdoEntryAttributes = new List<string>();

                    if (pdoEntryName == "")
                    {
                        pdoEntryName = "reserve_" + reserveIndex.ToString();
                        pdoEntryAttributes.Add(attributeHide);
                        reserveIndex++;
                    }
                    else
                    {
                        pdoEntryAttributes.Add("{attribute addProperty Name \"" + pdoEntryName + "\"}");
                    }

                    string pdoEntryType = varUnstructured.Type_Value;

                    //"Replacement" BITx by ARRAY [0..x-1] of BIT
                    if (!pdoEntryType.Contains("ARRAY") && pdoEntryType.Contains("BIT") && pdoEntryType.Length>3)
                    {
                        uint hiIndex = Convert.ToUInt32(pdoEntryType.Substring(3, pdoEntryType.Length-3))- 1;
                        pdoEntryType = "ARRAY [0.." + hiIndex.ToString() +"] OF BIT" ;
                    }
                    //For example terminal EL5101-0011 ProductCode="#x13ed3052" RevisionNo="#x0010000b" contains TxPdo "ENC TxPdo 1 Samples Counter value" with index "x1a10" with <Name>Counter value__ARRAY [0]</Name> and < DataType > UDINT </ DataType >
                    //In such a case "Counter_value" will be the structure instance name and "Counter_value_0" will be the member of this structure
                    if (pdoEntryName.ToUpper().Contains("ARRAY"))
                    {
                        string s = pdoEntryName.ToUpper().Replace("ARRAY", "").Replace("[", "").Replace("]", "").Replace(".", "").Replace(" ","");
                        pdoEntryName = actVarStruct.Prefix + "_" + s;
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
                                member.Type_Value = baseType;
                                member.TypeNamespace = varUnstructured.TypeNamespace;
                                member.Size = PlcBaseTypes.GetSize(baseType);
                                member.SubIndexNumber = varUnstructured.SubIndexNumber;
                                actVarStruct.StructMembers.Add(member);
                                actVarStruct.Id = actVarStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
                                actVarStruct.Size = actVarStruct.Size + member.Size;
                            }
                        }
                        else
                        {
                            PdoEntryStructMemberViewModel member = new PdoEntryStructMemberViewModel();
                            member.Attributes = pdoEntryAttributes;
                            member.Name = ValidatePlcItem.Name( pdoEntryName) ;
                            member.Type_Value = baseType;
                            member.Size = PlcBaseTypes.GetSize(baseType);
                            member.SubIndexNumber = varUnstructured.SubIndexNumber;
                            actVarStruct.StructMembers.Add(member);
                            actVarStruct.Id = actVarStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
                            actVarStruct.Size = actVarStruct.Size + member.Size;
                        }
                    }
                    else
                    {
                        PdoEntryStructMemberViewModel member = new PdoEntryStructMemberViewModel();
                        member.Attributes = pdoEntryAttributes;
                        member.Name = pdoEntryName;
                        member.Type_Value = varUnstructured.Type_Value;
                        member.Size = PlcBaseTypes.GetSize(varUnstructured.Type_Value);
                        member.SubIndexNumber = varUnstructured.SubIndexNumber;
                        actVarStruct.StructMembers.Add(member);
                        actVarStruct.Id = actVarStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
                        actVarStruct.Size = actVarStruct.Size + member.Size;
                    }
                    if(varUnstructured.Name != null && varUnstructured.Name.Contains("__"))
                    {  
                       varUnstructuredPrefix = varUnstructured.Name.Split('_')[0];
                    }

                    if (nextVarUnstructured.Name != null && nextVarUnstructured.Name.Contains("__"))
                    {
                        nextVarUnstructuredPrefix = nextVarUnstructured.Name.Split('_')[0];
                    }
                    bool isLastEntryOfTheCompletePDO = varIndex == varsUnstructuredCount - 1; //this pdo entry is the last member of the complete pdo object
                    bool nextEntryIsNotStructured = nextVarUnstructured.Name != null && !nextVarUnstructured.Name.Contains("__");//this pdo entry is the last member of this structure, as the next member of the pdo object is not structured
                    bool nextEntryIsMemberOfTheDifferentStructure = varUnstructuredPrefix != null && nextVarUnstructuredPrefix != null && varUnstructuredPrefix != nextVarUnstructuredPrefix;//this pdo entry is the last member of this structure, as the next member of the pdo object is the member of the new structure
                    if (isLastEntryOfTheCompletePDO || nextEntryIsNotStructured || nextEntryIsMemberOfTheDifferentStructure)
                    {
                        ValidateVarGrpStructMemberNamesUniqueness(ref actVarStruct);
                        //Calculate CRC of the actPdoEntryStruct.Id
                        actVarStruct.Crc32 = CRC32.Calculate_CRC32(actVarStruct.Id);
                        actVarStruct.Name = ValidatePlcItem.Name(actVarStruct.Prefix + "_" + actVarStruct.Crc32.ToString("X8"));
                        //Check if such an structure exists
                        //if not add to the structure list
                        PdoEntryStructures.Add(actVarStruct);
                        //create pdo entry of the structured type
                        PdoEntryViewModel pdoEntryStructured = new PdoEntryViewModel() { Name = ValidatePlcItem.Name(actVarStruct.Prefix), Type_Value = actVarStruct.Name, TypeNamespace = actVarStruct.TypeNamespace, OwnerBname = varUnstructured.OwnerBname, InOut = varUnstructured.InOut, VarB = actVarStruct.PdoEntryVarB, VarA = actVarStruct.PdoEntryVarA, BoxOrderCode = pdoViewModel.BoxOrderCode };
                        //add to the return value list
                        varsStructured.Add(pdoEntryStructured);
                        //delete actPdoEntryStruct
                        actVarStruct = null;
                        structOpen = false;
                    }
                }
                
                else
                {
                    if (varUnstructured.Name != null)
                    {
                        varUnstructured.Name = ValidatePlcItem.Name(varUnstructured.Name);
                        varsStructured.Add(varUnstructured);
                    }
                }
            }

            return varsStructured;
        }
    }
}
