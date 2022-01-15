using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoViewModel> GetAllVarGrps(IBox box, BoxViewModel boxViewModel)
        {
            ObservableCollection<PdoViewModel> pdos = new ObservableCollection<PdoViewModel>();


            bool wcStateWcState = true;
            bool wcStateInputToggle = false;
            bool hasFixedPdoStructure = true;

            //Collect all var grps
            foreach (TcSmVarGrpDef varGrp in box.Vars)
            {
                boxViewModel.TotalNumberOfPdos++;

                PdoViewModel pdoViewModel = CreatePdoFromVarGrp(varGrp, boxViewModel);
                if (pdoViewModel.SyncUnit != null)
                {
                    boxViewModel.SyncUnitDefinedOnAtLeastOnePdo = true;
                }

                if (pdoViewModel != null)
                {
                    pdos.Add(pdoViewModel);
                }
                
                if(pdoViewModel.InOut == null)
                {
                    wcStateInputToggle = true;
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

        private ObservableCollection<PdoViewModel> GetAllVarGrps(TcSmTermDef box, BoxViewModel boxViewModel)
        {
            ObservableCollection<PdoViewModel> pdos = new ObservableCollection<PdoViewModel>();
 
            //Collect all var grps
            foreach (TcSmVarGrpDef varGrp in box.Vars)
            {
                boxViewModel.TotalNumberOfPdos++;

                PdoViewModel pdoViewModel = CreatePdoFromVarGrp(varGrp, boxViewModel);
                if (pdoViewModel.SyncUnit != null)
                {
                    boxViewModel.SyncUnitDefinedOnAtLeastOnePdo = true;
                }

                if(pdoViewModel != null)
                {
                    pdos.Add(pdoViewModel);
                }
            }
            if (pdos.Count == 0)
            {
                pdos = null;
            }
            return pdos;
        }
    }
}
