using System;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private PdoViewModel FillPdoData(EtherCATSlavePdo pdo, BoxViewModel boxViewModel)
        {
            PdoViewModel pdoViewModel = new PdoViewModel();

            string pdo_name = "";
            try
            {
                if (pdo.Name != null)
                {
                    pdo_name = pdo.Name;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdo_index = "";
            uint pdo_index_number = 0;
            try
            {
                if (pdo.Index != null)
                {
                    pdo_index = pdo.Index;
                    pdo_index_number = uint.Parse(pdo_index.Replace("#x", ""), System.Globalization.NumberStyles.HexNumber);

                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdo_inout = "";
            try
            {
                if (pdo.InOut != null)
                {
                    pdo_inout = pdo.InOut;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdo_syncman = "";
            try
            {
                if (pdo.SyncMan != null)
                {
                    pdo_syncman = pdo.SyncMan;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string pdo_syncunit = "";
            try
            {
                if (pdo.SyncUnit != null)
                {
                    pdo_syncunit = pdo.SyncUnit;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            uint pdo_entry_count = 0;
            try
            {
                if (pdo.Entry != null)
                {
                    pdo_entry_count =(uint) pdo.Entry.Length;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            int box_id = 0;
            try
            {
                if (boxViewModel.Id != 0)
                {
                    box_id = boxViewModel.Id;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
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
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
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
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
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
                EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            pdoViewModel.Name = pdo_name;
            pdoViewModel.Index = pdo_index;
            pdoViewModel.IndexNumber = pdo_index_number;
            pdoViewModel.InOut = pdo_inout;
            pdoViewModel.SyncMan = pdo_syncman;
            pdoViewModel.SyncUnit = pdo_syncunit;
            pdoViewModel.OwnerBname = boxOwnerBname + tmpLevelSeparator + box_name;
            pdoViewModel.BoxOrderCode = boxOrderCode;

            return pdoViewModel;
        }
    }
}
