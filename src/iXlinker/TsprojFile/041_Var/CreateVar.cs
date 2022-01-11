using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private PdoEntryViewModel CreateVar(TcSmVarDef varEntry, PdoViewModel pdoViewModel)
        {
            PdoEntryViewModel pdoEntryViewModel = FillVarData(varEntry, pdoViewModel);

            TotalNumberOfPdoEntries++;

            return pdoEntryViewModel;
        }
    }
}
