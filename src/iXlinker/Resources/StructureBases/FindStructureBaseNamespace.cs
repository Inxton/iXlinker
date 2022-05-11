using iXlinker.Resources;
using iXlinkerDtos;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        public string FindStructureBaseNamespace(string structureName)
        {
            string baseStructureNamespace = "";
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (plcStruct.Name.Equals(structureName))
                {
                    baseStructureNamespace = plcStruct.Namespace;
                    break;
                }
            }
            return baseStructureNamespace;
        }
    }
}
