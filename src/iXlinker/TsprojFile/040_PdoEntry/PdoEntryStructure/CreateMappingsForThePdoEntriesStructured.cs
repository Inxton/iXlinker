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
        private MappableObject CreateMappingsForThePdoEntriesStructured(PdoEntryViewModel pdoEntryViewModel, PdoEntryStructViewModel actPdoEntryStruct)
        {
            MappableObject mapableObject = new MappableObject();
            if (actPdoEntryStruct.StructMembers != null)
            {
                foreach (PdoEntryStructMemberViewModel member in actPdoEntryStruct.StructMembers)
                {
                    string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                    MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntryViewModel.VarA + tmpStructSeparator + member.NameA, VarB = pdoEntryViewModel.VarB + tmpStructSeparator + member.NameB };
                    mapableObject.MapableItems.Add(mapableItem);
                }
            }
            else
            {
                actPdoEntryStruct.Name = emptyStructure;
                actPdoEntryStruct.Id = emptyStructure;
                actPdoEntryStruct.Crc32 = 0;
            }

            //create pdo of the structured type
            mapableObject.Name = ValidatePlcItem.Name(actPdoEntryStruct.Prefix);
            mapableObject.Type_Value = ValidatePlcItem.Type(actPdoEntryStruct.Name);
            mapableObject.TypeNamespace = actPdoEntryStruct.TypeNamespace;
            mapableObject.Size = actPdoEntryStruct.Size;

            return mapableObject;
        }
    }
}
