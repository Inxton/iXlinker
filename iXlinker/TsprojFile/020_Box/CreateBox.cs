using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private BoxViewModel CreateBox(TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, IBox box, string parent_path)
        {
            return AddBox(device, ref deviceVm, box, parent_path);
        }

        private BoxViewModel CreateBox(TcSmProjectProjectIODevice device, ref DeviceViewModel deviceVm, TcSmTermDef box, string parent_path)
        {
            return AddTerminal(device, ref deviceVm, box, parent_path);           
        }
     }
}
