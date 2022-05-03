using iXlinkerDtos;
using Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfBoxBaseStructDoesNotExist(BoxStructViewModel actBoxStruct)
        {
            bool ret = true;
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (plcStruct.Name.Equals(EtcSlaveBaseStructNameFinal) && plcStruct.Namespace.Equals(plcLibContainingHwBaseName))
                {
                    actBoxStruct.Extends = plcLibContainingHwBaseName + "." + EtcSlaveBaseStructNameFinal;
                    ret = false;
                    break;
                }
            }

            foreach(BoxStructViewModel structVM in BoxStructures)
            {
                if (structVM.Name.Equals(EtcSlaveBaseStructNameFinal))
                {
                    ret = false;
                    break;
                }
            }
            if (string.IsNullOrEmpty(actBoxStruct.Extends))
            {
                actBoxStruct.Extends = EtcSlaveBaseStructNameFinal;
            }
            return ret;
        }
    }
}

