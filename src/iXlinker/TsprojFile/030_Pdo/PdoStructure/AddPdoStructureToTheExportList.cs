using iXlinkerDtos;
using PlcprojFile;
using Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddPdoStructureToTheExportList(PdoStructViewModel actPdoStruct)
        {
            ValidatePdoStructMemberNamesUniqueness(ref actPdoStruct);
            //Calculate CRC of the actPdoStruct.Id
            actPdoStruct.Crc32 = CRC32.Calculate_CRC32(actPdoStruct.Id);
            actPdoStruct.Name = ValidatePlcItem.Name(actPdoStruct.Prefix + "_" + actPdoStruct.Crc32.ToString("X8"));

            ReplacePdoStructureNameByItsReplacement(actPdoStruct);
            AddExtensionFromBasePdoStructure(actPdoStruct);

            bool isNewStructure = true;
            actPdoStruct.NumberOfUses = 1;
            //Lookup already added structures, if such a name exists
            foreach (PdoStructViewModel structVM in PdoStructures)
            {
                if (actPdoStruct.Name.Equals(structVM.Name) && actPdoStruct.Crc32.Equals(structVM.Crc32) && actPdoStruct.Id.Equals(structVM.Id))
                {
                    isNewStructure = false;
                    structVM.NumberOfUses++;
                    break;
                }

            }
            //Lookup PLC library structures, if such a name exists
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (actPdoStruct.Name.Equals(plcStruct.Name))
                {
                    actPdoStruct.TypeNamespace = plcStruct.Namespace;
                    isNewStructure = false;
                    plcStruct.NumberOfUses++;
                    break;
                }
            }

            if (isNewStructure)
            {
                PdoStructures.Add(actPdoStruct);
            }
        }
    }
}
