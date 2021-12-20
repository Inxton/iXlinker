using ViewModels;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoViewModel> GetAllPdos(IBox box, BoxViewModel boxViewModel)
        {
            ObservableCollection<PdoViewModel> pdos = new ObservableCollection<PdoViewModel>();

            TcSmBoxDefEtherCAT boxItem = (TcSmBoxDefEtherCAT)box.Item;

            bool wcStateWcState = false;
            bool wcStateInputToggle = false;
            bool hasFixedPdoStructure = true;

            //Collect all pdos
            foreach (EtherCATSlavePdo pdo in boxItem.Pdo)
            {
                wcStateWcState = true;
                boxViewModel.TotalNumberOfPdos++;
                if (pdo.SyncMan != null)
                {
                    PdoViewModel pdoViewModel = CreatePdo(pdo, boxViewModel);
                    if (pdoViewModel.SyncUnit != null)
                    {
                        boxViewModel.SyncUnitDefinedOnAtLeastOnePdo = true;
                    }

                    if (pdoViewModel != null)
                    {
                        pdos.Add(pdoViewModel);
                    }

                    if (pdo.InOut == null)
                    {
                        wcStateInputToggle = true;
                    }
                }
                if (pdo.ExcludePdo != null)
                {
                    hasFixedPdoStructure = false;
                }
            }

            boxViewModel.WcStateWcState = wcStateWcState;
            boxViewModel.WcStateInputToggle = wcStateInputToggle;
            boxViewModel.HasFixedPdoStructure = hasFixedPdoStructure;
            if (pdos.Count == 0)
            {
                pdos = null;
            }
            return pdos;
        }
    }
}
