using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoEntryViewModel CreatePdoEntry(EtherCATSlavePdoEntry pdoEntry, PdoViewModel pdoViewModel)
        {
            PdoEntryViewModel pdoEntryViewModel = FillPdoEntryData(pdoEntry, pdoViewModel);

            if (pdoEntryViewModel.Index != null)
            {
                TotalNumberOfPdoEntries++;
            }

            return pdoEntryViewModel;
        }
    }
}
