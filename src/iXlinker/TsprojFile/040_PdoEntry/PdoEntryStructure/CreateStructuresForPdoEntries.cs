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
        private ObservableCollection<PdoEntryViewModel> CreateStructuresForPdoEntries(EtherCATSlavePdo pdo, PdoViewModel pdoViewModel, ObservableCollection<PdoEntryViewModel> pdoEntriesUnstructured)
        {
            ObservableCollection<PdoEntryViewModel> pdoEntriesStructured = new ObservableCollection<PdoEntryViewModel>();
            if (pdo.Entry != null && pdoEntriesUnstructured != null && pdoEntriesUnstructured.Count > 0)
            {
                PdoEntryStructViewModel actPdoEntryStruct = new PdoEntryStructViewModel();
                bool structOpen = false;

                int pdoEntriesUnstructuredCount = pdoEntriesUnstructured.Count;

                for (int pdoEntryItemIndex = 0; pdoEntryItemIndex < pdoEntriesUnstructuredCount; pdoEntryItemIndex++)
                {
                    PdoEntryViewModel pdoEntryUnstructured = pdoEntriesUnstructured[pdoEntryItemIndex];
                    bool isStructured = pdoEntryUnstructured.Name != null && pdoEntryUnstructured.Name.Contains("__");
                    string structurePrefix = isStructured ? pdoEntryUnstructured.Name.Split("__")[0] : null;
                    if (isStructured)
                    {
                        PdoEntryViewModel nextPdoEntryUnstructured = (pdoEntryItemIndex < pdoEntriesUnstructuredCount - 1) ? pdoEntriesUnstructured[pdoEntryItemIndex + 1] : new PdoEntryViewModel();

                        if (!structOpen)   //First member of the structure
                        {

                            if (structurePrefix.Contains('_') || structurePrefix.Contains(' '))
                            {
                                string _tmpStructurePrefix = structurePrefix.Replace(' ', '_');
                                string _structurePrefix = _tmpStructurePrefix.Substring(0, _tmpStructurePrefix.LastIndexOf('_'));
                                string _endOfStructurePrefix = _tmpStructurePrefix.Substring(_tmpStructurePrefix.LastIndexOf('_') + 1);
                                if (Int16.TryParse(_endOfStructurePrefix, out short _index))
                                {
                                    structurePrefix = _structurePrefix;
                                }
                            }

                            actPdoEntryStruct = new PdoEntryStructViewModel() { Prefix = ValidatePlcItem.Name(structurePrefix), Id = "", BoxOrderCode = pdoEntryUnstructured.BoxOrderCode };

                            structOpen = true;
                        }
                        if (structOpen && actPdoEntryStruct.PdoEntryVarA == null && pdoEntryUnstructured.VarA != null)
                            actPdoEntryStruct.PdoEntryVarA = ValidatePlcItem.Name(pdoEntryUnstructured.VarB.Substring(0, pdoEntryUnstructured.VarB.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal)));
                        if (structOpen && actPdoEntryStruct.PdoEntryVarB == null && pdoEntryUnstructured.VarB != null)
                            actPdoEntryStruct.PdoEntryVarB = pdoEntryUnstructured.VarB.Substring(0, pdoEntryUnstructured.VarB.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));

                        string pdoEntryName = pdoEntryUnstructured.Name.Split("__")[0];
                        string pdoEntryItemName = pdoEntryUnstructured.Name.Split("__")[1];

                        PdoEntryStructMemberViewModel member = new PdoEntryStructMemberViewModel();
                        member.Name = pdoEntryItemName;
                        member.NameUnmodified = pdoEntryItemName;
                        member.BoxOrderCode = pdoEntryUnstructured.BoxOrderCode;
                        member.Type_Value = pdoEntryUnstructured.Type_Value;
                        member.TypeNamespace = pdoEntryUnstructured.TypeNamespace;
                        member.InOut = pdoEntryUnstructured.InOut;
                        if (member.InOut == "1")
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
                        member.Size = PlcBaseTypes.GetSize(pdoEntryUnstructured.Type_Value);
                        member.Index = pdoEntryUnstructured.Index;
                        member.IndexNumber = pdoEntryUnstructured.IndexNumber;
                        member.SubIndex = pdoEntryUnstructured.SubIndex;
                        member.SubIndexNumber = pdoEntryUnstructured.SubIndexNumber;

                        actPdoEntryStruct.StructMembers.Add(member);

                        //this pdo entry is the last member of the complete pdo object
                        bool isLastPdoEntryItemOfTheCompletePDO = pdoEntryItemIndex == pdoEntriesUnstructuredCount - 1;
                        //if the next member of the pdo object is not structured this pdo entry is the last member of this structure 
                        bool nextPdoEntryItemIsNotStructured = nextPdoEntryUnstructured.Name != null && !nextPdoEntryUnstructured.Name.Contains("__");
                        string nextPdoEntryName = (isLastPdoEntryItemOfTheCompletePDO || nextPdoEntryItemIsNotStructured) ? null : nextPdoEntryUnstructured.Name.Split("__")[0];
                        //if the next member of the pdo object is the member of the new structure this pdo entry is the last member of this structure 
                        bool nextPdoEntryItemIsMemberOfTheDifferentStructure = pdoEntryName != null && nextPdoEntryName != null && pdoEntryName != nextPdoEntryName;
                        if (isLastPdoEntryItemOfTheCompletePDO || nextPdoEntryItemIsNotStructured || nextPdoEntryItemIsMemberOfTheDifferentStructure)
                        {
                            ValidatePdoEntryStructMemberNamesUniqueness(ref actPdoEntryStruct);

                            if (IsArrayOfPdoEntryStructureItem(actPdoEntryStruct, out int lowIndex, out int highIndex, out string typeValue, out string typeNamespace))
                            {
                                PdoEntryStructMemberViewModel firstStructMember = actPdoEntryStruct.StructMembers.FirstOrDefault();
                                PdoEntryViewModel pdoEntryArray = new PdoEntryViewModel() { Name = pdoEntryName, Type_Value = "ARRAY [0.." + (highIndex - lowIndex).ToString() + "] OF " + ValidatePlcItem.NameIncludingNamespace(typeNamespace, typeValue), OwnerBname = actPdoEntryStruct.StructMembers.FirstOrDefault().OwnerBname, InOut = pdoEntryUnstructured.InOut, VarB = actPdoEntryStruct.PdoEntryVarB, VarA = actPdoEntryStruct.PdoEntryVarA, BoxOrderCode = pdoViewModel.BoxOrderCode, Index = firstStructMember.Index, IndexNumber = firstStructMember.IndexNumber, SubIndex = firstStructMember.SubIndex, SubIndexNumber = firstStructMember.SubIndexNumber, Size = actPdoEntryStruct.Size };
                                pdoEntryArray = CreateMappingsForThePdoEntriesOfArrayType(pdoEntryArray, actPdoEntryStruct);
                                pdoEntriesStructured.Add(pdoEntryArray);
                                //delete actPdoEntryStruct
                                actPdoEntryStruct = null;
                                structOpen = false;
                            }
                            else
                            {
                                SavePdoEntryStructure(ref actPdoEntryStruct);
                                PdoEntryStructMemberViewModel firstStructMember = actPdoEntryStruct.StructMembers.FirstOrDefault();
                                PdoEntryViewModel pdoEntryStructured = new PdoEntryViewModel() { Name = pdoEntryName, TypeNamespace = actPdoEntryStruct.TypeNamespace, Type_Value = actPdoEntryStruct.Name, OwnerBname = pdoEntryUnstructured.OwnerBname, InOut = pdoEntryUnstructured.InOut, VarB = actPdoEntryStruct.PdoEntryVarB, VarA = actPdoEntryStruct.PdoEntryVarA, BoxOrderCode = pdoViewModel.BoxOrderCode, Index = firstStructMember.Index, IndexNumber = firstStructMember.IndexNumber, SubIndex = firstStructMember.SubIndex, SubIndexNumber = firstStructMember.SubIndexNumber, Size = actPdoEntryStruct.Size };
                                pdoEntryStructured.MapableObject = CreateMappingsForThePdoEntriesStructured(pdoEntryStructured, actPdoEntryStruct);
                                //add to the return value list
                                pdoEntryStructured.Size = actPdoEntryStruct.Size;
                                pdoEntriesStructured.Add(pdoEntryStructured);
                                //delete actPdoEntryStruct
                                actPdoEntryStruct = null;
                                structOpen = false;
                            }
                        }
                    }

                    else
                    {
                        if (pdoEntryUnstructured.Name != null && pdoEntryUnstructured.Index != null)
                        {
                            pdoEntryUnstructured.Name = ValidatePlcItem.Name(pdoEntryUnstructured.Name);
                            pdoEntryUnstructured.Type_Value = ValidatePlcItem.Type(pdoEntryUnstructured.Type_Value);
                            pdoEntryUnstructured.TypeNamespace = pdoEntryUnstructured.TypeNamespace;
                            pdoEntryUnstructured.Size = PlcBaseTypes.GetSize(pdoEntryUnstructured.Type_Value);
                            pdoEntryUnstructured.MapableObject = new MappableObject() 
                            {
                                Name = pdoViewModel.Name, 
                                Type_Value = pdoEntryUnstructured.Type_Value , 
                                TypeNamespace = pdoEntryUnstructured.TypeNamespace,
                                Size = pdoEntryUnstructured.Size,
                                MapableItems = new ObservableCollection<MappableItem>() 
                                { 
                                    new MappableItem() 
                                    { 
                                        VarAprefix = Context + (pdoEntryUnstructured.InOut == null ? " Inputs" : " Outputs") + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (pdoEntryUnstructured.OwnerBname).Replace("TIID" + tmpLevelSeparator, ""), 
                                        OwnerBname = pdoEntryUnstructured.OwnerBname, 
                                        VarA = pdoEntryUnstructured.VarA, 
                                        VarB = pdoEntryUnstructured.VarB 
                                    } 
                                } 
                            };

                            pdoEntriesStructured.Add(pdoEntryUnstructured);
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
