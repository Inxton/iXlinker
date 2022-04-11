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
        //private MappableObject CreateMappingsForThePdoEntriesOfArrayType(PdoEntryViewModel pdoEntryViewModel, PdoEntryStructViewModel actPdoEntryStruct, out uint sizeInBites, out double sizeInBytes)
        private PdoEntryViewModel CreateMappingsForThePdoEntriesOfArrayType(PdoEntryViewModel pdoEntryViewModel, PdoEntryStructViewModel actPdoEntryStruct)
        {
            PdoEntryViewModel _pdoEntryViewModel = pdoEntryViewModel;
            MappableObject mapableObject = new MappableObject();

            int arrayIndex = 0;
            uint _sizeInBites = 0;
            double _sizeInBytes = 0;

            foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
            {
                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                string varA = pdoEntryViewModel.VarA + tmpLevelSeparator + member.Name.Substring(0, member.Name.LastIndexOf("_")) + "[" + arrayIndex.ToString() + "]";
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = varA , VarB = pdoEntryViewModel.VarB + tmpLevelSeparator + member.Name };
                mapableObject.MapableItems.Add(mapableItem);
                _sizeInBites = _sizeInBites + member.SizeInBites;
                _sizeInBytes = _sizeInBytes + member.SizeInBytes;
                arrayIndex++;
            }


            //create pdo of the structured type
            mapableObject.Name = ValidatePlcItem.Name(actPdoEntryStruct.Prefix);
            mapableObject.Type_Value = ValidatePlcItem.Type(actPdoEntryStruct.Name);
            mapableObject.TypeNamespace = actPdoEntryStruct.TypeNamespace;
            mapableObject.SizeInBites = actPdoEntryStruct.SizeInBites;
            mapableObject.SizeInBytes = actPdoEntryStruct.SizeInBytes;

            //sizeInBites = _sizeInBites;
            //sizeInBytes = _sizeInBytes;

            //return mapableObject;

            pdoEntryViewModel.MapableObject = mapableObject;
            pdoEntryViewModel.SizeInBites = _sizeInBites;
            pdoEntryViewModel.SizeInBytes = _sizeInBytes;
            return pdoEntryViewModel;
        }
    }
}
