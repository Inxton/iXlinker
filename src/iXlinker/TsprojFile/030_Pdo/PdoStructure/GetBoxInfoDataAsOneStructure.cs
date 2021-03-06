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
        private PdoViewModel GetBoxInfoDataAsOneStructure(IBox box, BoxViewModel boxViewModel)
        {
            ObservableCollection<PdoEntryViewModel> InfoDataEntriesStructured = new ObservableCollection<PdoEntryViewModel>();
            ObservableCollection<PdoEntryViewModel> InfoDataEntriesUnstructured = new ObservableCollection<PdoEntryViewModel>();
            TcSmBoxDefEtherCAT boxItem = (TcSmBoxDefEtherCAT)box.Item;

            if (boxViewModel.InfoDataAddr)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "AdsAddr", VarB = "InfoData" + tmpLevelSeparator + "AdsAddr", VarA = "InfoData" + tmpLevelSeparator + "AdsAddr", Type_Value = ReplaceSpecialPlcTypeIfFoundInLibrary("AMSADDR"), InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesStructured.Add(pdoEntryViewModel);
                pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "netId", VarB = "InfoData" + tmpLevelSeparator + "AdsAddr.netId", VarA = "InfoData" + tmpLevelSeparator + "AdsAddr.netId", Type_Value = ReplaceSpecialPlcTypeIfFoundInLibrary("AMSNETID"), InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
                pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "port", VarB = "InfoData" + tmpLevelSeparator + "AdsAddr.port", VarA = "InfoData" + tmpLevelSeparator + "AdsAddr.port", Type_Value = "WORD", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
            }

            if (boxViewModel.InfoDataDcTimes)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "DcOutputShift", VarB = "InfoData" + tmpLevelSeparator + "DcOutputShift", VarA = "InfoData" + tmpLevelSeparator + "DcOutputShift", Type_Value = "DINT", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesStructured.Add(pdoEntryViewModel);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);

                pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "DcInputShift", VarB = "InfoData" + tmpLevelSeparator + "DcInputShift", VarA = "InfoData" + tmpLevelSeparator + "DcInputShift", Type_Value = "DINT", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesStructured.Add(pdoEntryViewModel);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
            }
            if (boxViewModel.InfoDataNetId)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "AoeNetId", VarB = "InfoData" + tmpLevelSeparator + "AoeNetId", VarA = "InfoData" + tmpLevelSeparator + "AoeNetId", Type_Value = ReplaceSpecialPlcTypeIfFoundInLibrary("AMSNETID"), InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesStructured.Add(pdoEntryViewModel);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
            }
            if (boxViewModel.InfoDataSoeDS401)
            {
                //TODO to be implemented
                //CoE  402  &  5001 profiles
                if (boxItem.CoeProfile != null)
                {
                    int chnIndex = 0;
                    foreach (EtherCATSlaveCoeProfile etherCATSlaveCoeProfile in boxItem.CoeProfile)
                    {
                        int coeProfileNo = 0;
                        int profileNo = 0;
                        int addInfo = 0;
                        Profile profileViewModel = new Profile() { CoeProfileNo = etherCATSlaveCoeProfile.ProfileNo ?? "", DisplayName = etherCATSlaveCoeProfile.DisplayName ?? "" };
                        if (profileViewModel.CoeProfileNo != null && profileViewModel.CoeProfileNo != "")
                        {
                            try
                            {
                                coeProfileNo = Int32.Parse(profileViewModel.CoeProfileNo);
                                profileNo = coeProfileNo % 65536;
                                addInfo = (coeProfileNo - profileNo) / 65536;
                                profileViewModel.ProfileNo = profileNo;
                                profileViewModel.AddInfo = addInfo;
                            }
                            catch (Exception ex)
                            {
                                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                            }
                        }
                        boxViewModel.Profiles.Add(profileViewModel);
                        //402-CANopen DS402, Ethercat CoE
                        //5001(252)-Pulse Train Interface (MDP252)
                        //5001(253)-Pulse Train Drive (MDP253)
                        //5001(703)-Stepper Drive (MDP703)
                        //5001(704)-probably positioning via DMC 
                        //5001(733) -DC Drive (MDP733)
                        //5001(742)-CANopen DS402 Ethercat CoE
                        //5001(750)-no idea yet WTH 750 is
                        //5001(0)-no idea yet WTH 0 is
                        //5001(100)-no idea yet WTH 100 is
                        //5001(200)-no idea yet WTH 200 is
                        //5001(300)-no idea yet WTH 300 is
                        if (profileNo == 402 || profileNo == 5001 && (addInfo == 252 || addInfo == 253 || addInfo == 703 || addInfo == 704 || addInfo == 733 || addInfo == 742 || addInfo == 750 || addInfo == 0 || addInfo == 100 || addInfo == 200 || addInfo == 300))
                        {
                            string entryName = "Chn" + chnIndex.ToString();
                            PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = entryName, VarB = "InfoData" + tmpLevelSeparator + entryName, VarA = "InfoData" + tmpLevelSeparator + entryName, Type_Value = "USINT", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                            pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                            InfoDataEntriesStructured.Add(pdoEntryViewModel);
                            InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
                            chnIndex++;
                        }
                    }
                }
                //SoE
                if (boxItem.SoeChannelsSpecified)
                {
                    int chnIndex = 0;
                    int soeChannels = boxItem.SoeChannels;
                    for (int i = 0; i < soeChannels; i++)
                    {
                        string entryName = "Chn" + chnIndex.ToString();
                        PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = entryName, VarB = "InfoData" + tmpLevelSeparator + entryName, VarA = "InfoData" + tmpLevelSeparator + entryName, Type_Value = "USINT", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                        pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                        InfoDataEntriesStructured.Add(pdoEntryViewModel);
                        InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
                        chnIndex++;
                    }
                }
            }
            if (boxViewModel.InfoDataState)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "State", VarB = "InfoData" + tmpLevelSeparator + "State", VarA = "InfoData" + tmpLevelSeparator + "State", Type_Value = "UINT", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesStructured.Add(pdoEntryViewModel);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
            }
            if (boxViewModel.InfoDataObjectId)
            {
                PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel() { OwnerBname = boxViewModel.OwnerBname + tmpLevelSeparator + boxViewModel.Name, Name = "ObjectId", VarB = "InfoData" + tmpLevelSeparator + "ObjectId", VarA = "InfoData" + tmpLevelSeparator + "ObjectId", Type_Value = "OTCID", InOut = "0", BoxOrderCode = boxViewModel.BoxOrderCode };
                pdoEntryViewModel.Size = PlcBaseTypes.GetSize(pdoEntryViewModel.Type_Value);
                InfoDataEntriesStructured.Add(pdoEntryViewModel);
                InfoDataEntriesUnstructured.Add(pdoEntryViewModel);
            }


            PdoStructViewModel actPdoStruct = new PdoStructViewModel() { Prefix = "InfoData", Id = "", BoxOrderCode = boxViewModel.BoxOrderCode };
            MappableObject mapableObject = new MappableObject();
            PdoViewModel InfoData = new PdoViewModel();

            foreach (PdoEntryViewModel pdoEntry in InfoDataEntriesStructured)
            {
                InfoData.PdoEntriesStructured.Add(pdoEntry);

                PdoStructMemberViewModel member = new PdoStructMemberViewModel();
                string pdoEntryName = ValidatePlcItem.Name(pdoEntry.Name);
                member.Attributes = new List<string> { "{attribute addProperty Name \"" + pdoEntryName + "\"}" };
                member.Name = pdoEntryName;
                member.BoxOrderCode = pdoEntry.BoxOrderCode;
                member.Type_Value = pdoEntry.Type_Value;
                member.TypeNamespace= pdoEntry.TypeNamespace;
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
                string varAprefix = Context + " " + member.InOutMappings + tmpLevelSeparator + NameOfTheExportedGVL + tmpLevelSeparator + (member.OwnerBname).Replace("TIID" + tmpLevelSeparator,"");
                MappableItem mapableItem = new MappableItem() { VarAprefix = varAprefix, OwnerBname = member.OwnerBname, VarA = pdoEntry.VarA, VarB = pdoEntry.VarB};
                mapableObject.MapableItems.Add(mapableItem);
            }
            foreach (PdoEntryViewModel pdoEntry in InfoDataEntriesUnstructured)
            {
                InfoData.PdoEntriesUnstructured.Add(pdoEntry);
            }

            ValidatePdoStructMemberNamesUniqueness(ref actPdoStruct);
            if (InfoDataEntriesStructured.Count > 0)
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
                mapableObject.TypeNamespace =actPdoStruct.TypeNamespace;
                mapableObject.Size = actPdoStruct.Size;

                InfoData.MapableObject = mapableObject;
            }
            else
            {
                InfoData = null;
            }
            return InfoData;
        }
    }
}
