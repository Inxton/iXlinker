using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoViewModel GetDeviceSyncUnitWcStateAsOneStructure(TcSmProjectProjectIODevice device, DeviceViewModel deviceViewModel, string plcTaskA , string plcTaskB)
        {

            ObservableCollection<PdoEntryViewModel> WcStateEntries = new ObservableCollection<PdoEntryViewModel>();

            PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "WcState", VarB = plcTaskB + tmpLevelSeparator + "WcState" + tmpLevelSeparator + "WcState", VarA = plcTaskA + tmpLevelSeparator + "WcState" + tmpLevelSeparator + "WcState", Type_Value = "BOOL", InOut = "0"};
            pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);

            WcStateEntries.Add(pdoEntryViewModel);

            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "WcState", Id = "", BoxOrderCode = deviceViewModel.Type.ToString() };
            MappableObject mapableObject = new MappableObject();

            PdoViewModel WcState = new PdoViewModel(); ;

            foreach (PdoEntryViewModel pdoEntry in WcStateEntries)
            {
                WcState.PdoEntriesStructured.Add(pdoEntry);

                PdoStructMemberViewModel member = new PdoStructMemberViewModel();
                string pdoEntryName = ValidatePlcItem.Name(pdoEntry.Name);
                member.Attributes.Add("{attribute addProperty Name \"" + pdoEntryName + "\"}");
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
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB };
                mapableObject.MapableItems.Add(mapableItem);
            }

            if (WcStateEntries.Count > 0)
            {
                AddPdoStructureToTheExportList(actPdoStruct, true);

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
            if (WcStateEntries.Count == 0)
            {
                WcState = null;
            }
            return WcState;
        }
    }
}
