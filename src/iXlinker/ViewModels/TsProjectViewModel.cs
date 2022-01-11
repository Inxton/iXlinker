using System.Collections.ObjectModel;

namespace ViewModels
{
    public class TsProjectViewModel : ProjectItemViewModel
    {
        private ObservableCollection<PlcProjectViewModel> plcProjects;
        public ObservableCollection<PlcProjectViewModel> PlcProjects
        {
            get
            {
                return this.plcProjects ?? (this.plcProjects = new ObservableCollection<PlcProjectViewModel>());
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
