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
                member.Size = _var.Size;
                member.Index = _var.Index;
                member.IndexNumber = _var.IndexNumber;
                member.SubIndex = _var.SubIndex;
                member.SubIndexNumber = _var.SubIndexNumber;
                actVarGrpStruct.AddMemberAndUpdateIdAndSize(member);
                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = _var.VarA, VarB = _var.VarB };
                mapableObject.MapableItems.Add(mapableItem);
            }

            AddPdoStructureToTheExportList(actVarGrpStruct,true);

            //create varGrp of the structured type
            PdoStructMemberViewModel firstStructMember = actVarGrpStruct.StructMembers.FirstOrDefault();
            mapableObject.Name = ValidatePlcItem.Name(actVarGrpStruct.Prefix);
            mapableObject.Type_Value = ValidatePlcItem.Type(actVarGrpStruct.Name);
            mapableObject.TypeNamespace = actVarGrpStruct.TypeNamespace;
            mapableObject.Size = actVarGrpStruct.Size;

            pdoViewModel.Size = actVarGrpStruct.Size;
            pdoViewModel.Type_Value = actVarGrpStruct.Name;
            pdoViewModel.TypeNamespace = actVarGrpStruct.TypeNamespace;

            return mapableObject;

        }
    }
}
