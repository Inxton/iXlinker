using iXlinker.Utils;
using iXlinkerDtos;
using System;
using System.IO;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel AddBox(Solution vs, TcSmDevDef device, ref DeviceViewModel deviceVm, IBox box, string parent_path, bool isIndependentProjectFile)
        {
            BoxViewModel boxViewModel = new BoxViewModel();
            if ((!vs.DoNotGenerateDisabled || !box.DisabledSpecified || !box.Disabled) && box.BusCoupler == null)
            {
                boxViewModel = FillBoxData(device, ref deviceVm, box, parent_path);

                if (box.Box != null)
                {
                    string my_childs_path = boxViewModel.OwnerBname + tmpLevelSeparator + box.Name;
                    foreach (TcSmBoxDefBox _sub_box in box.Box)
                    {
                        TcSmBoxDef sub_box = _sub_box as TcSmBoxDef;
                        bool _isIndependentProjectFile = _sub_box.Name == null && _sub_box.File != null;
                        string path = Path.Combine(vs.TsProject.FolderPathInFileSystem, @"_Config\IO", my_childs_path.Replace(("TIID" + tmpLevelSeparator), "").Replace(tmpLevelSeparator,@"\"));

                        if (_isIndependentProjectFile)
                        {
                            sub_box = GetBoxFromXtiFile(path, _sub_box);
                            try
                            {
                                ((EtherCATSlave)sub_box.Item).PortABoxInfo = ((EtherCATSlave)_sub_box.Item).PortABoxInfo;
                            }
                            catch (Exception ex)
                            {
                                EventLogger.Instance.Logger.Error(@"Unable to discover PortABoxInfo for box: " + sub_box.Name
                                     + Environment.NewLine + @", in: " + path + "!!!"
                                     + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                            }
                        }

                        BoxViewModel subBoxViewModel = CreateBox(vs, device, ref deviceVm, sub_box, my_childs_path,_isIndependentProjectFile);
                        if (subBoxViewModel != null && subBoxViewModel.MapableObjectGrouped.Name != null) //&& subBoxViewModel.MapableObjectGrouped.MapableItems.Count > 0
                        {
                            boxViewModel.Boxes.Add(subBoxViewModel);
                            boxViewModel.MapableObjects.Add(subBoxViewModel.MapableObjectGrouped);
                        }
                        boxViewModel.NumberOfSubBoxes++;
                        boxViewModel.TotalNumberOfBoxes = boxViewModel.TotalNumberOfBoxes + subBoxViewModel.TotalNumberOfBoxes + 1;
                    }
                }
                boxViewModel.MapableObjectGrouped = GetAllMapableObjectsAsOneStructure(boxViewModel, boxViewModel.MapableObjects);
                boxViewModel.MapableObjectGrouped.PreviousPort = boxViewModel.PreviousPort;
                boxViewModel.MapableObjectGrouped.NameOrigin = boxViewModel.Name;
            }
            else if (box.BusCoupler == null)
            {
                string boxName = parent_path.Replace(("TIID" + tmpLevelSeparator), "").Replace(tmpLevelSeparator, ".") + "." + box.Name;
                string fileName = isIndependentProjectFile ? Path.Combine(Directory.GetParent(vs.TsProject.CompletePathInFileSystem).FullName.ToString(), @"_Config\IO", parent_path.Replace(("TIID"+tmpLevelSeparator),"").Replace(tmpLevelSeparator,@"\"), box.Name + ".xti") : vs.TsProject.CompletePathInFileSystem;
                EventLogger.Instance.Logger.Information("Disabled box: {0} found in the XAE project file: {1}!!!", boxName, fileName);
            }
            else if ((!vs.DoNotGenerateDisabled || !box.DisabledSpecified || !box.Disabled) && box.BusCoupler != null)
            {
                boxViewModel = FillBoxData(device, ref deviceVm, box, parent_path);

                if (box.BusCoupler != null && box.BusCoupler.Term != null)
                {
                    string my_childs_path = boxViewModel.OwnerBname + tmpLevelSeparator + box.Name;
                    foreach (TcSmTermDef sub_box in box.BusCoupler.Term)
                    {

                        BoxViewModel subBoxViewModel = CreateBox(vs, device, ref deviceVm, sub_box, my_childs_path, false); //TODO check if KL terminal can be saved in independent project file
                        if (subBoxViewModel !=null && subBoxViewModel.MapableObjectGrouped.Name != null && subBoxViewModel.MapableObjectGrouped.MapableItems.Count > 0)
                        {
                            boxViewModel.Boxes.Add(subBoxViewModel);
                            boxViewModel.MapableObjects.Add(subBoxViewModel.MapableObjectGrouped);
                        }
                        boxViewModel.NumberOfSubBoxes++;
                        boxViewModel.TotalNumberOfBoxes = boxViewModel.TotalNumberOfBoxes + subBoxViewModel.TotalNumberOfBoxes + 1;
                    }
                }
                boxViewModel.MapableObjectGrouped = GetAllMapableObjectsAsOneStructure(boxViewModel, boxViewModel.MapableObjects);
                boxViewModel.MapableObjectGrouped.PreviousPort = boxViewModel.PreviousPort;
                boxViewModel.MapableObjectGrouped.NameOrigin = boxViewModel.Name;
            }
            return boxViewModel;
        }
    }
}
