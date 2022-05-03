using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddStructuresToPlcproj(Solution vs , ref Project tcPlcProj)
        {
            AddPdoEntryStructuresToPlcproj(vs, ref tcPlcProj);
            AddPdoStructuresToPlcproj(vs, ref tcPlcProj);
            AddBoxStructuresToPlcproj(vs, ref tcPlcProj);
            AddDeviceStructuresToPlcproj(vs, ref tcPlcProj);
            AddTopologyStructuresToPlcproj(vs, ref tcPlcProj);
            AddBaseStructuresToPlcproj(vs, ref tcPlcProj);
        }
    }
}
