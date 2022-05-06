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
            StructureBasesResourceDictionary.Add(StructureBase.Build("Channel_830843C1", pdo, "InputBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("Channel_42CA91F7", pdo, "OutputBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("AI_Standard_Channel_A8DF64E3", pdo, "InputBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("AI_Standard_Channel_4A394481", pdo, "InputBase", PlcStructuresInPlcLibraries));

            
        }
    }
}
