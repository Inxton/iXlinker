using System;
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
        private PdoViewModel GetAllVarGrpsAsOneStructure(BoxViewModel boxViewModel, ObservableCollection<PdoViewModel> varGrps)
        {

            MappableObject mapableObject = new MappableObject();
            PdoViewModel pdoViewModel = new PdoViewModel();
            string structName = boxViewModel.BoxOrderCode;

            BoxStructViewModel actBoxStruct = new BoxStructViewModel() { Prefix = ValidatePlcItem.StructurePrefix(structName), Id = "", BoxOrderCode = boxViewModel.BoxOrderCode };
            foreach (PdoViewModel varGrp in varGrps)
            {
                BoxStructMemberViewModel member = new BoxStructMemberViewModel();
                string pdoName = ValidatePlcItem.Name(varGrp.Name);
                member.Attributes.Add("{attribute addProperty Name \"" + pdoName + "\"}");
                member.Name = pdoName;
                member.BoxOrderCode = varGrp.BoxOrderCode;
                member.Type_Value = varGrp.Type_Value;
                member.TypeNamespace = varGrp.TypeNamespace;
                member.InOutPlcProj = varGrp.InOutPlcProj;
                member.InOutMappings = varGrp.InOutMappings;
                member.OwnerBname = varGrp.OwnerBname;
                member.SizeInBites = varGrp.SizeInBites;
                member.SizeInBytes = varGrp.SizeInBytes;
                member.Index = varGrp.Index;
                member.IndexNumber = varGrp.IndexNumber;
                actBoxStruct.StructMembers.Add(member);
                //actBoxStruct.Id = actBoxStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes;
                actBoxStruct.Id = actBoxStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBytes;
                actBoxStruct.SizeInBites = actBoxStruct.SizeInBites + member.SizeInBites;
                actBoxStruct.SizeInBytes = actBoxStruct.SizeInBytes + member.SizeInBytes;

                foreach (PdoEntryViewModel pdoEntry in varGrp.PdoEntriesUnstructured)
                {
                    pdoViewModel.PdoEntriesUnstructured.Add(pdoEntry);
                }
                foreach (PdoEntryViewModel pdoEntry in varGrp.PdoEntriesStructured)
                {
                    pdoViewModel.PdoEntriesStructured.Add(pdoEntry);
                }

                foreach (MappableItem mapableItem in varGrp.MapableObject.MapableItems)
                {
                    mapableObject.MapableItems.Add(mapableItem);
                }
            }


            ValidateBoxStructMemberNamesUniqueness(ref actBoxStruct);
            if (actBoxStruct.StructMembers.Count > 0)
            {
                //Calculate CRC of the actPdoStruct.Id
                actBoxStruct.Crc32 = CRC32.Calculate_CRC32(actBoxStruct.Id);
                actBoxStruct.Name = ValidatePlcItem.Name(actBoxStruct.Prefix + "_" + actBoxStruct.Crc32.ToString("X8"));

                //Check if such an structure exists
                if (CheckIfBoxStructureDoesNotExist(actBoxStruct))
                {
                    //if not add to the structure list
                    BoxStructures.Add(actBoxStruct);
                }
                BoxStructMemberViewModel firstStructMember = actBoxStruct.StructMembers.FirstOrDefault();
                pdoViewModel.Name = ValidatePlcItem.Name(actBoxStruct.Prefix);
                pdoViewModel.Type_Value = actBoxStruct.Name;
                pdoViewModel.TypeNamespace = actBoxStruct.TypeNamespace;
                pdoViewModel.OwnerBname = firstStructMember.OwnerBname;
                pdoViewModel.InOutPlcProj = firstStructMember.InOutPlcProj;
                pdoViewModel.InOutMappings = firstStructMember.InOutMappings;
                pdoViewModel.BoxOrderCode = firstStructMember.BoxOrderCode;
                pdoViewModel.SizeInBites = actBoxStruct.SizeInBites;
                pdoViewModel.SizeInBytes = actBoxStruct.SizeInBytes;

                mapableObject.Name = ValidatePlcItem.Name(boxViewModel.Name);
                mapableObject.Type_Value = ValidatePlcItem.Type(actBoxStruct.Name);
                mapableObject.TypeNamespace = actBoxStruct.TypeNamespace;
                mapableObject.SizeInBites = actBoxStruct.SizeInBites;
                mapableObject.SizeInBytes = actBoxStruct.SizeInBytes;

                pdoViewModel.MapableObject = mapableObject;
            }
            else
            {
                pdoViewModel = null;
            }
            return pdoViewModel;
        }
    }
}
