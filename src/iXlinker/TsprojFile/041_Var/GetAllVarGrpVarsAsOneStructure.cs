using iXlinkerDtos;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using PlcprojFile;
using System;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private MappableObject GetAllVarGrpVarsAsOneStructure(PdoViewModel pdoViewModel, ObservableCollection<PdoEntryViewModel> varGrpVars)
        {
            string pdoViewModelName = pdoViewModel.Name;
            if (pdoViewModelName.Contains(tmpSlotSeparator))
            {
                pdoViewModelName = pdoViewModelName.Substring(pdoViewModelName.LastIndexOf(tmpSlotSeparator, StringComparison.Ordinal) + 1);
            }

            PdoStructViewModel actVarGrpStruct = new PdoStructViewModel() { Prefix = ValidatePlcItem.StructurePrefix(pdoViewModel.Name), Id = "", BoxOrderCode = pdoViewModel.BoxOrderCode };
            MappableObject mapableObject = new MappableObject();
            foreach (PdoEntryViewModel _var in varGrpVars)
            {
                PdoStructMemberViewModel member = new PdoStructMemberViewModel();
                string _varName = ValidatePlcItem.Name(_var.Name);
                member.Attributes.Add("{attribute addProperty Name \"" + _varName + "\"}");
                member.Name = _varName;
                member.BoxOrderCode = _var.BoxOrderCode;
                member.Type_Value = _var.Type_Value;
                member.TypeNamespace= _var.TypeNamespace;
                if (_var.InOut == "1")
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
                member.OwnerBname = _var.OwnerBname;
                member.SizeInBites = _var.SizeInBites;
                member.SizeInBytes = _var.SizeInBytes;
                member.Index = _var.Index;
                member.IndexNumber = _var.IndexNumber;
                member.SubIndex = _var.SubIndex;
                member.SubIndexNumber = _var.SubIndexNumber;
                actVarGrpStruct.StructMembers.Add(member);
                //actVarGrpStruct.Id = actVarGrpStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                actVarGrpStruct.Id = actVarGrpStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBytes;
                actVarGrpStruct.SizeInBites = actVarGrpStruct.SizeInBites + member.SizeInBites;
                actVarGrpStruct.SizeInBytes = actVarGrpStruct.SizeInBytes + member.SizeInBytes;

                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = _var.VarA, VarB = _var.VarB };
                mapableObject.MapableItems.Add(mapableItem);
            }

            ValidatePdoStructMemberNamesUniqueness(ref actVarGrpStruct);

            //Calculate CRC of the actVarGrpStruct.Id
            actVarGrpStruct.Crc32 = CRC32.Calculate_CRC32(actVarGrpStruct.Id);
            actVarGrpStruct.Name = ValidatePlcItem.Name(actVarGrpStruct.Prefix + "_" + actVarGrpStruct.Crc32.ToString("X8"));
            //Check if such an structure exists
            if (CheckIfPdoStructureDoesNotExist(actVarGrpStruct))
            {
                //if not add to the structure list
                PdoStructures.Add(actVarGrpStruct);
            }
            //create varGrp of the structured type
            PdoStructMemberViewModel firstStructMember = actVarGrpStruct.StructMembers.FirstOrDefault();
            mapableObject.Name = ValidatePlcItem.Name(actVarGrpStruct.Prefix);
            mapableObject.Type_Value = ValidatePlcItem.Type(actVarGrpStruct.Name);
            mapableObject.TypeNamespace = actVarGrpStruct.TypeNamespace;
            mapableObject.SizeInBites = actVarGrpStruct.SizeInBites;
            mapableObject.SizeInBytes = actVarGrpStruct.SizeInBytes;

            pdoViewModel.SizeInBites = actVarGrpStruct.SizeInBites;
            pdoViewModel.SizeInBytes = actVarGrpStruct.SizeInBytes;
            pdoViewModel.Type_Value = actVarGrpStruct.Name;
            pdoViewModel.TypeNamespace = actVarGrpStruct.TypeNamespace;

            return mapableObject;

        }
    }
}
