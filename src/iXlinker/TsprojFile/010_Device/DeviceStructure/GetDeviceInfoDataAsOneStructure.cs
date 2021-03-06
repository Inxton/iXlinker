using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using System.Collections.Generic;
using PlcprojFile;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoViewModel GetDeviceInfoDataAsOneStructure(TcSmDevDef device, DeviceViewModel deviceViewModel)
        {
            ObservableCollection<PdoEntryViewModel> InfoDataEntries = new ObservableCollection<PdoEntryViewModel>();

            //bool infoDataSupport = true;
            //bool infoDataId = true;
            //bool infoDataNetId = true;
            //bool infoDataChangeCnt = true;
            //bool infoDataCfgSlaveCnt = true;
            //bool infoDataDcTimeOffsets = false;
            //try
            //{
            //    TcSmDevDefEtherCAT tcSmDevDefEtherCAT = (TcSmDevDefEtherCAT)device.Items[0];

            //    if (tcSmDevDefEtherCAT.InfoDataSupportSpecified && !tcSmDevDefEtherCAT.InfoDataSupport) infoDataSupport = false;
            //    if (tcSmDevDefEtherCAT.InfoDataIdSpecified && !tcSmDevDefEtherCAT.InfoDataId) infoDataId = false;
            //    if (tcSmDevDefEtherCAT.InfoDataNetIdSpecified && !tcSmDevDefEtherCAT.InfoDataNetId) infoDataNetId = false;
            //    if (tcSmDevDefEtherCAT.InfoDataChangeCntSpecified && !tcSmDevDefEtherCAT.InfoDataChangeCnt) infoDataChangeCnt = false;
            //    if (tcSmDevDefEtherCAT.InfoDataCfgSlaveCntSpecified && !tcSmDevDefEtherCAT.InfoDataCfgSlaveCnt) infoDataCfgSlaveCnt = false;
            //    if (tcSmDevDefEtherCAT.InfoDataDcTimeOffsetsSpecified && tcSmDevDefEtherCAT.InfoDataDcTimeOffsets) infoDataDcTimeOffsets = true;
            //}
            //catch (Exception ex)
            //{
            //    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            //}
            //Try to avoid the same structure for the device info data, even in the case when for example DcTimeOffsets are not used
            bool infoDataSupport = true;
            bool infoDataId = true;
            bool infoDataNetId = true;
            bool infoDataChangeCnt = true;
            bool infoDataCfgSlaveCnt = true;
            bool infoDataDcTimeOffsets = true;

            deviceViewModel.InfoDataSupport = infoDataSupport;
            deviceViewModel.InfoDataId = infoDataId;
            deviceViewModel.InfoDataNetId = infoDataNetId;
            deviceViewModel.InfoDataChangeCnt = infoDataChangeCnt;
            deviceViewModel.InfoDataCfgSlaveCnt = infoDataCfgSlaveCnt;
            deviceViewModel.InfoDataDcTimeOffsets = infoDataDcTimeOffsets;

            if (infoDataSupport)
            {
                if (infoDataChangeCnt)
                {
                    PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "ChangeCount", VarB = "InfoData" + tmpLevelSeparator + "ChangeCount", VarA = "InfoData" + tmpLevelSeparator + "ChangeCount", Type_Value = "UINT", InOut = "0" };
                    pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                    InfoDataEntries.Add(pdoEntryViewModel);
                }
                if (infoDataId)
                {
                    PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "DevId", VarB = "InfoData" + tmpLevelSeparator + "DevId", VarA = "InfoData" + tmpLevelSeparator + "DevId", Type_Value = "UINT", InOut = "0" };
                    pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                    InfoDataEntries.Add(pdoEntryViewModel);
                }
                if (infoDataNetId)
                {
                    PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "AmsNetId", VarB = "InfoData" + tmpLevelSeparator + "AmsNetId", VarA = "InfoData" + tmpLevelSeparator + "AmsNetId", Type_Value = ReplaceSpecialPlcTypeIfFoundInLibrary("AMSNETID"), InOut = "0" };
                    pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                    InfoDataEntries.Add(pdoEntryViewModel);
                }
                if (infoDataCfgSlaveCnt)
                {
                    PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "CfgSlaveCount", VarB = "InfoData" + tmpLevelSeparator + "CfgSlaveCount", VarA = "InfoData" + tmpLevelSeparator + "CfgSlaveCount", Type_Value = "UINT", InOut = "0" };
                    pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                    InfoDataEntries.Add(pdoEntryViewModel);
                }
                if (infoDataDcTimeOffsets)
                {
                    PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "DcToTcTimeOffset", VarB = "InfoData" + tmpLevelSeparator + "DcToTcTimeOffset", VarA = "InfoData" + tmpLevelSeparator + "DcToTcTimeOffset", Type_Value = "LINT", InOut = "0" };
                    pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                    InfoDataEntries.Add(pdoEntryViewModel);
                    pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = "TIID" + tmpLevelSeparator + device.Name, Name = "DcToExtTimeOffset", VarB = "InfoData" + tmpLevelSeparator + "DcToExtTimeOffset", VarA = "InfoData" + tmpLevelSeparator + "DcToExtTimeOffset", Type_Value = "LINT", InOut = "0" };
                    pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                    InfoDataEntries.Add(pdoEntryViewModel);
                }
            }

            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "InfoData", Id = "", BoxOrderCode = deviceViewModel.Type.ToString()};
            MappableObject mapableObject = new MappableObject();

            foreach (PdoEntryViewModel pdoEntry in InfoDataEntries)
            {
                PdoStructMemberViewModel member = new PdoStructMemberViewModel();
                string pdoEntryName = ValidatePlcItem.Name(pdoEntry.Name);
                member.Attributes = new List<string> { "{attribute addProperty Name \"" + pdoEntryName + "\"}" };
                member.Name = pdoEntryName;
                member.BoxOrderCode = pdoEntry.BoxOrderCode;
                member.Type_Value = pdoEntry.Type_Value;
                member.TypeNamespace = pdoEntry.TypeNamespace;
                if (pdoEntry.InOut == "1")
                {
                    member.InOutPlcProj = "AT %Q*";
                    member.InOutMappings = "Outputs";
                }
                else
                {
                    member.InOutPlcProj = "AT %I*";
                    member.InOutMappings = "Inputs";
                }
                member.OwnerBname = pdoEntry.OwnerBname;
                member.Size = pdoEntry.Size;
                member.Index = pdoEntry.Index;
                member.IndexNumber = pdoEntry.IndexNumber;
                member.SubIndex = pdoEntry.SubIndex;
                member.SubIndexNumber = pdoEntry.SubIndexNumber;
                actPdoStruct.AddMemberAndUpdateIdAndSize(member);

                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator, "");
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                mapableObject.MapableItems.Add(mapableItem);
            }
            PdoViewModel InfoData = new PdoViewModel(); ;
            if (InfoDataEntries.Count > 0)
            {
                AddPdoStructureToTheExportList(actPdoStruct,true);

                PdoStructMemberViewModel firstStructMember = actPdoStruct.StructMembers.FirstOrDefault();
                InfoData.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                InfoData.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                InfoData.TypeNamespace = actPdoStruct.TypeNamespace;
                InfoData.OwnerBname = firstStructMember.OwnerBname;
                InfoData.InOutPlcProj = firstStructMember.InOutPlcProj;
                InfoData.InOutMappings = firstStructMember.InOutMappings;
                InfoData.BoxOrderCode = firstStructMember.BoxOrderCode;
                InfoData.Size = actPdoStruct.Size;

                mapableObject.Name = ValidatePlcItem.Name(actPdoStruct.Prefix);
                mapableObject.Type_Value = ValidatePlcItem.Type(actPdoStruct.Name);
                mapableObject.TypeNamespace = actPdoStruct.TypeNamespace;
                mapableObject.Size = actPdoStruct.Size;

                InfoData.MapableObject = mapableObject;
            }
            if (InfoDataEntries.Count == 0)
            {
                InfoData = null;
            }
            return InfoData;
        }
    }
}
