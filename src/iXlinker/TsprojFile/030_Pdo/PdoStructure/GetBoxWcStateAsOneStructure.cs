using iXlinkerDtos;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using System.Collections.Generic;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoViewModel GetBoxWcStateAsOneStructure(BoxViewModel boxViewModel)
        {
            ObservableCollection<PdoEntryViewModel> WcStateEntriesStructured = new ObservableCollection<PdoEntryViewModel>();
            ObservableCollection<PdoEntryViewModel> WcStateEntriesUnstructured = new ObservableCollection<PdoEntryViewModel>();
            PdoViewModel WcState = new PdoViewModel();

            if (boxViewModel.WcStateWcState && !boxViewModel.SyncUnitDefinedOnAtLeastOnePdo && boxViewModel.TotalNumberOfPdos > 0)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "WcState", VarB = "WcState" + tmpLevelSeparator + "WcState", VarA = "WcState" + tmpLevelSeparator + "WcState", Type_Value = "BOOL", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                WcStateEntriesStructured.Add(pdoEntryViewModel);
                WcStateEntriesUnstructured.Add(pdoEntryViewModel);
            }
            if (boxViewModel.WcStateWcState && boxViewModel.SyncUnitDefinedOnAtLeastOnePdo)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "WcState0", VarB = "WcState" + tmpLevelSeparator + "WcState0", VarA = "WcState" + tmpLevelSeparator + "WcState0", Type_Value = "BOOL", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                WcStateEntriesStructured.Add(pdoEntryViewModel);
                WcStateEntriesUnstructured.Add(pdoEntryViewModel);

                pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "WcState1", VarB = "WcState" + tmpLevelSeparator + "WcState1", VarA = "WcState" + tmpLevelSeparator + "WcState1", Type_Value = "BOOL", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                WcStateEntriesStructured.Add(pdoEntryViewModel);
                WcStateEntriesUnstructured.Add(pdoEntryViewModel);
            }
            if (boxViewModel.WcStateInputToggle && !boxViewModel.SyncUnitDefinedOnAtLeastOnePdo)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "InputToggle", VarB = "WcState" + tmpLevelSeparator + "InputToggle", VarA = "WcState" + tmpLevelSeparator + "InputToggle", Type_Value = "BOOL", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                WcStateEntriesStructured.Add(pdoEntryViewModel);
                WcStateEntriesUnstructured.Add(pdoEntryViewModel);
            }
            if (boxViewModel.WcStateInputToggle && boxViewModel.SyncUnitDefinedOnAtLeastOnePdo)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "InputToggle0", VarB = "WcState" + tmpLevelSeparator + "InputToggle0", VarA = "WcState" + tmpLevelSeparator + "InputToggle0", Type_Value = "BOOL", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                WcStateEntriesStructured.Add(pdoEntryViewModel);
                WcStateEntriesUnstructured.Add(pdoEntryViewModel);

                pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "InputToggle1", VarB = "WcState" + tmpLevelSeparator + "InputToggle1", VarA = "WcState" + tmpLevelSeparator + "InputToggle1", Type_Value = "BOOL", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                WcStateEntriesStructured.Add(pdoEntryViewModel);
                WcStateEntriesUnstructured.Add(pdoEntryViewModel);
            }

            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "WcState", Id = "", BoxOrderCode = boxViewModel.BoxOrderCode };
            MappableObject mapableObject = new MappableObject();

            foreach (PdoEntryViewModel pdoEntry in WcStateEntriesStructured)
            {
                WcState.PdoEntriesStructured.Add(pdoEntry);

                PdoStructMemberViewModel member = new PdoStructMemberViewModel();
                string pdoEntryName = ValidatePlcItem.Name(pdoEntry.Name);
                member.Attributes = new List<string> { "{attribute addProperty Name \"" + pdoEntryName + "\"}" };
                member.Name = pdoEntryName;
                member.BoxOrderCode = pdoEntry.BoxOrderCode;
                member.Type_Value = pdoEntry.Type_Value;
                member.TypeNamespace = pdoEntry.TypeNamespace;
                if (pdoEntry.InOut == "1")
                {
                    member.InOutPlcProj = "AT %Q*";
                    member.InOutMappings = "Outputs";
                }
                else
                {
                    member.InOutPlcProj = "AT %I*";
                    member.InOutMappings = "Inputs";
                }
                member.OwnerBname = pdoEntry.OwnerBname;
                member.Size = pdoEntry.Size;
                member.Index = pdoEntry.Index;
                member.IndexNumber = pdoEntry.IndexNumber;
                member.SubIndex = pdoEntry.SubIndex;
                member.SubIndexNumber = pdoEntry.SubIndexNumber;
                actPdoStruct.AddMemberAndUpdateIdAndSize(member);
                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                mapableObject.MapableItems.Add(mapableItem);
            }

            foreach (PdoEntryViewModel pdoEntry in WcStateEntriesUnstructured)
            {
                WcState.PdoEntriesUnstructured.Add(pdoEntry);
            }

            ValidatePdoStructMemberNamesUniqueness(ref actPdoStruct);

            if (actPdoStruct.StructMembers.Count > 0)
            {
                //Calculate CRC of the actPdoStruct.Id
                actPdoStruct.Crc32 = CRC32.Calculate_CRC32(actPdoStruct.Id);
                actPdoStruct.Name = ValidatePlcItem.Name(actPdoStruct.Prefix + "_" + actPdoStruct.Crc32.ToString("X8"));
                //Check if such an structure exists
                if (CheckIfPdoStructureDoesNotExist(actPdoStruct))
                {
                    //if not add to the structure list
                    AddExtensionFromBasePdoStructure(actPdoStruct);
                    PdoStructures.Add(actPdoStruct);
                }
                PdoStructMemberViewModel firstStructMember = actPdoStruct.StructMembers.FirstOrDefault();
                WcState.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                WcState.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                WcState.TypeNamespace = actPdoStruct.TypeNamespace;
                WcState.OwnerBname = firstStructMember.OwnerBname;
                WcState.InOutPlcProj = firstStructMember.InOutPlcProj;
                WcState.InOutMappings = firstStructMember.InOutMappings;
                WcState.BoxOrderCode = firstStructMember.BoxOrderCode;
                WcState.Size = actPdoStruct.Size;

                mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
                mapableObject.Size = actPdoStruct.Size;

                WcState.MapableObject = mapableObject;
            }
            else
            {
                WcState = null;
            }
            return WcState;
        }
    }
}
