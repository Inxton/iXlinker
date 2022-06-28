﻿using iXlinker.Resources;
using iXlinkerDtos;
using System.Collections.ObjectModel;

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
            string structureBase = new StructureBase("","","","", new ObservableCollection<PlcStruct>()).GetType().ToString();
            string KboxFilter = nameof(BoxViewModel.Physics) + "=K*";
            string YboxFilter = nameof(BoxViewModel.Physics) + "=Y*";
            string EndboxFilter = nameof(BoxViewModel.Physics) + "=null";

            StructureBasesResourceDictionary.Add(StructureBase.Build("EtcSlaveTerminalBase_947E5A46",   structureBase, "*", "EtcSlaveBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("EtcSlaveBoxBase_77A0E4A7", structureBase, "*", "EtcSlaveBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("EtcSlaveEndTerminalBase_866C7F0C", structureBase, "*", "EtcSlaveBase", PlcStructuresInPlcLibraries));
            StructureBasesResourceDictionary.Add(StructureBase.Build("*", box, KboxFilter, "EtcSlaveTerminalBase",  PlcStructuresInPlcLibraries).AddAttribute("{attribute clr[Container(Layout.Stack)]}").AddAttribute("{attribute clr[Group(Layout.GroupBox)]}").AddAttribute("{attribute addProperty PreviousPort \"Unknown\"}"));
            StructureBasesResourceDictionary.Add(StructureBase.Build("*", box, YboxFilter, "EtcSlaveBoxBase", PlcStructuresInPlcLibraries).AddAttribute("{attribute clr[Container(Layout.Stack)]}").AddAttribute("{attribute clr[Group(Layout.GroupBox)]}").AddAttribute("{attribute addProperty PreviousPort \"Unknown\"}"));
            StructureBasesResourceDictionary.Add(StructureBase.Build("EL90*", box, EndboxFilter, "EtcSlaveEndTerminalBase", PlcStructuresInPlcLibraries).AddAttribute("{attribute clr[Container(Layout.Stack)]}").AddAttribute("{attribute clr[Group(Layout.GroupBox)]}").AddAttribute("{attribute addProperty PreviousPort \"Unknown\"}"));

            StructureBasesResourceDictionary.Add(StructureBase.Build("InfoData_23CBA837", pdo, "*","!", PlcStructuresInPlcLibraries));  //The structureName is NOT extended by any baseStructure
            StructureBasesResourceDictionary.Add(StructureBase.Build("InfoData_6F19DB2B", pdo, "*","!", PlcStructuresInPlcLibraries));  //The structureName is NOT extended by any baseStructure
            StructureBasesResourceDictionary.Add(StructureBase.Build("WcState_0B2B16F9",  pdo, "*","!", PlcStructuresInPlcLibraries));  //The structureName is NOT extended by any baseStructure
            StructureBasesResourceDictionary.Add(StructureBase.Build("WcState_9091E0EB", pdo, "*", "!", PlcStructuresInPlcLibraries));  //The structureName is NOT extended by any baseStructure
            StructureBasesResourceDictionary.Add(StructureBase.Build("!", pdo, "*", "InputBase", PlcStructuresInPlcLibraries));         //Only the base structure is created
            StructureBasesResourceDictionary.Add(StructureBase.Build("!", pdo, "*", "OutputBase", PlcStructuresInPlcLibraries));        //Only the base structure is created
            StructureBasesResourceDictionary.Add(StructureBase.Build("!", topology, "*", "EtcMasterBase", PlcStructuresInPlcLibraries));//Only the base structure is created
        }
    }
}
