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
        private PdoViewModel GetDeviceInputsAsOneStructure(TcSmProjectProjectIODevice device, DeviceViewModel deviceViewModel, uint frames)
        {
            ObservableCollection<PdoEntryViewModel> InputEntries = new ObservableCollection<PdoEntryViewModel>();

            PdoEntryViewModel pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "SlaveCount" , VarB = "Inputs" + tmpLevelSeparator + "SlaveCount", VarA = "Inputs" + tmpLevelSeparator + "SlaveCount", Type_Value = "UINT", InOut = "0" };
            pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
            InputEntries.Add(pdo);
            pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "DevState", VarB = "Inputs" + tmpLevelSeparator + "DevState", VarA = "Inputs" + tmpLevelSeparator + "DevState", Type_Value = "UINT", InOut = "0" };
            pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
            InputEntries.Add(pdo);

            for (int i = 0; i <= frames; i++)
            {
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "State" , VarB = "Inputs" + tmpLevelSeparator + "Frm" + i.ToString() + "State", VarA = "Inputs" + tmpLevelSeparator + "Frm" + i.ToString() + "State", Type_Value = "UINT", InOut = "0" };
                pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
                InputEntries.Add(pdo);
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "WcState", VarB = "Inputs" + tmpLevelSeparator + "Frm" + i.ToString() + "WcState", VarA = "Inputs" + tmpLevelSeparator + "Frm" + i.ToString() + "WcState", Type_Value = "UINT", InOut = "0" };
                pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
                InputEntries.Add(pdo);
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "InputToggle", VarB = "Inputs" + tmpLevelSeparator + "Frm" + i.ToString() + "InputToggle", VarA = "Inputs" + tmpLevelSeparator + "Frm" + i.ToString() + "InputToggle", Type_Value = "UINT", InOut = "0" };
                pdo.Size = PlcBaseTypes.GetSize(pdo.Type_Value);
                InputEntries.Add(pdo);
            }

            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "Inputs", Id = "", BoxOrderCode = deviceViewModel.Type.ToString() };
            MappableObject mapableObject = new MappableObject();

            foreach (PdoEntryViewModel pdoEntry in InputEntries)
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
            PdoViewModel Inputs = new PdoViewModel(); ;
            if (InputEntries.Count > 0)
            {
                ValidatePdoStructMemberNamesUniqueness(ref actPdoStruct);
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
                Inputs.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                Inputs.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                Inputs.TypeNamespace = actPdoStruct.TypeNamespace;
                Inputs.OwnerBname = firstStructMember.OwnerBname;
                Inputs.InOutPlcProj = firstStructMember.InOutPlcProj;
                Inputs.InOutMappings = firstStructMember.InOutMappings;
                Inputs.BoxOrderCode = firstStructMember.BoxOrderCode;
                Inputs.Size = actPdoStruct.Size;

                mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
                mapableObject.Size = actPdoStruct.Size;

                Inputs.MapableObject = mapableObject;
            }
            if (InputEntries.Count == 0)
            {
                Inputs = null;
            }
            return Inputs;

        }
    }
}
