using iXlinker.Resources;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void FillStructureBaseResourseDictionary()
        {
            string pdoEntry = new PdoEntryStructViewModel().GetType().ToString();
            string pdo = new PdoStructViewModel().GetType().ToString();
            string box = new BoxStructViewModel().GetType().ToString();
            string device = new DeviceStructViewModel().GetType().ToString();
            string topology = new TopologyStructViewModel().GetType().ToString();


            StructureBasesResourceDictionary.Add(StructureBase.Build("*", box, "EtcSlaveTerminalBase", PlcStructuresInPlcLibraries).AddAttribute("{attribute clr[Container(Layout.Stack)]}").AddAttribute("{attribute clr[Group(Layout.GroupBox)]}").AddAttribute("{attribute addProperty PreviousPort \"Unknown\"}"));
            StructureBasesResourceDictionary.Add(StructureBase.Build("InfoData_23CBA837", pdo, "!", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("InfoData_6F19DB2B", pdo, "!", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("WcState_0B2B16F9", pdo, "!", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("WcState_9091E0EB", pdo, "!", PlcStructuresInPlcLibraries));
        }
    }
}
