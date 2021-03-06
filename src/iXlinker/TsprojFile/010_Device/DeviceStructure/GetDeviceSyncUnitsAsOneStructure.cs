using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using System.Collections.Generic;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoViewModel GetDeviceSyncUnitsAsOneStructure(TcSmDevDef device, DeviceViewModel deviceViewModel)
        {
            ObservableCollection<PdoViewModel> SyncUnits= new ObservableCollection<PdoViewModel>();

            foreach (SyncUnitViewModel su in deviceViewModel.SyncUnits)
            {
                ObservableCollection<PdoViewModel> SyncUnitTaskEntries = new ObservableCollection<PdoViewModel>();

                string suNameA = ValidatePlcItem.Name(su.Name.Replace("<", "").Replace(">", ""));
                string suNameB = su.Name.Replace("<", "&lt;").Replace(">", "&gt;");

                foreach (string task in su.PlcTasks)
                {
                    string plcTaskA = "SyncUnits" + tmpLevelSeparator + suNameA + tmpLevelSeparator + task;
                    string plcTaskB = "SyncUnits" + tmpLevelSeparator + suNameB + tmpLevelSeparator + task;

                    PdoViewModel WcState = GetDeviceSyncUnitWcStateAsOneStructure(device, deviceViewModel, plcTaskA, plcTaskB);

                    PdoViewModel InfoData = GetDeviceSyncUnitInfoDataAsOneStructure(device, deviceViewModel, plcTaskA, plcTaskB);
                    if (!deviceViewModel.InfoDataSupport)
                    {
                        InfoData = null;
                    }

                    PdoViewModel SyncUnitTask = GetDeviceSyncUnitTaskAsOneStructure(task, deviceViewModel, WcState, InfoData);

                    SyncUnitTaskEntries.Add(SyncUnitTask);
                }
                PdoViewModel SyncUnitTasks = GetDeviceSyncUnitTasksAsOneStructure(SyncUnitTaskEntries);
                SyncUnitTasks.Name = su.Name;
                SyncUnits.Add(SyncUnitTasks);
            }

            DeviceStructViewModel actDevStruct = new DeviceStructViewModel() { Prefix = "SyncUnits", Id = "" };
            MappableObject mapableObject = new MappableObject();

            foreach (PdoViewModel pdo in SyncUnits)
            {
                DeviceStructMemberViewModel member = new DeviceStructMemberViewModel();
                string pdoName = ValidatePlcItem.Name(pdo.Name);
                member.Attributes = new List<string> { "{attribute addProperty Name \"" + pdoName + "\"}" };
                member.Name = pdoName;
                member.BoxOrderCode = pdo.BoxOrderCode;
                member.Type_Value = pdo.Type_Value;
                member.TypeNamespace= pdo.TypeNamespace;
                member.InOutPlcProj = pdo.InOutPlcProj;
                member.InOutMappings= pdo.InOutMappings;
                member.OwnerBname = pdo.OwnerBname;
                member.Size = pdo.Size;
                member.Index = pdo.Index;
                member.IndexNumber = pdo.IndexNumber;
                actDevStruct.AddMemberAndUpdateIdAndSize(member);

                foreach (PdoEntryViewModel pdoEntry in pdo.PdoEntriesStructured)
                {
                    string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                    MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                    mapableObject.MapableItems.Add(mapableItem);
                }
            }
            PdoViewModel SyncUnit = new PdoViewModel(); ;
            if (SyncUnits.Count > 0)
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
                SyncUnit.Name = ValidatePlcItem.Name(actDevStruct.Prefix);
                SyncUnit.Type_Value = ValidatePlcItem.Type(actDevStruct.Name);
                SyncUnit.TypeNamespace = actDevStruct.TypeNamespace;
                SyncUnit.OwnerBname = firstStructMember.OwnerBname;
                SyncUnit.InOutPlcProj = firstStructMember.InOutPlcProj;
                SyncUnit.InOutMappings = firstStructMember.InOutMappings;
                SyncUnit.BoxOrderCode = firstStructMember.BoxOrderCode;
                SyncUnit.Size = actDevStruct.Size;

                mapableObject.Name = ValidatePlcItem.Name(actDevStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actDevStruct.Name);
                mapableObject.TypeNamespace = actDevStruct.TypeNamespace;
                mapableObject.Size = actDevStruct.Size;

                SyncUnit.MapableObject = mapableObject;
            }
            if (SyncUnits.Count == 0)
            {
                SyncUnit = null;
            }
            return SyncUnit;
        }
    }
}
