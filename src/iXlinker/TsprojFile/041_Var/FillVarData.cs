using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using PlcprojFile;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoEntryViewModel FillVarData(TcSmVarDef varEntry, PdoViewModel pdoViewModel)

        {
            PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel();

            string varName = "";
            try
            {
                if (varEntry.Name != null)
                {
                    varName = varEntry.Name;
                }

            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string varType_GUID = "";
            try
            {
                if (varEntry.Type.GUID != null)
                {
                    varType_GUID = varEntry.Type.GUID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string varType_Value = "";
            try
            {
                if (varEntry.Type.Value != null)
                {
                    varType_Value = varEntry.Type.Value;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string varOwnerBname = "";
            try
            {
                if (pdoViewModel.OwnerBname != null)
                {
                    varOwnerBname = pdoViewModel.OwnerBname;
                }

            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string varEntryVarB = "";
            try
            {
                if (varName.Contains("__"))
                {
                    varEntryVarB = pdoViewModel.Name + tmpLevelSeparator + varName.Replace("__", tmpLevelSeparator);
                }
                else
                {
                    varEntryVarB = pdoViewModel.Name + tmpLevelSeparator + varName;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
            string boxOrderCode = "";
            try
            {
                if (pdoViewModel.BoxOrderCode != null)
                {
                    boxOrderCode = pdoViewModel.BoxOrderCode;
                }

            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string varInout = "";
            try
            {
                if (varEntry.InOutSpecified)
                {
                    if(varEntry.InOut == 0)
                    {
                        varInout = null;
                    }
                    else if (varEntry.InOut == 1)
                    {
                        varInout = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
            string pdoEntryVarA = ValidatePlcItem.Name(varEntryVarB).Replace('.', '_');

            pdoEntryViewModel.Name = varName;
            pdoEntryViewModel.InOut = varInout;
            pdoEntryViewModel.Type_GUID = varType_GUID;
            pdoEntryViewModel.Type_Value = varType_Value;
            //pdoEntryViewModel.TypeNamespace = "*";
            pdoEntryViewModel.OwnerBname = varOwnerBname;
            pdoEntryViewModel.VarB = varEntryVarB;
            pdoEntryViewModel.VarA = pdoEntryVarA;
            pdoEntryViewModel.BoxOrderCode = boxOrderCode;

            return pdoEntryViewModel;

        }
    }
}
