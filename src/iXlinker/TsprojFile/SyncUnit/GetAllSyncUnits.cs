using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private ObservableCollection<SyncUnitViewModel> GetAllSyncUnits(TcSmProjectProjectIODevice device)
        {
            ObservableCollection<SyncUnitViewModel> syncUnitViewModels = new ObservableCollection<SyncUnitViewModel>();

            SyncUnitViewModel syncUnitViewModel = new SyncUnitViewModel() { Name = "<default>", PlcTasks = new ObservableCollection<string>() { "<unreferenced>" } };
            
            foreach (TaskViewModel taskViewModel in Tasks)
            {
                syncUnitViewModel.PlcTasks.Add(taskViewModel.Name);
            }
            syncUnitViewModels.Add(syncUnitViewModel);

            if (device.Items != null)
            {
                try
                {
                    TcSmDevDefEtherCAT dev = (TcSmDevDefEtherCAT) device.Items[0];
                    if (dev.SyncUnit != null)
                    {

                        foreach (TcSmDevDefEtherCATSyncUnit su in dev.SyncUnit)
                        {
                            syncUnitViewModel = new SyncUnitViewModel() { Name = su.Name ?? "", PlcTasks = new ObservableCollection<string>() { "<unreferenced>" } };

                            foreach (TaskViewModel taskViewModel in Tasks)
                            {
                                syncUnitViewModel.PlcTasks.Add(taskViewModel.Name);
                            }
                            syncUnitViewModels.Add(syncUnitViewModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
            }
            return syncUnitViewModels;
        }
    }
}
