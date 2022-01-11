using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel CreateBox(Solution vs,TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, IBox box, string parent_path)
        {
            return AddBox(vs,device, ref deviceVm, box, parent_path);
        }

        private BoxViewModel CreateBox(Solution vs,TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path)
        {
            return AddTerminal(vs, device, ref deviceVm, box, parent_path);           
        }
     }
}
