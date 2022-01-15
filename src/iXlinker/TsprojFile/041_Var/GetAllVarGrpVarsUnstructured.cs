using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<PdoEntryViewModel> GetAllVarGrpVarsUnstructured(TcSmVarGrpDef varGrp, PdoViewModel pdoViewModel)
        {

            ObservableCollection<PdoEntryViewModel> VarGrpVarsUnstructured = new ObservableCollection<PdoEntryViewModel>();

            ValidateVarNamesUniqueness(ref varGrp, pdoViewModel);
            //Collect all var grp vars
            foreach (TcSmVarDef varItem in varGrp.Var)
            {
                PdoEntryViewModel varVM = CreateVar(varItem, pdoViewModel);
                VarGrpVarsUnstructured.Add(varVM);
            }

            return VarGrpVarsUnstructured;
        }
    }
}
