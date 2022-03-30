using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using Utils;
using PlcprojFile;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoEntryViewModel FillPdoEntryData(EtherCATSlavePdoEntry pdoEntry, PdoViewModel pdoViewModel)

        {
            PdoEntryViewModel pdoEntryViewModel = new PdoEntryViewModel();

            string pdoEntryName = "";
            try
            {
                if (pdoEntry.Name != null)
                {
                    pdoEntryName = pdoEntry.Name;
                }

            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdoEntryIndex = "";
            uint pdoEntryIndexNumber = 0;
            try
            {
                if (pdoEntry.Index != null)
                {
                    pdoEntryIndex = pdoEntry.Index;
                    pdoEntryIndexNumber = uint.Parse(pdoEntryIndex.Replace("#x", ""), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }


            string pdoEntrySubIndex = "";
            uint pdoEntrySubIndexNumber = 0;
            try
            {
                if (pdoEntry.Sub != null)
                {
                    pdoEntrySubIndex = pdoEntry.Sub;
                    pdoEntrySubIndexNumber = uint.Parse(pdoEntrySubIndex.Replace("#x",""),System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdoEntryType_GUID = "";
            try
            {
                if (pdoEntry.Type.GUID != null)
                {
                    pdoEntryType_GUID = pdoEntry.Type.GUID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdoEntryType_Value = "";
            try
            {
                if (pdoEntry.Type.Value != null)
                {
                    pdoEntryType_Value = pdoEntry.Type.Value;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdoOwnerBname = "";
            try
            {
                if (pdoViewModel.OwnerBname != null)
                {
                    pdoOwnerBname = pdoViewModel.OwnerBname;
                }

            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdoEntryVarA = "";
            try
            {
                if (pdoEntryName.Contains("__"))
                {
                    pdoEntryVarA = ValidatePlcItem.Name(pdoViewModel.Name) + tmpLevelSeparator + ValidatePlcItem.Name(pdoEntryName.Replace("__", tmpLevelSeparator));
                }
                else
                {
                    pdoEntryVarA = ValidatePlcItem.Name(pdoViewModel.Name) + tmpLevelSeparator + ValidatePlcItem.Name(pdoEntryName);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdoEntryVarB = "";
            try
            {
                if (pdoEntryName.Contains("__"))
                {
                    pdoEntryVarB = pdoViewModel.Name + tmpLevelSeparator + pdoEntryName.Replace("__", tmpLevelSeparator);
                }
                else
                {
                    pdoEntryVarB = pdoViewModel.Name + tmpLevelSeparator + pdoEntryName;
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

            pdoEntryViewModel.Name = pdoEntryName;
            pdoEntryViewModel.Index = pdoEntryIndex;
            pdoEntryViewModel.IndexNumber = pdoEntryIndexNumber;
            pdoEntryViewModel.SubIndex = pdoEntrySubIndex;
            pdoEntryViewModel.SubIndexNumber = pdoEntrySubIndexNumber;
            pdoEntryViewModel.InOut = pdoViewModel.InOut;
            pdoEntryViewModel.Type_GUID = pdoEntryType_GUID;
            pdoEntryViewModel.Type_Value = pdoEntryType_Value;
            pdoEntryViewModel.SizeInBites = PlcBaseTypes.GetSizeInBites(pdoEntryViewModel.Type_Value);
            pdoEntryViewModel.SizeInBytes = PlcBaseTypes.GetSizeInBytes(pdoEntryViewModel.Type_Value);
            pdoEntryViewModel.OwnerBname = pdoOwnerBname;
            pdoEntryViewModel.VarB = pdoEntryVarB;
            pdoEntryViewModel.VarA = pdoEntryVarA;
            pdoEntryViewModel.BoxOrderCode = boxOrderCode;

            pdoEntryViewModel.Type_Value = iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntryViewModel);

            return pdoEntryViewModel;

        }
    }
}
