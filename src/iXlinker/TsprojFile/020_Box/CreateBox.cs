using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel CreateBox(Solution vs, TcSmDevDef device, ref DeviceViewModel deviceVm, IBox box, string parent_path, bool isIndependentProjectFile)
        {
            return AddBox(vs,device, ref deviceVm, box, parent_path, isIndependentProjectFile);
        }

        private BoxViewModel CreateBox(Solution vs, TcSmDevDef device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path, bool isIndependentProjectFile)
        {
            return AddTerminal(vs, device, ref deviceVm, box, parent_path, isIndependentProjectFile);           
        }
     }
}
