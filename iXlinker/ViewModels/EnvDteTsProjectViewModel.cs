using System.Collections.ObjectModel;

namespace ViewModels
{
    public class EnvDteTsProjectViewModel : EnvDTEProjectViewModel
    {
        private ObservableCollection<EnvDtePlcProjectViewModel> plcProjects;
        public ObservableCollection<EnvDtePlcProjectViewModel> PlcProjects
        {
            get
            {
                return this.plcProjects ?? (this.plcProjects = new ObservableCollection<EnvDtePlcProjectViewModel>());
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
