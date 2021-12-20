using ViewModels;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoViewModel CreatePdoFromVarGrp(TcSmVarGrpDef varGrp, BoxViewModel boxViewModel)
        {
            PdoViewModel pdoViewModel = FillVarGrpData(varGrp, boxViewModel);

            if (varGrp.Var != null)
            {

                ObservableCollection<PdoEntryViewModel> varsUnstructured = GetAllVarGrpVarsUnstructured(varGrp, pdoViewModel);
                pdoViewModel.PdoEntriesUnstructured = varsUnstructured;

                ObservableCollection<PdoEntryViewModel> varsStructured = GetAllVarGrpVarsStructured(varGrp, pdoViewModel, varsUnstructured);
                pdoViewModel.PdoEntriesStructured = varsStructured;

                MapableObject mapableObject = GetAllVarGrpVarsAsOneStructure(pdoViewModel, varsStructured);
                pdoViewModel.MapableObject = mapableObject;

            }

            return pdoViewModel;
        }
    }
}
