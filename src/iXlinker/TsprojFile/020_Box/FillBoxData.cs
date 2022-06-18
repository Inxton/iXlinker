using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel FillBoxData(TcSmDevDef device, ref DeviceViewModel deviceVm, IBox box, string parent_path)
        {
           return FillBox(device, ref deviceVm, box, parent_path);
        }
        private BoxViewModel FillBoxData(TcSmDevDef device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path)
        {
            return FillTerminal(device, ref deviceVm, box, parent_path);
        }
    }
}
