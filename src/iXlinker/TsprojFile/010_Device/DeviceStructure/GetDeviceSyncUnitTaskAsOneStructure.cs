using iXlinkerDtos;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using System.Collections.Generic;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoViewModel GetDeviceSyncUnitTaskAsOneStructure(string task, DeviceViewModel deviceViewModel, PdoViewModel WcState, PdoViewModel InfoData)
        {

            ObservableCollection<PdoViewModel> SyncUnitTaskEntries = new ObservableCollection<PdoViewModel>();
            SyncUnitTaskEntries.Add(WcState);
            SyncUnitTaskEntries.Add(InfoData);

            DeviceStructViewModel actDevStruct = new DeviceStructViewModel() { Prefix = "SyncUnitTask", Id = "", BoxOrderCode = deviceViewModel.Type.ToString() };
            MappableObject mapableObject = new MappableObject();
            PdoViewModel SyncUnitTask = new PdoViewModel(); ;

            foreach (PdoViewModel pdo in SyncUnitTaskEntries)
            {
                DeviceStructMemberViewModel member = new DeviceStructMemberViewModel();
                string pdoName = ValidatePlcItem.Name(pdo.Name);
                member.Attributes = new List<string> { "{attribute addProperty Name \"" + pdoName + "\"}" };
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
                actDevStruct.StructMembers.Add(member);
                //actDevStruct.Id = actDevStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes + member.SubIndexNumber;
                actDevStruct.Id = actDevStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.SizeInBytes;
                actDevStruct.SizeInBites = actDevStruct.SizeInBites + member.SizeInBites;
                actDevStruct.SizeInBytes = actDevStruct.SizeInBytes + member.SizeInBytes;

                foreach (PdoEntryViewModel pdoEntry in pdo.PdoEntriesStructured)
                {
                    SyncUnitTask.PdoEntriesStructured.Add(pdoEntry);

                    string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                    MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                    mapableObject.MapableItems.Add(mapableItem);
                }
            }
            if (SyncUnitTaskEntries.Count > 0)
            {
                ValidateDeviceStructMemberNamesUniqueness(ref actDevStruct);
                //Calculate CRC of the actPdoStruct.Id
                actDevStruct.Crc32 = CRC32.Calculate_CRC32(actDevStruct.Id);
                actDevStruct.Name = ValidatePlcItem.Name(actDevStruct.Prefix + "_" + actDevStruct.Crc32.ToString("X8"));
                //Check if such an structure exists
                if (CheckIfDeviceStructureDoesNotExist(actDevStruct))
                {
                    //if not add to the structure list
                    DeviceStructures.Add(actDevStruct);
                }
                DeviceStructMemberViewModel firstStructMember = actDevStruct.StructMembers.FirstOrDefault();
                SyncUnitTask.Name = ValidatePlcItem.Name(task);
                SyncUnitTask.Type_Value = ValidatePlcItem.Type(actDevStruct.Name);
                SyncUnitTask.TypeNamespace = actDevStruct.TypeNamespace;
                SyncUnitTask.OwnerBname = firstStructMember.OwnerBname;
                SyncUnitTask.InOutPlcProj = firstStructMember.InOutPlcProj;
                SyncUnitTask.InOutMappings = firstStructMember.InOutMappings;
                SyncUnitTask.BoxOrderCode = firstStructMember.BoxOrderCode;
                SyncUnitTask.SizeInBites = actDevStruct.SizeInBites;
                SyncUnitTask.SizeInBytes = actDevStruct.SizeInBytes;

                mapableObject.Name = ValidatePlcItem.Name(task);
                mapableObject.Type_Value = ValidatePlcItem.Type(actDevStruct.Name);
                mapableObject.TypeNamespace = actDevStruct.TypeNamespace;
                mapableObject.SizeInBites = actDevStruct.SizeInBites;
                mapableObject.SizeInBytes = actDevStruct.SizeInBytes;

                SyncUnitTask.MapableObject = mapableObject;
            }
            if (SyncUnitTaskEntries.Count == 0)
            {
                SyncUnitTask = null;
            }

            return SyncUnitTask;
        }
    }
}
