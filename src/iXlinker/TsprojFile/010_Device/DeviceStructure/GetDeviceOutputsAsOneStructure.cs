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
        private PdoViewModel GetDeviceOutputsAsOneStructure(TcSmDevDef device, DeviceViewModel deviceViewModel, uint frames)
        {
            ObservableCollection<PdoEntryViewModel> OutputEntries = new ObservableCollection<PdoEntryViewModel>();

            PdoEntryViewModel pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "DevCtrl", VarB = "Outputs" + tmpLevelSeparator + "DevCtrl", VarA = "Outputs" + tmpLevelSeparator + "DevCtrl", Type_Value = "UINT", InOut = "1" };
            pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
            OutputEntries.Add(pdo);

            for (int i = 0; i <= frames; i++)
            {
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "Ctrl", VarB = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "Ctrl", VarA = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "Ctrl", Type_Value = "UINT", InOut = "1" };
                pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
                OutputEntries.Add(pdo);
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "WcCtrl", VarB = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "WcCtrl", VarA = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "WcCtrl", Type_Value = "UINT", InOut = "1" };
                pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
                OutputEntries.Add(pdo);
            }

            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "Outputs", Id = "", BoxOrderCode = deviceViewModel.Type.ToString() };
            MappableObject mapableObject = new MappableObject();

            foreach (PdoEntryViewModel pdoEntry in OutputEntries)
            {
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
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                mapableObject.MapableItems.Add(mapableItem);
            }
            PdoViewModel Outputs = new PdoViewModel(); ;
            if (OutputEntries.Count > 0)
            {
                AddPdoStructureToTheExportList(actPdoStruct, true);

                PdoStructMemberViewModel firstStructMember = actPdoStruct.StructMembers.FirstOrDefault();
                Outputs.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                Outputs.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                Outputs.TypeNamespace = actPdoStruct.TypeNamespace;
                Outputs.OwnerBname = firstStructMember.OwnerBname;
                Outputs.InOutPlcProj = firstStructMember.InOutPlcProj;
                Outputs.InOutMappings = firstStructMember.InOutMappings;
                Outputs.BoxOrderCode = firstStructMember.BoxOrderCode;
                Outputs.Size = actPdoStruct.Size;

                mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
                mapableObject.Size = actPdoStruct.Size;

                Outputs.MapableObject = mapableObject;
            }
            if (OutputEntries.Count == 0)
            {
                Outputs = null;
            }
            return Outputs;

        }
    }
}
