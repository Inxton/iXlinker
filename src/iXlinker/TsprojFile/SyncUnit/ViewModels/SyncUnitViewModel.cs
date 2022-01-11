using System.Collections.ObjectModel;

namespace iXlinkerDtos
{
    public class SyncUnitViewModel : NotifiableBase
    {

        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        private ObservableCollection<string> plcTasks;
        public ObservableCollection<string> PlcTasks
        {
            get { return this.plcTasks ?? (this.plcTasks = new ObservableCollection<string>()); }
            set
            {
                if (value != null)
                {
                    this.plcTasks = value;
                    NotifyPropertyChanged(nameof(PlcTasks));
                }
            }
        }
    }
}
