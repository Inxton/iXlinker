using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel AddTerminal(SolutionViewModel vs,TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path)
        {
            BoxViewModel boxViewModel = new BoxViewModel();

            if ((!vs.DoNotGenerateDisabled || !box.DisabledSpecified || !box.Disabled))
            {
                boxViewModel = FillBoxData(device, ref deviceVm, box, parent_path);
            }
            boxViewModel.MapableObjectGrouped = GetAllMapableObjectsAsOneStructure(boxViewModel, boxViewModel.MapableObjects);
            return boxViewModel;
        }
    }
}


