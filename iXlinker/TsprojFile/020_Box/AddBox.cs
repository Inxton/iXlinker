using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel AddBox(SolutionViewModel vs,TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, IBox box, string parent_path)
        {
            BoxViewModel boxViewModel = new BoxViewModel();
            if ((!vs.DoNotGenerateDisabled || !box.DisabledSpecified || !box.Disabled) && box.BusCoupler == null)
            {
                boxViewModel = FillBoxData(device, ref deviceVm, box, parent_path);

                if (box.Box != null)
                {
                    string my_childs_path = boxViewModel.OwnerBname + tmpLevelSeparator + box.Name;
                    foreach (TcSmBoxDefBox sub_box in box.Box)
                    {
                        BoxViewModel subBoxViewModel = CreateBox(vs, device, ref deviceVm, sub_box, my_childs_path);
                        if (subBoxViewModel != null && subBoxViewModel.MapableObjectGrouped.Name != null && subBoxViewModel.MapableObjectGrouped.MapableItems.Count > 0)
                        {
                            boxViewModel.Boxes.Add(subBoxViewModel);
                            boxViewModel.MapableObjects.Add(subBoxViewModel.MapableObjectGrouped);
                        }
                        boxViewModel.NumberOfSubBoxes++;
                        boxViewModel.TotalNumberOfBoxes = boxViewModel.TotalNumberOfBoxes + subBoxViewModel.TotalNumberOfBoxes + 1;
                    }
                }
                boxViewModel.MapableObjectGrouped = GetAllMapableObjectsAsOneStructure(boxViewModel, boxViewModel.MapableObjects);
            }
            else if ((!vs.DoNotGenerateDisabled || !box.DisabledSpecified || !box.Disabled) && box.BusCoupler != null)
            {
                boxViewModel = FillBoxData(device, ref deviceVm, box, parent_path);

                if (box.BusCoupler != null && box.BusCoupler.Term != null)
                {
                    string my_childs_path = boxViewModel.OwnerBname + tmpLevelSeparator + box.Name;
                    foreach (TcSmTermDef sub_box in box.BusCoupler.Term)
                    {
                        BoxViewModel subBoxViewModel = CreateBox(vs, device, ref deviceVm, sub_box, my_childs_path);
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
            }
            return boxViewModel;
        }
    }
}
