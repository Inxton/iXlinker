using iXlinkerDtos;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using PlcprojFile;
using System.Collections.Generic;
using System;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoViewModel> GetAllPdosAsOneStructuresOfSlotTypes(BoxViewModel boxViewModel, ObservableCollection<PdoViewModel> pdos)
        {
            List<PdoStructViewModel> slotStructs = new List<PdoStructViewModel>();
            List<PdoViewModel> pdoViewModels = new List<PdoViewModel>();
            List<MappableObject> mapableObjects = new List<MappableObject>();

            foreach (PdoViewModel pdo in pdos)
            {
                if (pdo.Name.Contains(ioSlotSeparator))
                {
                    string slotName = ValidatePlcItem.Name(pdo.Name.Substring(0, pdo.Name.IndexOf(ioSlotSeparator, StringComparison.Ordinal)));
                    
                    //Check if slot structure already exists
                    bool slotStructAlreadyExists = false;
                    foreach (PdoStructViewModel slotStruct in slotStructs)
                    {
                        if (slotStruct.Prefix.Equals(ValidatePlcItem.StructurePrefix(slotName)))
                        {
                            slotStructAlreadyExists = true;
                            break;
                        }
                    }
                    
                    //if not, create it
                    if (!slotStructAlreadyExists)
                    {
                        PdoStructViewModel actSlotStruct = new PdoStructViewModel() { Prefix = ValidatePlcItem.Name(slotName), Id = "", BoxOrderCode = boxViewModel.BoxOrderCode };
                        slotStructs.Add(actSlotStruct);
                        PdoViewModel pdoViewModel = new PdoViewModel() { Name = slotName };
                        pdoViewModels.Add(pdoViewModel);
                        MappableObject mapableObject = new MappableObject() { Name = slotName };
                        mapableObjects.Add(mapableObject);
                    }
                }
            }

            foreach (PdoViewModel pdo in pdos)
            {
                if (pdo.Name.Contains(ioSlotSeparator))
                {
                    //Create the struct member
                    PdoStructMemberViewModel member = new PdoStructMemberViewModel();
                    string slotName = ValidatePlcItem.Name(pdo.Name.Substring(0, pdo.Name.IndexOf(ioSlotSeparator, StringComparison.Ordinal)));
                    string pdoName = ValidatePlcItem.Name(pdo.Name.Substring(pdo.Name.LastIndexOf(ioSlotSeparator, StringComparison.Ordinal) + 1));
                    string typeValue = pdo.Type_Value;
                    if (typeValue.Contains(ioSlotSeparator))
                    {
                        typeValue = typeValue.Substring(typeValue.LastIndexOf(ioSlotSeparator, StringComparison.Ordinal) + 1);
                    }
                    member.Attributes.Add("{attribute addProperty Name \"" + pdoName + "\"}");
                    member.Name = pdoName;
                    member.BoxOrderCode = pdo.BoxOrderCode;
                    member.Type_Value = typeValue;
                    member.TypeNamespace = pdo.TypeNamespace;
                    //member.InOutPlcProj = pdo.InOutPlcProj;
                    //member.InOutMappings = pdo.InOutMappings;
                    member.OwnerBname = pdo.OwnerBname;
                    member.SizeInBites = pdo.SizeInBites;
                    member.SizeInBytes = pdo.SizeInBytes;
                    member.Index = pdo.Index;
                    member.IndexNumber = pdo.IndexNumber;

                    PdoStructViewModel actStruct = new PdoStructViewModel() { Prefix = ValidatePlcItem.Name(slotName) };
                    int slotIndex = slotStructs.FindIndex(info => info.Prefix == actStruct.Prefix);
                    slotStructs[slotIndex].StructMembers.Add(member);
                    slotStructs[slotIndex].Id = slotStructs[slotIndex].Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes;
                    slotStructs[slotIndex].SizeInBites = slotStructs[slotIndex].SizeInBites + member.SizeInBites;
                    slotStructs[slotIndex].SizeInBytes = slotStructs[slotIndex].SizeInBytes + member.SizeInBytes;

                    foreach (PdoEntryViewModel pdoEntry in pdo.PdoEntriesUnstructured)
                    {
                        pdoViewModels[slotIndex].PdoEntriesUnstructured.Add(pdoEntry);
                    }
                    foreach (PdoEntryViewModel pdoEntry in pdo.PdoEntriesStructured)
                    {
                        pdoViewModels[slotIndex].PdoEntriesStructured.Add(pdoEntry);
                    }

                    foreach (MappableItem mapableItem in pdo.MapableObject.MapableItems)
                    {
                        mapableObjects[slotIndex].MapableItems.Add(new MappableItem() { VarAprefix = mapableItem.VarAprefix, OwnerBname = mapableItem.OwnerBname, VarA = mapableItem.VarA, VarB = mapableItem.VarB});
                    }
                }
                else
                {
                    pdoViewModels.Add(pdo);
                }
            }

            for(int i = 0; i< slotStructs.Count; i++)
            {
                PdoStructViewModel actSlotStruct = slotStructs[i];
                ValidatePdoStructMemberNamesUniqueness(ref actSlotStruct);
                if (actSlotStruct.StructMembers.Count > 0)
                {

                    //Calculate CRC of the actPdoStruct.Id
                    actSlotStruct.Crc32 = CRC32.Calculate_CRC32(actSlotStruct.Id);
                    actSlotStruct.Name = ValidatePlcItem.Name(actSlotStruct.Prefix + "_" + actSlotStruct.Crc32.ToString("X8"));
                    //Check if such an structure exists
                    if (CheckIfPdoStructureDoesNotExist(actSlotStruct))
                    {
                        //if not add to the structure list
                        //actSlotStruct.TypeNamespace = "*";
                        PdoStructures.Add(actSlotStruct);
                    }
                    PdoStructMemberViewModel firstStructMember = actSlotStruct.StructMembers.FirstOrDefault();
                    pdoViewModels[i].Name = ValidatePlcItem.Name(actSlotStruct.Prefix);
                    pdoViewModels[i].Type_Value = actSlotStruct.Name;
                    pdoViewModels[i].TypeNamespace = actSlotStruct.TypeNamespace;
                    pdoViewModels[i].OwnerBname = firstStructMember.OwnerBname;
                    pdoViewModels[i].InOutPlcProj = firstStructMember.InOutPlcProj;
                    pdoViewModels[i].InOutMappings = firstStructMember.InOutMappings;
                    pdoViewModels[i].BoxOrderCode = firstStructMember.BoxOrderCode;
                    pdoViewModels[i].SizeInBites = actSlotStruct.SizeInBites;
                    pdoViewModels[i].SizeInBytes = actSlotStruct.SizeInBytes;

                    mapableObjects[i].Name = ValidatePlcItem.Name(boxViewModel.Name);
                    //mapableObjects[i].Type_Value = ValidatePlcItem.NameIncludingNamespace(actSlotStruct.TypeNamespace, ValidatePlcItem.Type(actSlotStruct.Name));
                    mapableObjects[i].Type_Value =ValidatePlcItem.Type(actSlotStruct.Name);
                    mapableObjects[i].TypeNamespace =actSlotStruct.TypeNamespace;

                    mapableObjects[i].SizeInBites = actSlotStruct.SizeInBites;
                    mapableObjects[i].SizeInBytes = actSlotStruct.SizeInBytes;

                    pdoViewModels[i].MapableObject = mapableObjects[i];
                }
                else
                {
                    pdoViewModels[i] = null;
                }

            }
            return new ObservableCollection<PdoViewModel>(pdoViewModels);
        }
    }
}
