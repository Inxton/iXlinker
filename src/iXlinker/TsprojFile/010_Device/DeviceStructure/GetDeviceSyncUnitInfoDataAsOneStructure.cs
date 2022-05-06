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
        private PdoViewModel GetDeviceSyncUnitInfoDataAsOneStructure(TcSmProjectProjectIODevice device, DeviceViewModel deviceViewModel, string plcTaskA , string plcTaskB)
        {

            ObservableCollection<PdoEntryViewModel> InfoDataEntries = new ObservableCollection<PdoEntryViewModel>();
            PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "ObjectId", VarB = plcTaskB + tmpLevelSeparator + "InfoData" + tmpLevelSeparator + "ObjectId", VarA = plcTaskA + tmpLevelSeparator + "InfoData" + tmpLevelSeparator + "ObjectId", Type_Value = "OTCID", InOut = "0" };
            pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
            InfoDataEntries.Add(pdoEntryViewModel);
            pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "State", VarB = plcTaskB + tmpLevelSeparator + "InfoData" + tmpLevelSeparator + "State", VarA = plcTaskA + tmpLevelSeparator + "InfoData" + tmpLevelSeparator + "State", Type_Value = "UINT", InOut = "0" };
            pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
            InfoDataEntries.Add(pdoEntryViewModel);
            pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "SlaveCount", VarB = plcTaskB + tmpLevelSeparator + "InfoData" + tmpLevelSeparator + "SlaveCount", VarA = plcTaskA + tmpLevelSeparator + "InfoData" + tmpLevelSeparator + "SlaveCount", Type_Value = "UINT", InOut = "0" };
            pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
            InfoDataEntries.Add(pdoEntryViewModel);

            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "InfoData", Id = "", BoxOrderCode = deviceViewModel.Type.ToString() };
            MappableObject mapableObject = new MappableObject();
            PdoViewModel InfoData = new PdoViewModel(); ;

            foreach (PdoEntryViewModel pdoEntry in InfoDataEntries)
            {
                InfoData.PdoEntriesStructured.Add(pdoEntry);
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
            if (InfoDataEntries.Count > 0)
            {
                AddPdoStructureToTheExportList(actPdoStruct, true);

                PdoStructMemberViewModel firstStructMember = actPdoStruct.StructMembers.FirstOrDefault();
                InfoData.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                InfoData.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                InfoData.TypeNamespace = actPdoStruct.TypeNamespace;
                InfoData.OwnerBname = firstStructMember.OwnerBname;
                InfoData.InOutPlcProj = firstStructMember.InOutPlcProj;
                InfoData.InOutMappings = firstStructMember.InOutMappings;
                InfoData.BoxOrderCode = firstStructMember.BoxOrderCode;
                InfoData.Size = actPdoStruct.Size;

                mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
                mapableObject.Size = actPdoStruct.Size;

                InfoData.MapableObject = mapableObject;
            }
            if (InfoDataEntries.Count == 0)
            {
                InfoData = null;
            }
            return InfoData;
        }
    }
}
