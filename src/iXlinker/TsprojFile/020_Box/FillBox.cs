using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel FillBox(TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, IBox box, string parent_path)
        {
            BoxViewModel boxViewModel = new BoxViewModel();
            string ownerB_name = "";

            if (box.Item is TcSmBoxDefEtherCAT)
            {
                TcSmBoxDefEtherCAT boxItem = (TcSmBoxDefEtherCAT)box.Item;

                if (boxItem.SuName != null)
                {
                    foreach(string suName in boxItem.SuName)
                    {
                        if(suName !=null && suName != "")
                        {
                            AddSyncUnitIfNotAlreadyExists(suName, ref deviceVm);
                        }
                    }
                }

                string box_name = "";
                try
                {
                    if (box.Name != null)
                    {
                        box_name = box.Name;
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                int box_id = 0;
                try
                {
                    if (box.Id != null)
                    {
                        box_id = int.Parse(box.Id);
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                string box_product_code = "";
                try
                {
                    if (boxItem.ProductCode != null)
                    {
                        box_product_code = boxItem.ProductCode;
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                string box_revision_number = "";
                try
                {
                    if (boxItem.RevisionNo != null)
                    {
                        string revision = boxItem.RevisionNo.ToUpper().Replace("#X", "");
                        revision = revision.Substring(0, revision.Length - 4);
                        box_revision_number = (uint.Parse(revision, System.Globalization.NumberStyles.HexNumber)).ToString("0000");
                    }
                    else
                    {
                        box_revision_number = "0000";
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                string box_sub_code = "";
                try
                {
                    if (boxItem.RevisionNo != null)
                    {
                        box_sub_code = boxItem.RevisionNo.Replace("#x", "");
                        box_sub_code = box_sub_code.Substring(4);
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                string box_order_code = "";
                try
                {
                    if (boxItem.Desc != null)
                    {
                        if (boxItem.Desc.Contains("-"))
                        {
                            box_order_code = boxItem.Desc + "-" + box_revision_number;
                        }
                        else
                        {
                            box_order_code = boxItem.Desc + "-" + box_sub_code  + "-" + box_revision_number;
                        }
                    }
                    else
                    {
                        box_order_code = GetBoxNameFromProductCode(box_product_code, box_revision_number);
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
                int portPhys = boxItem.PortPhys;
                
                string physics = "";

                switch (portPhys & 0x000F)
                {
                    case 1: physics = "Y"; break;  //EtherCAT
                    case 3: physics = "K"; break;  //E-Bus
                    case 4: physics = "H"; break;  //EtherCAT hotswap
                    default: physics = "_"; break;
                }
                switch (portPhys & 0x00F0)
                {
                    case 16: physics = physics + "Y"; break;    //EtherCAT
                    case 48: physics = physics + "K"; break;    //E-Bus
                    case 64: physics = physics + "H"; break;    //EtherCAT hotswap
                    default: physics = physics + "_"; break;    
                }
                switch (portPhys & 0x0F00)
                {
                    case 256: physics = physics + "Y"; break;   //EtherCAT
                    case 768: physics = physics + "K"; break;   //E-Bus
                    case 1024: physics = physics + "H"; break;  //EtherCAT hotswap
                    default: physics = physics + "_"; break;
                }
                switch (portPhys & 0xF000)
                {
                    case 4096: physics = physics + "Y"; break;  //EtherCAT
                    case 12288: physics = physics + "K"; break; //E-Bus
                    case 16384: physics = physics + "H"; break; //EtherCAT hotswap
                    default: physics = physics + "_"; break;
                }
                while (physics.EndsWith("_"))
                {
                    physics = physics.Substring(0, physics.Length - 1);
                }
                string box_PortABoxInfo = "";
                string connectedToPort = "";
                int connectedToBoxId = 0;
                string previousPort = "";
                try
                {
                    if (boxItem.PortABoxInfo != null)
                    {
                        box_PortABoxInfo = boxItem.PortABoxInfo;
                        if (box_PortABoxInfo != "#x00ffffff")
                        {
                            ownerB_name = parent_path;

                            string port = box_PortABoxInfo.Substring(2, 2);
                            string boxNumber = box_PortABoxInfo.Substring(4, 6);
                            switch (port)
                            {
                                case "00":
                                    connectedToPort = "A";
                                    break;
                                case "01":
                                    connectedToPort = "B";
                                    break;
                                case "02":
                                    connectedToPort = "C";
                                    break;
                                case "03":
                                    connectedToPort = "D";
                                    break;
                                default:
                                    connectedToPort = "??";
                                    break;
                            }

                            connectedToBoxId = int.Parse(boxNumber, System.Globalization.NumberStyles.HexNumber);
                            if (connectedToPort == "A" || connectedToPort == "B" || connectedToPort == "C" || connectedToPort == "D")
                            {
                                previousPort = GetBoxNameFromId(connectedToBoxId) + " : " + connectedToPort;
                            }
                        }
                        else
                        {
                            ownerB_name = parent_path;
                            connectedToPort = "";
                            connectedToBoxId = 0;
                            previousPort = device.Name.ToString() + " : Master";
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
                string description = "";
                try
                {
                    description = boxItem.Type;
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
                bool containsSubBoxes = false;
                int subBoxesCount = 0;

                bool infoDataAddr = false;
                bool infoDataDcTimes = false;
                bool infoDataNetId = false;
                bool infoDataSoeDS401 = false;
                bool infoDataState = true;
                bool infoObjectId = false;

                try
                {
                    TcSmBoxDefEtherCAT tcSmBoxDefEtherCAT = (TcSmBoxDefEtherCAT)boxItem;

                    if (tcSmBoxDefEtherCAT.InfoDataAddrSpecified && tcSmBoxDefEtherCAT.InfoDataAddr) infoDataAddr = true;
                    if (tcSmBoxDefEtherCAT.InfoDataDcTimesSpecified && tcSmBoxDefEtherCAT.InfoDataDcTimes) infoDataDcTimes = true;
                    if (tcSmBoxDefEtherCAT.InfoDataNetIdSpecified && tcSmBoxDefEtherCAT.InfoDataNetId) infoDataNetId = true;
                    if (tcSmBoxDefEtherCAT.InfoDataSoeDS401Specified && tcSmBoxDefEtherCAT.InfoDataSoeDS401) infoDataSoeDS401 = true;
                    if (tcSmBoxDefEtherCAT.InfoDataStateSpecified && !tcSmBoxDefEtherCAT.InfoDataState) infoDataState = false;
                    if (tcSmBoxDefEtherCAT.InfoDataObjectIdSpecified && tcSmBoxDefEtherCAT.InfoDataObjectId) infoObjectId = true;
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                if (box.Box != null)
                {
                    containsSubBoxes = true;
                    subBoxesCount = box.Box.Length;
                }
                else if (box.BusCoupler != null)
                {
                    if (box.BusCoupler.Term != null)
                    {
                        containsSubBoxes = true;
                        subBoxesCount = box.BusCoupler.Term.Length;
                    }
                }
                boxViewModel.BoxType = BoxTypes.EtherCAT;
                boxViewModel.MasterDeviceType = (DeviceTypes)device.DevType;
                boxViewModel.MasterDeviceName = device.Name;
                boxViewModel.MasterDeviceId = device.Id;
                boxViewModel.Name = box_name;
                boxViewModel.BoxOrderCode = box_order_code;
                boxViewModel.OwnerBname = ownerB_name;
                boxViewModel.Id = box_id;
                boxViewModel.ProductCode = box_product_code;
                boxViewModel.RevisionNo = box_revision_number;
                boxViewModel.PortABoxInfo = box_PortABoxInfo;
                boxViewModel.ConnectedToPort = connectedToPort;
                boxViewModel.ConnectedToBoxId = connectedToBoxId;
                boxViewModel.ConnectedToBox = "";
                boxViewModel.PreviousPort = previousPort;
                boxViewModel.ContainsSubBoxes = containsSubBoxes;
                boxViewModel.NumberOfSubBoxes = 0;
                boxViewModel.TotalNumberOfBoxes = 0;
                boxViewModel.NumberOfMapablePdos = 0;
                boxViewModel.TotalNumberOfPdos = 0;
                boxViewModel.InfoDataAddr = infoDataAddr;
                boxViewModel.InfoDataDcTimes = infoDataDcTimes;
                boxViewModel.InfoDataNetId = infoDataNetId;
                boxViewModel.InfoDataSoeDS401 = infoDataSoeDS401;
                boxViewModel.InfoDataState = infoDataState;
                boxViewModel.InfoDataObjectId = infoObjectId;
                boxViewModel.PortPhys = portPhys;
                boxViewModel.Physics= physics;
                boxViewModel.Description = description;

                ObservableCollection<PdoViewModel> allPdos = new ObservableCollection<PdoViewModel>();
                if (boxItem.Pdo != null)
                {
                    AppendSlotNameToPdo(ref box);
                    if (ValidatePdoNamesUniqueness(ref box))
                    {
                        allPdos = GetAllPdos(box, boxViewModel);
                    }
                }
                else if (box.BoxType != 0 && box.BusCoupler != null)
                {
                    if (box.Vars !=null && ValidateVarGrpNamesUniqueness(ref box))
                    {
                        allPdos = GetAllVarGrps(box, boxViewModel);
                    }
                }
                

                PdoViewModel WcState = GetBoxWcStateAsOneStructure(boxViewModel);
                if(WcState != null)
                {
                    if(allPdos == null) 
                    {
                        allPdos = new ObservableCollection<PdoViewModel>();
                    }
                    allPdos.Add(WcState);
                }

                PdoViewModel InfoData = GetBoxInfoDataAsOneStructure(box, boxViewModel);
                if(InfoData != null)
                {
                    if (allPdos == null)
                    {
                        allPdos = new ObservableCollection<PdoViewModel>();
                    }
                    allPdos.Add(InfoData);
                }

                PdoViewModel pdoViewModel = GetAllPdosAsOneStructure(boxViewModel, allPdos);
                if (pdoViewModel != null)
                {
                    boxViewModel.Pdos.Add(pdoViewModel);
                    boxViewModel.MapableObjects = new ObservableCollection<MappableObject>();
                    boxViewModel.MapableObjects.Add(pdoViewModel.MapableObject);
                }
                if (boxViewModel.HasFixedPdoStructure)
                {
                    TotalNumberOfFixedBoxes++;
                }
                else
                {
                    TotalNumberOfVariableBoxes++;
                }
                BoxIdentificationList.Add(new BoxIdentification() { Name = boxViewModel.Name, Id = boxViewModel.Id});
                TotalNumberOfBoxes++;
            }
            return boxViewModel;
        }
    }
}
