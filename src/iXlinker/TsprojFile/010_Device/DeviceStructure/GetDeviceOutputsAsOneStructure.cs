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
        private PdoViewModel GetDeviceOutputsAsOneStructure(TcSmProjectProjectIODevice device, DeviceViewModel deviceViewModel, uint frames)
        {
            ObservableCollection<PdoEntryViewModel> OutputEntries = new ObservableCollection<PdoEntryViewModel>();

            PdoEntryViewModel pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "DevCtrl", VarB = "Outputs" + tmpLevelSeparator + "DevCtrl", VarA = "Outputs" + tmpLevelSeparator + "DevCtrl", Type_Value = "UINT", InOut = "1" };
            pdo.SizeInBites = PlcBaseTypes.GetSizeInBites(pdo.Type_Value);
            pdo.SizeInBytes = PlcBaseTypes.GetSizeInBytes(pdo.Type_Value);
            OutputEntries.Add(pdo);

            for (int i = 0; i <= frames; i++)
            {
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "Ctrl", VarB = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "Ctrl", VarA = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "Ctrl", Type_Value = "UINT", InOut = "1" };
                pdo.SizeInBites = PlcBaseTypes.GetSizeInBites(pdo.Type_Value);
                pdo.SizeInBytes = PlcBaseTypes.GetSizeInBytes(pdo.Type_Value);
                OutputEntries.Add(pdo);
                pdo = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "Frm" + i.ToString() + "WcCtrl", VarB = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "WcCtrl", VarA = "Outputs" + tmpLevelSeparator + "Frm" + i.ToString() + "WcCtrl", Type_Value = "UINT", InOut = "1" };
                pdo.SizeInBites = PlcBaseTypes.GetSizeInBites(pdo.Type_Value);
                pdo.SizeInBytes = PlcBaseTypes.GetSizeInBytes(pdo.Type_Value);
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
                member.SizeInBites = pdoEntry.SizeInBites;
                member.SizeInBytes = pdoEntry.SizeInBytes;
                member.Index = pdoEntry.Index;
                member.IndexNumber = pdoEntry.IndexNumber;
                member.SubIndex = pdoEntry.SubIndex;
                member.SubIndexNumber = pdoEntry.SubIndexNumber;
                actPdoStruct.StructMembers.Add(member);
                //actPdoStruct.Id = actPdoStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                actPdoStruct.Id = actPdoStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBytes;
                actPdoStruct.SizeInBites = actPdoStruct.SizeInBites + member.SizeInBites;
                actPdoStruct.SizeInBytes = actPdoStruct.SizeInBytes + member.SizeInBytes;

                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                mapableObject.MapableItems.Add(mapableItem);
            }
            PdoViewModel Outputs = new PdoViewModel(); ;
            if (OutputEntries.Count > 0)
            {
                ValidatePdoStructMemberNamesUniqueness(ref actPdoStruct);
                //Calculate CRC of the actPdoStruct.Id
                actPdoStruct.Crc32 = CRC32.Calculate_CRC32(actPdoStruct.Id);
                actPdoStruct.Name = ValidatePlcItem.Name(actPdoStruct.Prefix + "_" + actPdoStruct.Crc32.ToString("X8"));
                //Check if such an structure exists
                if (CheckIfPdoStructureDoesNotExist(actPdoStruct))
                {
                    //if not add to the structure list
                    PdoStructures.Add(actPdoStruct);
                }
                PdoStructMemberViewModel firstStructMember = actPdoStruct.StructMembers.FirstOrDefault();
                Outputs.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                Outputs.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                Outputs.TypeNamespace = actPdoStruct.TypeNamespace;
                Outputs.OwnerBname = firstStructMember.OwnerBname;
                Outputs.InOutPlcProj = firstStructMember.InOutPlcProj;
                Outputs.InOutMappings = firstStructMember.InOutMappings;
                Outputs.BoxOrderCode = firstStructMember.BoxOrderCode;
                Outputs.SizeInBites = actPdoStruct.SizeInBites;
                Outputs.SizeInBytes = actPdoStruct.SizeInBytes;

                mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
                mapableObject.SizeInBites = actPdoStruct.SizeInBites;
                mapableObject.SizeInBytes = actPdoStruct.SizeInBytes;

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
