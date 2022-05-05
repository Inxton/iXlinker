using System;
using System.IO;
using iXlinker.Resources;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;

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
            StructureBasesResourceDictionary.Add(StructureBase.Build("Channel_830843C1", pdo, "DigitalInputBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("Channel_42CA91F7", pdo, "DigitalOutputBase", PlcStructuresInPlcLibraries));
            //StructureBasesResourceDictionary.Add(StructureBase.Build("WcState_9091E0EB", pdo, "WcStateInputToggleBase", PlcStructuresInPlcLibraries));
            //StructureBasesResourceDictionary.Add(StructureBase.Build("InfoData_32EB74EA", pdo, "InfoDataAdsAddrStateBase", PlcStructuresInPlcLibraries));
            //StructureBasesResourceDictionary.Add(StructureBase.Build("WcState_0B2B16F9", pdo, "WcStateBase", PlcStructuresInPlcLibraries));


        }
    }
}
