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
        private PdoEntryViewModel CreateMappingsForThePdoEntriesOfArrayType(PdoEntryViewModel pdoEntryViewModel, PdoEntryStructViewModel actPdoEntryStruct)
        {
            MappableObject mapableObject = new MappableObject();

            int arrayIndex = 0;
            double _size = 0;

            foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
            {
                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                string varA = pdoEntryViewModel.VarA + tmpLevelSeparator + member.NameA.Substring(0, member.NameA.LastIndexOf("_")) + "[" + arrayIndex.ToString() + "]";
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = varA , VarB = pdoEntryViewModel.VarB + tmpLevelSeparator + member.NameA };
                mapableObject.MapableItems.Add(mapableItem);
                _size = _size + member.Size;
                arrayIndex++;
            }


            //create pdo of the structured type
            mapableObject.Name = ValidatePlcItem.Name(actPdoEntryStruct.Prefix);
            mapableObject.Type_Value = ValidatePlcItem.Type(actPdoEntryStruct.Name);
            mapableObject.TypeNamespace = actPdoEntryStruct.TypeNamespace;
            mapableObject.Size = actPdoEntryStruct.Size;


            pdoEntryViewModel.MapableObject = mapableObject;
            pdoEntryViewModel.Size = _size;
            return pdoEntryViewModel;
        }
    }
}
