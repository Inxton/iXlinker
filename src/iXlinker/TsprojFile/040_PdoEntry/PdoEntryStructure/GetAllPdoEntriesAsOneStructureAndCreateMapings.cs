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
        private void GetAllPdoEntriesAsOneStructureAndCreateMapings(ObservableCollection<PdoEntryViewModel> pdoEntries, ref PdoViewModel pdoViewModel)
        {
            string pdoViewModelName = pdoViewModel.Name;
            if (pdoViewModelName.Contains(tmpSlotSeparator))
            {
                pdoViewModelName = pdoViewModelName.Substring(pdoViewModelName.LastIndexOf(tmpSlotSeparator, StringComparison.Ordinal) + 1);
            }
            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = ValidatePlcItem.StructurePrefix(pdoViewModelName), Id = "", BoxOrderCode = pdoViewModel.BoxOrderCode };
            MappableObject mapableObject = new MappableObject();
            if (pdoEntries != null)
            {
                foreach (PdoEntryViewModel pdoEntry in pdoEntries)
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
                    member.OwnerBname = pdoEntry.OwnerBname;
                    member.Size = pdoEntry.Size;
                    member.Index = pdoEntry.Index;
                    member.IndexNumber = pdoEntry.IndexNumber;
                    member.SubIndex = pdoEntry.SubIndex;
                    member.SubIndexNumber = pdoEntry.SubIndexNumber;
                    actPdoStruct.AddMemberAndUpdateIdAndSize(member);
                    foreach (MappableItem mapableItem in pdoEntry.MapableObject.MapableItems)
                    {
                        mapableObject.MapableItems.Add(mapableItem);
                    }
                }

                ValidatePdoStructMemberNamesUniqueness(ref actPdoStruct);

                //Calculate CRC of the actPdoEntryStruct.Id
                actPdoStruct.Crc32 = CRC32.Calculate_CRC32(actPdoStruct.Id);
                actPdoStruct.Name = ValidatePlcItem.Name(actPdoStruct.Prefix + "_" + actPdoStruct.Crc32.ToString("X8"));
            }
            else
            {
                actPdoStruct.Name = emptyStructure;
                actPdoStruct.Id = emptyStructure;
                actPdoStruct.Crc32 = 0;
            }

            GroupPdoEntriesIntoArrayIfPossibleAndModifyMapings(ref actPdoStruct, ref pdoViewModel);

            AddPdoStructureToTheExportList(actPdoStruct,false);

            //create pdo of the structured type
            mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
            mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
            mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
            mapableObject.Size = actPdoStruct.Size;

            pdoViewModel.Size = actPdoStruct.Size;
            pdoViewModel.Type_Value = actPdoStruct.Name;
            pdoViewModel.TypeNamespace = actPdoStruct.TypeNamespace;

            pdoViewModel.MapableObject = mapableObject;
        }
    }
}
