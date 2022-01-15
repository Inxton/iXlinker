using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private PdoViewModel FillVarGrpData(TcSmVarGrpDef varGrp, BoxViewModel boxViewModel)
        {
            PdoViewModel _varGrp = new PdoViewModel();

            string var_name = "";
            try
            {
                if (varGrp.Name != null)
                {
                    var_name = varGrp.Name;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            uint varGrpCount = 0;
            try
            {
                if (varGrp.Var != null)
                {
                    varGrpCount = (uint)varGrp.Var.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string box_name = "";
            try
            {
                if (boxViewModel.Name != null)
                {
                    box_name = boxViewModel.Name;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string boxOwnerBname = "";
            try
            {
                if (boxViewModel.OwnerBname != null)
                {
                    boxOwnerBname = boxViewModel.OwnerBname;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string boxOrderCode = "";
            try
            {
                if (boxViewModel.BoxOrderCode != null)
                {
                    boxOrderCode = boxViewModel.BoxOrderCode;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string varGrpType = "";
            try
            {
                if (varGrp.VarGrpTypeSpecified && varGrp.VarGrpType == 2 )
                {
                    _varGrp.InOut = "1";
                    _varGrp.InOutPlcProj = "AT %Q*";
                    _varGrp.InOutMappings = "Outputs";
                }
                else if (varGrp.VarGrpTypeSpecified && varGrp.VarGrpType == 1)
                {
                    _varGrp.InOut = null;
                    _varGrp.InOutPlcProj = "AT %I*";
                    _varGrp.InOutMappings = "Inputs";
                }
                else
                {
                    _varGrp.InOut = null;
                    _varGrp.InOutPlcProj = "AT %I*";
                    _varGrp.InOutMappings = "Inputs";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
            _varGrp.Name = var_name;
            _varGrp.OwnerBname = boxOwnerBname + tmpLevelSeparator + box_name;
            _varGrp.BoxOrderCode = boxOrderCode;

            return _varGrp;
        }
    }
}
