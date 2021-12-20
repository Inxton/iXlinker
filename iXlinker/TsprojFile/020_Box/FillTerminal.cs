using System;
using ViewModels;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel FillTerminal(TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path)
        {
            BoxViewModel boxViewModel = new BoxViewModel();
            ObservableCollection<PdoViewModel> allPdos = new ObservableCollection<PdoViewModel>();

            string ownerB_name = parent_path;

            TcSmTermDef term;

            if (box is TcSmTermDef)
            {
                term = (TcSmTermDef)box;

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
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
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
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                string box_term_type = "";
                try
                {
                    if (term.TermType != null)
                    {
                        uint type = uint.Parse(term.TermType.ToUpper().Replace("#X", ""), System.Globalization.NumberStyles.HexNumber);
                        uint subtype = 0;
                        if (type>=16777216)
                        {
                            subtype = type;
                            type = type % 16777216;
                            subtype = (subtype - type) / 16777216;
                            box_term_type = "KL" + type.ToString("0000") + "-" + subtype.ToString("0000");
                        }
                        else
                            box_term_type = "KL" + type.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                boxViewModel.BoxType = BoxTypes.EtherCAT;
                boxViewModel.MasterDeviceType = (DeviceTypes)device.DevType;
                boxViewModel.MasterDeviceName = device.Name;
                boxViewModel.MasterDeviceId = device.Id;
                boxViewModel.Name = box_name;
                boxViewModel.BoxOrderCode = box_term_type;
                boxViewModel.OwnerBname = ownerB_name;
                boxViewModel.Id = box_id;
                boxViewModel.NumberOfSubBoxes = 0;
                boxViewModel.TotalNumberOfBoxes = 0;
                boxViewModel.NumberOfMapablePdos = 0;
                boxViewModel.TotalNumberOfPdos = 0;

                if (box.Vars != null)
                {
                    if (ValidateVarGrpNamesUniqueness(ref box))
                    {
                        allPdos = GetAllVarGrps(box, boxViewModel);
                    }
                }
                PdoViewModel pdoViewModel = GetAllVarGrpsAsOneStructure(boxViewModel, allPdos);
                if (pdoViewModel != null)
                {
                    boxViewModel.Pdos.Add(pdoViewModel);
                    boxViewModel.MapableObjects = new ObservableCollection<MapableObject>();
                    boxViewModel.MapableObjects.Add(pdoViewModel.MapableObject);
                    TotalNumberOfBoxes++;
                }
            }
            return boxViewModel;
        }
    }
}
