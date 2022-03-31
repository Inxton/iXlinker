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
        private PdoViewModel GetAllPdosAsOneStructure(BoxViewModel boxViewModel, ObservableCollection<PdoViewModel> pdos)
        {

            ObservableCollection<PdoViewModel> slotPdos = GetAllPdosAsOneStructuresOfSlotTypes(boxViewModel, pdos);

            MappableObject mapableObject = new MappableObject();
            PdoViewModel pdoViewModel = new PdoViewModel();
            string structName = boxViewModel.BoxOrderCode;
            if (structName.Contains("-"))
            {
                structName = structName.Substring(0, structName.IndexOf("-", StringComparison.Ordinal));
            }
            BoxStructViewModel actBoxStruct = new BoxStructViewModel() { Prefix = ValidatePlcItem.Name(structName), Id = "", BoxOrderCode = boxViewModel.BoxOrderCode };
            foreach (PdoViewModel pdo in slotPdos)
            {
                BoxStructMemberViewModel member = new BoxStructMemberViewModel();
                string pdoName = ValidatePlcItem.Name(pdo.Name);
                member.Attributes.Add("{attribute addProperty Name \"" + pdoName + "\"}");
                member.Name = pdoName;
                member.BoxOrderCode = pdo.BoxOrderCode;
                member.Type_Value = pdo.Type_Value;
                member.TypeNamespace = pdo.TypeNamespace;
                member.InOutPlcProj = pdo.InOutPlcProj;
                member.InOutMappings = pdo.InOutMappings;
                member.OwnerBname = pdo.OwnerBname;
                member.SizeInBites = pdo.SizeInBites;
                member.SizeInBytes = pdo.SizeInBytes;
                member.Index = pdo.Index;
                member.IndexNumber = pdo.IndexNumber;
                actBoxStruct.StructMembers.Add(member);
                actBoxStruct.Id = actBoxStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes;
                actBoxStruct.SizeInBites = actBoxStruct.SizeInBites + member.SizeInBites;
                actBoxStruct.SizeInBytes = actBoxStruct.SizeInBytes + member.SizeInBytes;

                foreach (PdoEntryViewModel pdoEntry in pdo.PdoEntriesUnstructured)
                {
                    pdoViewModel.PdoEntriesUnstructured.Add(pdoEntry);
                }
                foreach (PdoEntryViewModel pdoEntry in pdo.PdoEntriesStructured)
                {
                    pdoViewModel.PdoEntriesStructured.Add(pdoEntry);
                }

                foreach (MappableItem mapableItem in pdo.MapableObject.MapableItems)
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

                //GroupPdosIntoArrayIfPossible(ref actBoxStruct, ref mapableObject);

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
