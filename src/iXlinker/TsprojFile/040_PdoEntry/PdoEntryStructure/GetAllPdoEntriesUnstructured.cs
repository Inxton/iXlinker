using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoEntryViewModel> GetAllPdoEntriesUnstructured(EtherCATSlavePdo pdo, PdoViewModel pdoViewModel)
        {
            ObservableCollection<PdoEntryViewModel> pdoEntriesUnstructured = new ObservableCollection<PdoEntryViewModel>();
            if (pdo.Entry != null)
            {
                ValidatePdoEntryNamesUniqueness(ref pdo, pdoViewModel);

                foreach (EtherCATSlavePdoEntry pdoEntry in pdo.Entry)
                {
                    PdoEntryViewModel pdoEntryViewModel = CreatePdoEntry(pdoEntry, pdoViewModel);
                    if (pdoEntryViewModel.InOut == "1")
                    {
                        pdoViewModel.InOutPlcProj = "AT %Q*";
                        pdoViewModel.InOutMappings = "Outputs";
                    }
                    else
                    {
                        pdoViewModel.InOutPlcProj = "AT %I*";
                        pdoViewModel.InOutMappings = "Inputs";
                    }

                    if (pdoEntryViewModel.Index != null)
                    {
                        pdoEntriesUnstructured.Add(pdoEntryViewModel);
                    }
                }
            }
            else
            {
                pdoEntriesUnstructured = null;
            }
            if (pdoEntriesUnstructured != null && pdoEntriesUnstructured.Count == 0)
            {
                pdoEntriesUnstructured = null;
            }
            return pdoEntriesUnstructured;
        }
    }
}
