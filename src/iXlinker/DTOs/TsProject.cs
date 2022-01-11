using System.Collections.ObjectModel;

namespace iXlinkerDtos
{
    public class TsProject : ProjectItem
    {
        private ObservableCollection<PlcProject> plcProjects;
        public ObservableCollection<PlcProject> PlcProjects
        {
            get
            {
                return this.plcProjects ?? (this.plcProjects = new ObservableCollection<PlcProject>());
            }
            set
            {
                if (value != null)
                {
                    this.plcProjects = value;
                    NotifyPropertyChanged(nameof(PlcProjects));
                }
            }
        }
    }
}
