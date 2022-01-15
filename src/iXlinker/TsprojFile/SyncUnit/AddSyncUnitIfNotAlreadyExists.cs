using iXlinkerDtos;
using System.Collections.ObjectModel;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddSyncUnitIfNotAlreadyExists(string SuName, ref DeviceViewModel deviceViewModel)
        {
            bool suAlreadyExists = false;
            foreach (SyncUnitViewModel syncUnit in deviceViewModel.SyncUnits )
            {
                if (syncUnit.Name.Equals(SuName))
                {
                    suAlreadyExists = true;
                    break;
                }
            }
            if (!suAlreadyExists)
            {
                SyncUnitViewModel syncUnitViewModel = new SyncUnitViewModel() { Name = SuName, PlcTasks = new ObservableCollection<string>() { "<unreferenced>" } };

                foreach (TaskViewModel taskViewModel in Tasks)
                {
                    syncUnitViewModel.PlcTasks.Add(taskViewModel.Name);
                }
                deviceViewModel.SyncUnits.Add(syncUnitViewModel);
            }
        }
    }
}
