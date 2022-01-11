using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ClearMappings()
        {
            MappingsType mappings = new MappingsType();
            Tc.Mappings = mappings;
        }
    }
}
