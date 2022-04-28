using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfBoxBaseStructDoesNotExist(BoxStructViewModel actBoxStruct)
        {
            bool ret = true;
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (plcStruct.Name.Equals(etcSlaveBaseStructName) && plcStruct.Namespace.Equals(plcLibContainingHwBaseName))
                {
                    actBoxStruct.Extends = plcLibContainingHwBaseName + "." + etcSlaveBaseStructName;
                    ret = false;
                    break;
                }
            }

            foreach(BoxStructViewModel structVM in BoxStructures)
            {
                if (structVM.Name.Equals(etcSlaveBaseStructName))
                {
                    ret = false;
                    break;
                }
            }
            if (string.IsNullOrEmpty(actBoxStruct.Extends))
            {
                actBoxStruct.Extends = etcSlaveBaseStructName;
            }
            return ret;
        }
    }
}
