using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel AddTerminal(Solution vs, TcSmDevDef device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path, bool isIndependentProjectFile)
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


